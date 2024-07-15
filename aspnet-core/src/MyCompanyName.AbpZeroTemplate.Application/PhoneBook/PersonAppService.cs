using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.Authorization;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization.Users;
using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization.Users.Exporting;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Exporting;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook
{
    [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook)]
    public class PersonAppService : AbpZeroTemplateAppServiceBase, IPersonAppService
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Phone,long> _phoneRepository;
        private readonly IPersonPhoneBookListExcellExporter _personsPhoneListExcelExporter;


        public PersonAppService(IRepository<Person> personRepository, IRepository<Phone, long> phoneRepository,
            IPersonPhoneBookListExcellExporter personsPhoneListExcelExporter)
        {
            _personRepository = personRepository;
            _phoneRepository = phoneRepository; 
            _personsPhoneListExcelExporter = personsPhoneListExcelExporter;
        }

        private IQueryable<Person> GetPersonsFilteredQuery(IGetPeopleInput input)
        {
            var query = _personRepository
                .GetAll()
                .Include(p => p.Phones)
                .WhereIf(
                    !input.Filter.IsNullOrEmpty(),
                    p => p.Name.Contains(input.Filter) ||
                            p.Surname.Contains(input.Filter) ||
                            p.EmailAddress.Contains(input.Filter)
                );
            return query;
        }

        [HttpPost]
        public async Task<PagedResultDto<PersonListDto>> GetPeople(GetPeopleInput input)
        {
            var query = GetPersonsFilteredQuery(input);

            var personCount = await query.CountAsync();

            var persons = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var personListDtos = ObjectMapper.Map<List<PersonListDto>>(persons);

            return new PagedResultDto<PersonListDto>(
                personCount,
                personListDtos
            );
        }

        public async Task<FileDto> GetPeoplePhoneToExcel(GetPeopleToExcelInput input)
        {
            var query = GetPersonsFilteredQuery(input);

            var persons = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            var personListDto = ObjectMapper.Map<List<PersonListDto>>(persons);
           
            return _personsPhoneListExcelExporter.ExportToFile(personListDto);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_CreatePerson)]
        public async Task CreatePerson(CreatePersonInput input)
        {
            var person = ObjectMapper.Map<Person>(input);
            await _personRepository.InsertAsync(person);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_DeletePerson)]
        public async Task DeletePerson(EntityDto input)
        {
            await _personRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_EditPerson)]
        public async Task DeletePhone(EntityDto<long> input)
        {
            await _phoneRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_EditPerson)]
        public async Task<PhoneInPersonListDto> AddPhone(AddPhoneInput input)
        {
            var person = _personRepository.Get(input.PersonId);
            await _personRepository.EnsureCollectionLoadedAsync(person, p => p.Phones);

            var phone = ObjectMapper.Map<Phone>(input);
            person.Phones.Add(phone);

            //Get auto increment Id of the new Phone by saving to database
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<PhoneInPersonListDto>(phone);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_EditPerson)]
        public async Task<GetPersonForEditOutput> GetPersonForEdit(GetPersonForEditInput input)
        {
            var person = await _personRepository.GetAsync(input.Id);
            return ObjectMapper.Map<GetPersonForEditOutput>(person);
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_EditPerson)]
        public async Task EditPerson(EditPersonInput input)
        { 
            var person = await _personRepository.GetAsync(input.Id);
            person.Name = input.Name;
            person.Surname = input.Surname;
            person.EmailAddress = input.EmailAddress;
            await _personRepository.UpdateAsync(person);
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_PhoneBook_EditPerson)]
        public async Task<List<PhoneInPersonListDto>> GetPersonPhones(EntityDto input) 
        {
            var persons = GetPersonsFilteredQuery(new GetPeopleInput() {Filter=""});
            var person = persons.FirstOrDefault(i=>i.Id == input.Id);
            var result = ObjectMapper.Map<List<PhoneInPersonListDto>>(person.Phones);
            return result;
        }
    }
}
