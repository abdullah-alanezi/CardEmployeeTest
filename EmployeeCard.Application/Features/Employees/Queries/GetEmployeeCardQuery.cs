using EmployeeCard.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR; // Add this using directive to resolve IRequest<>

namespace EmployeeCard.Application.Features.Employees.Queries
{
    public class GetEmployeeCardQuery : IRequest<byte[]>
    {
        public EmployeeCardDto EmployeeData { get; set; }

        public GetEmployeeCardQuery(EmployeeCardDto employeeData)
        {
            EmployeeData = employeeData;
        }
    }
}
