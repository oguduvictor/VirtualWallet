using AutoMapper;
using VirtualWallet.Application.Features.Products.Commands.CreateProduct;
using VirtualWallet.Application.Features.Products.Queries.GetAllProducts;
using VirtualWallet.Domain.Entities;

namespace VirtualWallet.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
