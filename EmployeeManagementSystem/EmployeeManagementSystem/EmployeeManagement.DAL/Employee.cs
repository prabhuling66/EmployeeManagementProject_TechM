using System;
using System.Collections.Generic;

namespace EmployeeManagementSystem.EmployeeManagement.DAL
{
    public partial class Employee
    {
        public Employee()
        {
            InverseManager = new HashSet<Employee>();
        }

        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int? DeptId { get; set; }
        public int? ManagerId { get; set; }

        public virtual Department Dept { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }
    }
}
