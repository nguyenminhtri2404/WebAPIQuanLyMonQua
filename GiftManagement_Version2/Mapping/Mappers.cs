using AutoMapper;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;

namespace GiftManagement_Version2.Mapping
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<UserRegisterRequest, User>();
            CreateMap<User, UserRespone>().ReverseMap();

            CreateMap<GiftRequest, Gift>();
            CreateMap<Gift, GiftRespone>().ReverseMap();

            CreateMap<PromotionRequest, Promotion>();
            CreateMap<Promotion, PromotionRespone>().ReverseMap();

            CreateMap<PermissionRequest, Permission>();
            CreateMap<Permission, PermissionRespone>().ReverseMap();

            CreateMap<RoleRequest, Role>();

            CreateMap<CartRequest, Cart>();
            CreateMap<Cart, CartRespone>().ReverseMap();


            CreateMap<Order, OrderRespone>().ReverseMap();

            CreateMap<Ranking, RankingRespone>().ReverseMap();


        }
    }
}
