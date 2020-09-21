using EmployeesProject.EL;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesProject.DAL
{
    public class EmployeeDAL
    {
        private EmployeeProjectContext dbcontext;

        public EmployeeDAL(string nameConString)
        {
            dbcontext = new EmployeeProjectContext(nameConString);
        }

        /// <summary>
        /// Insert Employee in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Employee Create(Employee entity)
        {
            dbcontext.Add(entity);
            dbcontext.SaveChanges();
            return entity;
        }

         /// <summary>
        /// Insert Employees in database
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public List<Employee> CreateMany(List<Employee> employees)
        {  
            dbcontext.AddRange(employees);
            dbcontext.SaveChanges();
            return employees;
        }

         /// <summary>
        /// Update Employee in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Employee Update(Employee entity)
        { var entityToModified = dbcontext.Employee.Find(entity.Id); 
            entityToModified.Nombres = entity.Nombres;
            entityToModified.Apellidos = entity.Apellidos;
            entityToModified.FechaNacimiento = entity.FechaNacimiento;
            entityToModified.DUI=entity.DUI;
            entityToModified.NIT = entity.NIT;
             entityToModified.ISSS = entity.ISSS;
              entityToModified.Telefono = entity.Telefono;
            dbcontext.Attach(entityToModified);
            dbcontext.SaveChanges();
            return entityToModified;
        }

        /// <summary>
        /// Get Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee Get(int id)
        {
            var entity = dbcontext.Employee.Find(id)??new Employee();
            return entity;

        }

        /// <summary>
        /// Get List of Employee
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Employee> GetAll()
        {
            var entity = dbcontext.Employee;
            return entity;
        }

        /// <summary>
        /// Delete Employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee Delete(int id)
        {
            Employee Employee = Get(id);
            if(Employee.Id!=0){
            dbcontext.Employee.Attach(Employee);
            dbcontext.Employee.Remove(Employee);
            dbcontext.SaveChanges();
            }
            return Employee;
        }
    
    }
}
