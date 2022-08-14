using AutoMapper;
using iLocker.EmailService.Data.Entities;

namespace iLocker.EmailService.Services.Internal
{
    public class EmailAccountProfile : Profile
    {
        public EmailAccountProfile()
        {
            CreateMap<iLocker.EmailService.DomainModels.EmailAccount, EmailAccount>().ReverseMap();
        }
    }

    public class QueuedEmailProfile : Profile
    {
        public QueuedEmailProfile()
        {
            CreateMap<iLocker.EmailService.DomainModels.QueuedEmail, QueuedEmail>().ReverseMap();
        }
    }
}
