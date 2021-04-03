using AutoMapper;
using EmployeeManagementSystem.EmployeeManagement.DAL;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services.EmployeeRepo
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeManagementContext databaseContext;
        private readonly IMapper mapper;
        private readonly IMailService mailService;
        private EmailModel emailModel = new EmailModel();
        public EmployeeRepository(EmployeeManagementContext _databaseContext, IMapper _mapper, IMailService _mailService)
        {
            databaseContext = _databaseContext;
            mapper = _mapper;
            mailService = _mailService;
        }

        public async Task<EmployeeModel> CreateEmployee(EmployeeModel empModel)
        {
            try
            {
                Employee employee = mapper.Map<Employee>(empModel);
                var responseResult = await databaseContext.Employee.AddAsync(employee);
                await databaseContext.SaveChangesAsync();
                //write logic to send mail
                SendMailOnEmployeeCreateOrDelete(responseResult.Entity, "Created");
                return mapper.Map<EmployeeModel>(responseResult.Entity);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteEmployeeById(int empId)
        {
            try
            {
                var employee = await databaseContext.Employee.Where(x => x.EmpId == empId).FirstOrDefaultAsync();
                if (employee != null)
                {
                    databaseContext.Employee.Remove(employee);
                    await databaseContext.SaveChangesAsync();
                    SendMailOnEmployeeCreateOrDelete(employee, "Deleted");
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeModel> UpdateEmployee(EmployeeModel employeeModel)
        {
            try
            {
                var employeeEntity = await databaseContext.Employee.FirstOrDefaultAsync(emp => emp.EmpId == employeeModel.EmpId);
                if (employeeEntity != null)
                {
                    employeeEntity.FirstName = employeeModel.FirstName;
                    employeeEntity.LastName = employeeModel.LastName;
                    employeeEntity.DeptId = employeeModel.DeptId;
                    employeeEntity.ManagerId = employeeModel.ManagerId;
                    databaseContext.Employee.Update(employeeEntity);
                    await databaseContext.SaveChangesAsync();
                    return mapper.Map<EmployeeModel>(employeeEntity);
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EmployeeDeptViewModel> GetEmployee(int empId)
        {
            try
            {
                var employeeEntity = await databaseContext.Employee.Include(dept => dept.Dept).Include(m => m.Manager).FirstOrDefaultAsync(emp => emp.EmpId == empId);
                return new EmployeeDeptViewModel()
                {
                    EmployeeId = employeeEntity.EmpId,
                    FirstName = employeeEntity.FirstName,
                    LastName = employeeEntity.LastName,
                    Email = employeeEntity.EmailId,
                    Department = employeeEntity.Dept.DepartmentName,
                    Manager = employeeEntity.Manager.FirstName
                };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public async Task<IEnumerable<EmployeeDeptViewModel>> GetAllEmployees()
        {
            try 
            {
                var employeesList = await  databaseContext.Employee.Include(dept => dept.Dept).Include(m => m.Manager)
                    .Select(emp => new EmployeeDeptViewModel
                    {
                        EmployeeId = emp.EmpId,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Email = emp.EmailId,
                        Department = emp.Dept.DepartmentName,
                        Manager = emp.Manager.FirstName
                    }).ToListAsync();

                return employeesList;
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        public async Task<object> GetEmployeeStatusById(int empId)
        {
            try
            {
                var employee = await databaseContext.Employee.Where(e => e.EmpId == empId).FirstOrDefaultAsync();
                //check whether any employees are reporting to this employee or not
                if (databaseContext.Employee.Where(e=>e.ManagerId == empId).Count() > 0)
                {
                    var result = databaseContext.Employee.Where(x => x.ManagerId == empId).GroupBy(m => m.ManagerId).Select(emp =>
                        new
                        {
                            empStatus = employee.ManagerId == null ? "Head" : (emp.Count() >= 1 ? "Manager" : "Asspciate")
                        }).ToString(); 

                    return new { EmployeeStatus = result };
                }
                else
                    return new { EmployeeStatus = "Associate" };
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmployeeDeptViewModel>> GetFilteredEmployees(int? empId, string department, string fName, string lName)
        {
            try
            {
                var employeesList = await databaseContext.Employee.Include(dept => dept.Dept).Include(m => m.Manager)
                   .Select(emp => new EmployeeDeptViewModel
                   {
                       EmployeeId = emp.EmpId,
                       FirstName = emp.FirstName,
                       LastName = emp.LastName,
                       Email = emp.EmailId,
                       Department = emp.Dept.DepartmentName,
                       Manager = emp.Manager.FirstName
                   }).ToListAsync();

                var resultSet = employeesList.Where(emp => (empId != null && empId != 0) ? emp.EmployeeId == empId : 
                (!string.IsNullOrEmpty(department)) ? emp.Department.Contains(department) : 
                (!string.IsNullOrEmpty(fName)) ? emp.FirstName.Contains(fName) :
                (!string.IsNullOrEmpty(lName)) ? emp.LastName.Contains(lName):
                emp.EmployeeId == emp.EmployeeId);
                return resultSet;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async  void SendMailOnEmployeeCreateOrDelete(Employee employee, string action)
        {
            try
            {
                emailModel.ToEmail = employee.EmailId;
                if (action == "Created")
                {
                    emailModel.Subject = "Registration Successfull!";
                    emailModel.Body = $"Hi {employee.FirstName}, your registration succefully completed";
                }
                else if (action == "Deleted")
                {
                    emailModel.Subject = "Employee record Deleted Successfull!";
                    emailModel.Body = $"Hi {employee.FirstName}, your record got deleted successfully";
                }
                await mailService.SendEmailAsync(emailModel);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
