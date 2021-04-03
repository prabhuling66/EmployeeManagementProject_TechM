using AutoMapper;
using EmployeeManagementSystem.EmployeeManagement.DAL;
using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeModel, Employee>().ReverseMap();
        }
    }
}
