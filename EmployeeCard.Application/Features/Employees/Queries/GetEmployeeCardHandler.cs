using EmployeeCard.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeCard.Application.Features.Employees.Queries
{
    public class GetEmployeeCardHandler : IRequestHandler<GetEmployeeCardQuery, byte[]>
    {
        private readonly IEmployeeCardRepo _repo;

        // نقوم بحقن الـ Repository هنا
        public GetEmployeeCardHandler(IEmployeeCardRepo repo)
        {
            _repo = repo;
        }

        public async Task<byte[]> Handle(GetEmployeeCardQuery request, CancellationToken cancellationToken)
        {
            // الـ Handler يطلب من الـ Repository تنفيذ المهمة التقنية
            return _repo.GenerateEmployeeCardReport(request.EmployeeData);
        }
    }
}
