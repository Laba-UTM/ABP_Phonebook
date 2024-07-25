using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.Migrations;
using MyCompanyName.AbpZeroTemplate.PhoneBook.Dto;
using NUglify.Helpers;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook
{
    public class DepartmentAppService : AbpZeroTemplateAppServiceBase, IDepartmentAppService
    {
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentAppService(IRepository<Department> departmentRepository) 
        {
            _departmentRepository = departmentRepository;
        }

        public void AddDepartment(DepartmentDto department)
        {
            _departmentRepository.Insert(new 
                    Department {
                        DeptNumber = department.DeptNumber,
                        DeptName = department.DeptName,
                        CreatedDate = department.CreatedDateFinal
            });
        }

        public void UpdateDepartment(DepartmentDto department)
        {
            var actualDepartment = _departmentRepository.FirstOrDefault(n=>n.DeptNumber == department.DeptNumber);

            if (actualDepartment == null)
            {
                throw new Exception($"Department with ID {actualDepartment.DeptNumber} not found.");
            }

            actualDepartment.DeptName = department.DeptName;
            actualDepartment.CreatedDate = department.CreatedDateFinal;

            _departmentRepository.Update(actualDepartment);
        }

        public List<DepartmentDto> GetDepartments()
        {
            var departments = _departmentRepository.GetAllList();
            var departmentsListDtos = ObjectMapper.Map<List<DepartmentDto>>(departments);
            return departmentsListDtos;
        }

        public DepartmentDto GetDepartmentByNumber(int deptNumber)
        {
            if (_departmentRepository != null)
            {
                var department = _departmentRepository.FirstOrDefault(d => d.DeptNumber == deptNumber);
                var departmentDto = ObjectMapper.Map<DepartmentDto>(department);
                return departmentDto;
            }
            else return null;
        }

        public void AddRangeDepartment(IEnumerable<DepartmentDto> departmentDtos) 
        {
            foreach (var dep in departmentDtos)
            {
                _departmentRepository.Insert(new
                    Department
                {
                    DeptNumber = dep.DeptNumber,
                    DeptName = dep.DeptName,
                    CreatedDate = dep.CreatedDateFinal
                });
            }
            var all = GetDepartments();
        }
        public void UpdateRangeDepartment(IEnumerable<DepartmentDto> departmentDtos)
        {
            foreach (var dep in departmentDtos)
            {
                var currentDepartment = _departmentRepository.FirstOrDefault(n => n.DeptNumber == dep.DeptNumber);

                if (currentDepartment == null)
                {
                    throw new Exception($"Department with ID {currentDepartment.DeptNumber} not found.");
                }

                currentDepartment.DeptName = dep.DeptName;
                currentDepartment.CreatedDate = dep.CreatedDateFinal;

                _departmentRepository.Update(currentDepartment);
            }
        }

        public List<DepartmentDto> MapDepCSVToDep(List<DepartmentDtoCSVReader> depCSV) 
        {
            return ObjectMapper.Map<List<DepartmentDto>>(depCSV);
        }
    }
}
