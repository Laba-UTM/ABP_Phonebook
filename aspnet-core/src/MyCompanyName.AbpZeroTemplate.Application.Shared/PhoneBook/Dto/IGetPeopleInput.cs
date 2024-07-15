using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Dto
{
    public interface IGetPeopleInput
    {
        string Filter { get; set; }
    }
}
