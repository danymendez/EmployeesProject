using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using EmployeesProject.EL;
using EmployeesProject.Utils.Enums;
using EmployeesProject.Utils.Helpers;
using EmployeesProject.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EmployeesProject.WebApp.Controllers
{
    public class EmployeeController : Controller
    {
      
        private EmployeeViewModel employeeViewModel;

        
        public EmployeeController(IOptions<UriHelpers> configuration)
        {
        
           
            employeeViewModel = new EmployeeViewModel(configuration);

        }

        // GET: EmployeeController
        public async Task<ActionResult> Form(TransactionEnum transaction,int id=0)
        {   await employeeViewModel.Initialize(id,transaction);
            return View(employeeViewModel);
        }

        [HttpPost]
         public async Task<ActionResult> Form(EmployeeViewModel _employeeViewModel)
        {  _employeeViewModel.helperApi = employeeViewModel.helperApi;
            await _employeeViewModel.Execute();

            return View(employeeViewModel);
        }
    }
}
