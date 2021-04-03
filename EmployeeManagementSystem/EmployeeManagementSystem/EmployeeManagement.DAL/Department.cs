﻿using System;
using System.Collections.Generic;

namespace EmployeeManagementSystem.EmployeeManagement.DAL
{
    public partial class Department
    {
        public Department()
        {
            Employee = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}