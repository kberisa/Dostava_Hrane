using System;
using System.Text.RegularExpressions;
using AutoMapper;
using DostavaHrane.Models;
using DostavaHrane.Models.DTO;

namespace DostavaHrane.Mappers
{
    public class KosaricaMapper
    {

        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Kosarica, KosaricaDTO>()
                .ForMember(dest => dest.Kupac, act => act.MapFrom(src => src.Kupac!.KorisnickoIme)) //null-forgiving "!" operator   https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
                .ForMember(dest => dest.Proizvod, act => act.MapFrom(src => src.Proizvodi.Count));

            });
            var mapper = new Mapper(config);
            return mapper;
        }

        public static Mapper InitializeAutomapperKrace()
        {
            return new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Kosarica, KosaricaDTO>()
                    .ForMember(dest => dest.Proizvod, act => act.MapFrom(src => src.Proizvod!.Naziv)) //null-forgiving "!" operator   https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-forgiving
                    .ForMember(dest => dest.Kolicina, act => act.MapFrom(src => src.Proizvodi.Count));

                }));
        }
    }
}

