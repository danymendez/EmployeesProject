using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using EmployeesProject.EL;
using EmployeesProject.Utils.Enums;
using EmployeesProject.Utils.Helpers;
using EmployeesProject.WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
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

         public async Task<ActionResult> JsonForm(TransactionEnum transaction,int id=0)
        {   await employeeViewModel.Initialize(id,transaction);
            return Json(employeeViewModel);
        }

        [HttpPost]
         public async Task<ActionResult> Form(EmployeeViewModel _employeeViewModel)
        {  _employeeViewModel.helperApi = employeeViewModel.helperApi;
            await _employeeViewModel.Execute();

            return Json(employeeViewModel);
        }
     
         public async Task<ActionResult> Delete(int id)
        {  employeeViewModel.Employee= new Employee{ Id=id};
            employeeViewModel.Transaction = TransactionEnum.Delete;
            await employeeViewModel.Execute();

            return Json(employeeViewModel);
        }

        [HttpPost]
         public ActionResult AddFile(EmployeeViewModel _employeeViewModel)
        {  _employeeViewModel.helperApi = employeeViewModel.helperApi;
           

            return Json(_employeeViewModel.InsertExcelToDb(_employeeViewModel.FormFile));
        }

       
         public ActionResult ExportToExcel()
        {  string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "evaluación.xlsx";
            var content = employeeViewModel.ExportToExcel();
            return File(content, contentType, fileName);
        }

         public ActionResult ReportAge(int age)
        {  string contentType = "application/pdf";
            string fileName = "reportePruebaPractica.pdf";
            var content = employeeViewModel.ExportToPdf(new EL.Filters.EmployeeFilter{Age=age });
      
            return File(content, contentType, fileName);
        }

    }
}
