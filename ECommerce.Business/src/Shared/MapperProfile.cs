using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;

namespace ECommerce.Business;
public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserReadDTO>().ForMember(dest => dest.AddressReadDTOs, opt => opt.MapFrom(src => src.Addresses));
        CreateMap<UserCreateDTO, User>().ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.AddressCreateDTOs));
        CreateMap<UserUpdateDTO, User>().ForMember(dest => dest.Addresses, opt => opt.MapFrom(src => src.AddressUpdateDTOs));

        CreateMap<Address, AddressReadDTO>();
        CreateMap<AddressCreateDTO, Address>();
        CreateMap<AddressUpdateDTO, Address>();

        CreateMap<Category, CategoryReadDTO>();
        CreateMap<CategoryCreateDTO, Category>();
        CreateMap<CategoryUpdateDTO, Category>();

        CreateMap<Image, ImageReadDTO>();
        CreateMap<ImageCreateDTO, Image>();
        CreateMap<ImageUpdateDTO, Image>();

        CreateMap<ProductReadDTO,Product>();
        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.ImageReadDTOs, opt => opt.MapFrom(src => src.Images)); // Map Images

        CreateMap<ProductCreateDTO, Product>()
            // .ForMember(dest => dest.categoryReadDTO, opt => opt.MapFrom(src => src.Category.Name)) // Map Category Name
            // .ForMember(dest=>dest.Category,opt=>opt.MapFrom(src=>src.))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageCreateDTOs));
            
        CreateMap<ProductUpdateDTO, Product>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ImageUpdateDTOs)); 

        CreateMap<Image, ImageReadDTO>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url)); 

        CreateMap<PurchaseItem, PurchaseItemReadDTO>();
        CreateMap<PurchaseItemCreateDTO, PurchaseItem>();
        CreateMap<PurchaseItemUpdateDTO, PurchaseItem>();

        CreateMap<Purchase, PurchaseReadDTO>();
        CreateMap<PurchaseCreateDTO, Purchase>()
        .ForMember(dest=>dest.PurchaseItems,opt=>opt.MapFrom(src=>src.PurchaseItemCreateDTOs));
        CreateMap<PurchaseUpdateDTO, Purchase>();
  
        

        CreateMap<Review, ReviewReadDTO>();
        CreateMap<ReviewCreateDTO, Review>();
        CreateMap<ReviewUpdateDTO, Review>();
    }
}
