using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using MyCompanyName.AbpZeroTemplate.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Dto
{
    public class GetPeopleInput : PagedAndSortedInputDto, IShouldNormalize, IGetPeopleInput
    {
        public string Filter { get; set; }
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
