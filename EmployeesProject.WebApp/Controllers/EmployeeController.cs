using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesProject.EL;
using EmployeesProject.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EmployeesProject.WebApp.Controllers
{
    public class EmployeeController : Controller
    {
        private HelperServiceApi helperApi;
        private string UrlServiceEmployee = "Employee";

        public EmployeeController(IOptions<UriHelpers> configuration)
        {
        
            helperApi = new HelperServiceApi(configuration.Value);

        }

        // GET: EmployeeController
        public async Task<ActionResult> Index()
        {   var list =await helperApi.GetAll<Employee>(UrlServiceEmployee);
            return View(list);
        }
    }
}
