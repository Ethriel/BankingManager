using AutoMapper;
using BankingManager.Database.Model;

namespace BankingManager.Services.Model.Mapper
{
    public class BankAccountMapperProfile : Profile
    {
        public BankAccountMapperProfile()
        {
            CreateMap<BankAccount, BankAccountDTO>()
                .ForMember(dto => dto.Number, options => options.MapFrom(ba => ba.Number.ToString()))
                .ForMember(dto => dto.Currency, options => options.MapFrom(ba => ba.Currency.ToString()));

            CreateMap<BankAccountDTO, BankAccount>()
                .ForMember(ba => ba.Number, options => options.MapFrom(dto => Guid.Parse(dto.Number)))
                .ForMember(ba => ba.Currency, options => options.MapFrom(dto => Enum.Parse(typeof(Currency), dto.Currency)));
        }
    }
}
