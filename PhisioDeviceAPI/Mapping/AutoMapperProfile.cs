using AutoMapper;
using PhisioDeviceAPI.DTOs.User;
using PhisioDeviceAPI.DTOs.Products;
using PhisioDeviceAPI.Models;

namespace PhisioDeviceAPI.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Product, ProductListItemDto>()
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images.Select(i => i.Url)));
            CreateMap<Product, ProductDetailDto>()
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images.Select(i => i.Url)));
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForMember(d => d.ImageUrl, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.ImageUrl)));
        }
    }
}


