using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services.EmailService
{
     public interface IMailService
    {
        Task SendEmailAsync(EmailModel emialModel);
    }
}
