using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EmployeesProject.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace EmployeesProject.Tests.Utils
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Startup>
    {


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");
            builder.ConfigureAppConfiguration((context, conf) =>
            {

                conf.AddJsonFile(configPath);

            });

        }
    }
}
