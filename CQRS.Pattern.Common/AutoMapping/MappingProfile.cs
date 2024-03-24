using AutoMapper;
using CQRS.Pattern.Common.Dtos;
using CQRS.Pattern.Infastructure.Models;

namespace CQRS.Pattern.Common.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();

            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();

            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceDto, Invoice>();

            CreateMap<TransactionDto, Transaction>();
            CreateMap<Transaction, TransactionDto>();

        }
    }
}
