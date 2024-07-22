using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook.Dto
{
    public class DepartmentDto
    { 
        public virtual int DeptNumber { get; set; }
        public virtual string DeptName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}
