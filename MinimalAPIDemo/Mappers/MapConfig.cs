using AutoMapper;
using MinimalAPIDemo.Models;
using MinimalAPIDemo.Models.DTO;

namespace MinimalAPIDemo.Mappers
{
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<CouponCreateDTO, Coupon>().ReverseMap();
            CreateMap<Coupon, CouponCreateResponse>().ReverseMap();
        }
    }
}
