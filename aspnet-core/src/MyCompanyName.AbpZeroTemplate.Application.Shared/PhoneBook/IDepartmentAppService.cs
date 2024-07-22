using Abp.Application.Services;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook
{
    public interface IDepartmentAppService : IApplicationService
    {
        List<DepartmentDto> GetDepartments();
        void AddDepartment(DepartmentDto department);
        void UpdateDepartment(DepartmentDto department);
        DepartmentDto GetDepartmentByNumber(int deptNumber);
    }
}
