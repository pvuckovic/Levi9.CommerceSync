using AutoMapper;
using Levi9.CommerceSync.Datas.Requests;
using Levi9.CommerceSync.Datas.Responses;

namespace Levi9.CommerceSync.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductResponse, ProductSyncRequest>()
                .ForMember(dest => dest.Price, opt =>
                {
                    opt.Condition(src => src.PriceList.Any(p => p.Currency == "EUR"));
                    opt.MapFrom(src => src.PriceList.FirstOrDefault(p => p.Currency == "EUR").PriceValue);
                })
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.ImageUrl));


            CreateMap<DocumentSyncResponse, DocumentSyncRequest>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<DocumentItemSyncResponse, DocumentItemSyncRequest>();
        }


    }
}
