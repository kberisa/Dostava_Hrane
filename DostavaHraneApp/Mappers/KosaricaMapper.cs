using AutoMapper;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;

namespace DostavaHrane.Mappers
{
    public class KosaricaMapper
    {
        public static IMapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kosarica, KosaricaDTO>()
                    .ForMember(dest => dest.Kupac, act => act.MapFrom(src => src.Kupac.KorisnickoIme))
                    .ForMember(dest => dest.Proizvod, act => act.MapFrom(src => src.Proizvod.Naziv));
            });

            return new Mapper(config);
        }

        public static IMapper InitializeAutomapperKrace()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kosarica, KosaricaDTO>()
                    .ForMember(dest => dest.Proizvod, act => act.MapFrom(src => src.Proizvod.Naziv))
                    .ForMember(dest => dest.Kolicina, act => act.MapFrom(src => src.Kolicina));
            });

            return new Mapper(config);
        }
    }
}