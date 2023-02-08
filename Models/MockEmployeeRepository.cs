using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeelist;

        public MockEmployeeRepository()
        {
            _employeelist = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Hassaan", Email = "Hassaan@gmail", Department = Dept.Comp_Science },
                new Employee() { Id = 2, Name = "Arqum", Email = "Arqum@gmail", Department = Dept.Urdu },
                new Employee() { Id = 3, Name = "Talha", Email = "Talha@gmail", Department =Dept.Pharmacy },
                new Employee() { Id = 4, Name = "Abdul Hadi", Email = "AbdulHadi@gmail", Department = Dept.Arts },
                new Employee() { Id = 5, Name = "Moiz", Email = "Moiz@gmail", Department = Dept.Maths },
                new Employee() { Id = 6, Name = "Ahsan", Email = "Ahsan@gmail", Department = Dept.Commerce },
                new Employee() { Id = 7, Name = "Ismail", Email = "Ismail@gmail", Department = Dept.Drawing },
                new Employee() { Id = 8, Name = "Taha", Email = "Taha@gmail", Department = Dept.Medical_Science }
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeelist.Max(e => e.Id) + 1;
            _employeelist.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee emp = _employeelist.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                _employeelist.Remove(emp);
            }

            return emp;
        }

        public IEnumerable<Employee> GetAllEmpployees()
        {
            return _employeelist;
        }


        public Employee GetEmployee(int Id)
        {
            return _employeelist.FirstOrDefault(e => e.Id == Id);
            // lambda expression used. 

            // throw new NotImplementedException();
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee emp = _employeelist.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (emp != null)
            {
                emp.Name = employeeChanges.Name;
                emp.Department = employeeChanges.Department;
                emp.Email = employeeChanges.Email;
            }

            return emp;
        }
    }
}
