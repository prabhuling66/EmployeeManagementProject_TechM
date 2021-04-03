using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services.EmployeeRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository empRepository;

        public EmployeeController(IEmployeeRepository _employeeRepository)
        {
            empRepository = _employeeRepository;
        }

        //Get All Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDeptViewModel>>> GetAllEmployees()
        {
            try
            {
                var result = await empRepository.GetAllEmployees();
                return Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Unable to retrive the employees data");
            }
        }

        //Get Employee by employeeId
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDeptViewModel>> GetEmployee(int id)
        {
            try
            {
                return (await empRepository.GetEmployee(id));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Unable to retrive the employee data");
            }
        }

        //Create new Employee
        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> CreateEmployee(EmployeeModel employee)
        {
            try
            {
                var responseResult = await empRepository.CreateEmployee(employee);
                return responseResult;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     "Unable to create new employee record");
            }
        }

        //Update existed employee
        [HttpPut]
        public async Task<ActionResult<EmployeeModel>> UpdateEmployee(EmployeeModel employee)
        {
            try
            {
                var updatedEmployee = await empRepository.UpdateEmployee(employee);
                return updatedEmployee;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
            }
        }

        //Delete Employee by Id
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteEmployee(int id)
        {
            try
            {
                var result = await empRepository.DeleteEmployeeById(id);
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error deleting data");
            }
        }

        //Get Employee status
        [HttpGet("GetEmployeeStatusById")]
        public async Task<object> GetEmployeeStatusById(int id)
        {
            try
            {
                var result = await empRepository.GetEmployeeStatusById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }

        }

        //Search emoployee on filter
        [HttpGet("GetFilteredEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeDeptViewModel>>> GetFilteredEmployees(int? employeeId, string department, string firstName, string LastName)
        {
            try
            {
                var result = await empRepository.GetFilteredEmployees(employeeId, department, firstName, LastName);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");
            }
        }
    }
}
