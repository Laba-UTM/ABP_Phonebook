using System;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.PhoneBook
{
    public class Department : FullAuditedEntity
    {
        public virtual int Id { get; set; }
        public virtual int DeptNumber { get; set; }
        public virtual string DeptName { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}
