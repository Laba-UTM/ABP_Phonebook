using MyCompanyName.AbpZeroTemplate.Authorization.Users.Dto;
using MyCompanyName.AbpZeroTemplate.Dto;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Exporting
{
    public interface IPersonPhoneBookListExcellExporter
    {
        FileDto ExportToFile(List<PersonListDto> personListDtos);
    }
}
