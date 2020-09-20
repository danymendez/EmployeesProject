using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesProject.BL;
using EmployeesProject.EL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EmployeesProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class EmployeeController : ControllerBase
    {
        private EmployeeBL EmployeeBL;
        public EmployeeController(IConfiguration configuration){ 
                EmployeeBL = new EmployeeBL(configuration);
            }

        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<Employee> Get()
        {  
           var ListEntity = EmployeeBL.GetAll();
            return ListEntity;
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public Employee Get(int id)
        {
            var entity = EmployeeBL.Get(id);
            return entity;
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public Employee Post(Employee entity)
        {
               var InsertedEntity = EmployeeBL.Create(entity);
            return InsertedEntity;
        }

        // PUT api/<EmployeeController>/5
        [HttpPut]
        public Employee Put(Employee entity)
        {
             var updatedEntity  = EmployeeBL.Update(entity);
            return updatedEntity;
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public Employee Delete(int id)
        {
            var updatedEntity  = EmployeeBL.Delete(id);
            return updatedEntity;
        }
    }
}
