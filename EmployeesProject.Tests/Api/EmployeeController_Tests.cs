using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EmployeesProject.EL;
using EmployeesProject.Tests.Utils;
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
                ISSS=0,
                NIT="",
                Telefono=""
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseInsert = await _client.PostAsJsonAsync(apiUrlEmployee, employee);
            var tupleInserted = await GetEntityResponse<Employee>(responseInsert);
            Assert.Equal(HttpStatusCode.OK, tupleInserted.Item1);
            Assert.NotNull(tupleInserted.Item2);


        }

        public async Task<Tuple<HttpStatusCode, T>> GetEntityResponse<T>(HttpResponseMessage response)
        {
            var forecast = JsonConvert.DeserializeObject<T>(
              await response.Content.ReadAsStringAsync()
            );
            return new Tuple<HttpStatusCode, T>(response.StatusCode, forecast);
        }
    }
}
