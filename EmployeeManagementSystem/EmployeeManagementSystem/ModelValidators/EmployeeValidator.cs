using EmployeeManagementSystem.EmployeeManagement.DAL;
using EmployeeManagementSystem.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.ModelValidators
{
    public class EmployeeValidator : AbstractValidator<EmployeeModel>
    {
        private readonly EmployeeManagementContext dbContext;
        public EmployeeValidator(EmployeeManagementContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(model => model.FirstName).NotEmpty()
                .WithMessage("Firs tName should be not empty.");
            RuleFor(model => model.LastName).NotEmpty()
                .WithMessage("Last Name should be not empty.");
            RuleFor(model=>model.EmailId).NotEmpty()
                 .WithMessage("Email should be not empty.")
                 .EmailAddress()
                 .WithMessage("{PropertyName} should be in valid email format.");
            RuleFor(emp => emp.ManagerId).NotEmpty()
                  .WithMessage("ManagerId should not be empty.")
                  .Must(IsNullManagerIdExisted)
                  .WithMessage("ManagerId cannot be null");
        }

        private bool IsNullManagerIdExisted(int? mangerId)
        {
            var result = dbContext.Employee.FirstOrDefault(emp => emp.ManagerId == null);
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
