using AutoMapper;
using Stone.BusinessEntities;
using Stone.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessServices.Config
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfiguration;

        public static void RegisterMappings()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Card, CardEntity>().ReverseMap();
                cfg.CreateMap<CardBrand, CardBrandEntity>().ReverseMap();
                cfg.CreateMap<CardType, CardTypeEntity>().ReverseMap();
                cfg.CreateMap<Transaction, TransactionEntity>().ReverseMap();
                cfg.CreateMap<TransactionType, TransactionTypeEntity>().ReverseMap();
            });
        }
    }
}
