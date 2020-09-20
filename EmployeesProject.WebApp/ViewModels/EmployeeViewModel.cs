using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesProject.EL;
using EmployeesProject.Utils.Enums;
using EmployeesProject.Utils.Helpers;
using Microsoft.Extensions.Options;

namespace EmployeesProject.WebApp.ViewModels
{
    public class EmployeeViewModel
    {
        public HelperServiceApi helperApi;
        private string UrlServiceEmployee = "Employee";
        public Employee Employee { get; set; }
        public IEnumerable<Employee> Employees { get; set; }

        public TransactionEnum Transaction{ get;set;}
        public EmployeeViewModel()
        {

        }


        public EmployeeViewModel(IOptions<UriHelpers> configuration)
        {

            helperApi = new HelperServiceApi(configuration.Value);

        }

     

        public async Task Initialize(int id,TransactionEnum transactionEnum)
        {Transaction = transactionEnum;
            switch (transactionEnum) {
                case TransactionEnum.Insert:
                      Employees=await GetAll();
                    break;
                case TransactionEnum.Update:
                    break;
                case TransactionEnum.Get:
                    Employee = await GetById(id);
                    Employees = await GetAll();
                    break;
                case TransactionEnum.GetAll:
                    break;
                default:
                    break;
            }
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var employees = await helperApi.GetAll<Employee>(UrlServiceEmployee);
            return employees;
        }

        public async Task<Employee> Insert(Employee _employee){ 
                var employee =await helperApi.Post(UrlServiceEmployee,_employee);
                 return employee;
            }

        public async Task<Employee> GetById(int id){ 
                return null;
            }

        public async Task<object> Execute(){
            switch (Transaction) {
                case TransactionEnum.Insert:
                    return await Insert(Employee);
                case TransactionEnum.Update:
                     return await Insert(Employee);
                case TransactionEnum.Get:
                    return null;
        
                case TransactionEnum.GetAll:
                    return null;
              
                default:
                    return null;
            }
        }
    }
}
