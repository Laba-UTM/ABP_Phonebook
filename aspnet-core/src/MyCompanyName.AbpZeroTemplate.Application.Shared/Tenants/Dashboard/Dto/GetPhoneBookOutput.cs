using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.Tenants.Dashboard.Dto
{
    public class GetPhoneBookOutput
    {
        public string Name { get; set; }
        public List<PhoneInPersonListDto> BusinessPhones { get; set; }
        public List<PhoneInPersonListDto> MobilePhones { get; set; }
        public List<PhoneInPersonListDto> HomePhones { get; set; }
    }
}
