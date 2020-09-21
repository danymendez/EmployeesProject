using EmployeesProject.DAL;
using EmployeesProject.EL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesProject.BL
{
   public class EmployeeBL
    {
        
       private EmployeeDAL dal;
        public EmployeeBL(IConfiguration configuration){ 
               dal = new EmployeeDAL(configuration.GetConnectionString("EmployeeDbConn"));
            }

        /// <summary>
        /// Insert Employee in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Employee Create(Employee entity)
        {
            var Employee = dal.Create(entity);
            return Employee;
        }

         /// <summary>
        /// Insert Employees in database
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public List<Employee> CreateMany(List<Employee> employees)
        {  
            var Employees = dal.CreateMany(employees);
            return Employees;
        }

        /// <summary>
        /// Update Employee in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Employee Update(Employee entity)
        {
            var Employee = dal.Update(entity);
            return Employee;
        }


        /// <summary>
        /// Get Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee Get(int id)
        {
            var Employee = dal.Get(id);
            return Employee;

        }

        /// <summary>
        /// Get List of Employee
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Employee> GetAll()
        {
          var ListEmployee = dal.GetAll();
            return ListEmployee;
        }

        /// <summary>
        /// Delete Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee Delete(int id)
        {
            var job = dal.Delete(id);
            return job;
        }
    }
}
