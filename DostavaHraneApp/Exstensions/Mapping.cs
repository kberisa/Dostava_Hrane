using System;
using DostavaHrane.Mappers;
using DostavaHrane.Models.DTO;
using DostavaHrane.Models;

namespace DostavaHrane.Mappings
{
    public static class Mapping
    {
        public static List<KosaricaDTO> MapKosarica(this List<Kosarica> kosarica)
        {

            var mapper = KosaricaMapper.InitializeAutomapper();
            var vrati = new List<KosaricaDTO>();
            kosarica.ForEach(k =>
            {
                vrati.Add(mapper.Map<KosaricaDTO>(k));
            });
            return vrati;
        }
    }
}
