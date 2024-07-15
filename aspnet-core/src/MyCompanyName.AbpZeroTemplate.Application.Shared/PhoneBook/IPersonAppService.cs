using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using System.Threading.Tasks;
using MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto;
using System.Linq;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook
{
    public interface IPersonAppService : IApplicationService
    {
        Task<PagedResultDto<PersonListDto>> GetPeople(GetPeopleInput input);
        Task CreatePerson(CreatePersonInput input);
        Task DeletePerson(EntityDto input);
        Task DeletePhone(EntityDto<long> input);
        Task<PhoneInPersonListDto> AddPhone(AddPhoneInput input);


    }
}
