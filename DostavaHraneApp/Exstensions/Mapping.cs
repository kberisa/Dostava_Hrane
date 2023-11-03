using System;
using AutoMapper;
using DostavaHrane.Models.DTO;
using DostavaHrane.Models;
using DostavaHrane.Mappers;

namespace DostavaHrane.Mappings
{
    public static class Mapping
    {
        public static List<KosaricaDTO> MapKosarica(this List<Kosarica> kosarica)
        {
            var config = KosaricaMapper.InitializeAutomapper();
            var mapper = new Mapper((AutoMapper.IConfigurationProvider)config);
            var vrati = new List<KosaricaDTO>();
            kosarica.ForEach(k =>
            {
                vrati.Add(mapper.Map<KosaricaDTO>(k));
            });
            return vrati;
        }
    }
}
