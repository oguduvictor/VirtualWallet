using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Application.DTOs.Account;
using VirtualWallet.Application.DTOs.Email;
using VirtualWallet.Application.Enums;
using VirtualWallet.Application.Exceptions;
using VirtualWallet.Application.Interfaces.Services;
using VirtualWallet.Application.Wrappers;
using VirtualWallet.Domain.Settings;
using VirtualWallet.Infrastructure.Identity.Helpers;
using VirtualWallet.Infrastructure.Identity.Models;

namespace VirtualWallet.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            IOptions<JWTSettings> jwtSettings,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }

            var jwtSecurityToken = await GenerateJWToken(user);

            var response = new AuthenticationResponse
            {
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = GenerateRefreshToken(ipAddress).Token
            };

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<AuthenticationResponse>> RegisterAsync(RegisterRequest request, string ipAddress)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ApiException($"{result.Errors}");
            }

            await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());


            //TODO: Send Email 
            //var verificationUri = await SendVerificationEmail(user, origin);

            //await _emailService.SendAsync(new EmailRequest
            //{
            //    From = "mail@virtualwalletapi.com",
            //    To = user.Email,
            //    Body = $"Please confirm your account by visiting this URL {verificationUri}",
            //    Subject = "Confirm Registration"
            //});

            var jwtSecurityToken = await GenerateJWToken(user);

            var response = new AuthenticationResponse
            {
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = GenerateRefreshToken(ipAddress).Token
            };

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }

            throw new ApiException($"An error occured while confirming {user.Email}.");
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            if (account == null)
            {
                return;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);

            var route = "api/account/reset-password/";

            var _endpointUri = new Uri(string.Concat($"{origin}/", route));

            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}. Please reset your account by visiting this URL {_endpointUri}", //TODO: Check later. Not priority
                To = model.Email,
                Subject = "Reset Password",

            };

            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            if (account == null)
            {
                throw new ApiException($"No Accounts Registered with {model.Email}.");
            }

            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);

            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }

            throw new ApiException($"Error occured while reseting the password.");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", IpHelper.GetIpAddress())
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "api/account/confirm-email/";

            var _endpointUri = new Uri(string.Concat($"{origin}/", route));

            var verificationUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "userId", user.Id);

            return QueryHelpers.AddQueryString(verificationUri, "code", code);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
