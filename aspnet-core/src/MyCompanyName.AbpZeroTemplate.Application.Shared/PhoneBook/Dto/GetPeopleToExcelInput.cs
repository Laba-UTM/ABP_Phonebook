using Abp.Configuration;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Dto
{
    public class GetPeopleToExcelInput : IShouldNormalize ,IGetPeopleInput
    {
        public string Filter { get; set; }
        public string Sorting { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }

            Filter = Filter?.Trim();
        }
    }
}
