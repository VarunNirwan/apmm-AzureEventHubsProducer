using System;

namespace AzureEventHubs.Model
{
    public class Employees
    {
        public class Employee
        {
            //public List<Names> Name { get; set; }
            public string FullName { get; set; }
            public string Address { get; set; }
            public int Age { get; set; }
        }

        public class Names
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public Employee EmpDetails { get; set; }
        public Names EmpName { get; set; }
    }

}
