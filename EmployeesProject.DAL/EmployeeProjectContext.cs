using EmployeesProject.EL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeesProject.DAL
{
   public class EmployeeProjectContext:DbContext
    {
          public EmployeeProjectContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        public EmployeeProjectContext(DbContextOptions<EmployeeProjectContext> options)  
            :base(options)  
        {  
            Database.EnsureCreated();  
        }  

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }


        public DbSet<Employee> Employee { get; set; }
    }
}
