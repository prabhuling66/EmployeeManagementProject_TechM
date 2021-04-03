using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services.EmployeeRepo
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDeptViewModel>> GetAllEmployees();
        Task<EmployeeDeptViewModel> GetEmployee(int empId);
        Task<EmployeeModel> CreateEmployee(EmployeeModel employee);
        Task<EmployeeModel> UpdateEmployee(EmployeeModel employee);
        Task<bool> DeleteEmployeeById(int empId);
        Task<object> GetEmployeeStatusById(int empId);
        Task<IEnumerable<EmployeeDeptViewModel>> GetFilteredEmployees(int? empId, string department, string fName, string lName);
    }
}
