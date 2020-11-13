﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using VirtualWallet.Application.Interfaces.Services;

namespace VirtualWallet.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        }

        public string UserId { get; }
    }
}
