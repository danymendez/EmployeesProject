using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace EmployeesProject.Tests.Utils
{
    public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
    {
        

        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;
  

        public IntegrationTest(ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
         }
    }
}
