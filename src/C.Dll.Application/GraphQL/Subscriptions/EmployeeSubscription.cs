using Domain.Entities;
using HotChocolate;
using HotChocolate.Types;

namespace Application.GraphQl
{
    [ExtendObjectType(typeof(BaseSubscriptionType))]
    public class EmployeeSubscriptionType
    {
        [Subscribe]
        public Employee EmployeeAdded([EventMessage] Employee employee) => employee;

        [Subscribe]
        public Employee EmployeeRemoved([EventMessage] Employee employee) => employee;
        
        [Subscribe]
        public Employee EmployeeUpdated([EventMessage] Employee employee) => employee;
    }
}