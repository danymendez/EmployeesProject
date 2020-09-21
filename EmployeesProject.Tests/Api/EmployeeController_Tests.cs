using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EmployeesProject.EL;
using EmployeesProject.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace EmployeesProject.Tests.Api
{
   public class EmployeeController_Tests: IntegrationTest
    {
        private const string apiUrlEmployee = "/api/Employee";
        public EmployeeController_Tests(ApiWebApplicationFactory fixture)
          : base(fixture) { }

        [Fact]
        public async Task Insert_Should_Return_Employee()
        {
            var employee = new Employee
            {
                Id = 0,
                Nombres = "Nombre uno",
                Apellidos = "Nombre dos",
                DUI = "00000",
                FechaNacimiento = DateTime.Now,
                ISSS="000",
                NIT="000",
                Telefono="0000"
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseInsert = await _client.PostAsJsonAsync(apiUrlEmployee, employee);
            var tupleInserted = await GetEntityResponse<Employee>(responseInsert);
            Assert.True(HttpStatusCode.OK == tupleInserted.Item1, tupleInserted.Item3);
            Assert.NotNull(tupleInserted.Item2);


        }
         [Fact]
        public async Task GetAll_Should_Return_Employee()
        {

            var response = await _client.GetAsync(apiUrlEmployee);
            var tuple = await GetEntityResponse<Employee[]>(response);
            Assert.Equal(HttpStatusCode.OK, tuple.Item1);
            Assert.NotNull(tuple.Item2);
        }

        public async Task<Tuple<HttpStatusCode, T, string>> GetEntityResponse<T>(HttpResponseMessage response)
        {

            var bodyResponseString = 
                await response.Content.ReadAsStringAsync();
 

            var forecast = JsonConvert.DeserializeObject<T>(
              await response.Content.ReadAsStringAsync()
            );
            return new Tuple<HttpStatusCode, T,string>(response.StatusCode, forecast,bodyResponseString);
        }
    }
}
