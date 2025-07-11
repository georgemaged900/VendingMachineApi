using AutoMapper;
using FlapKapBackendChallenge.Dto;
using FlapKapBackendChallenge.Models;

namespace FlapKap
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateProductRequestDto,Product>();
            CreateMap<User, RegisterResponseDto>();
            CreateMap<UpdateProductRequestDto, Product>();

        }
    }
}
