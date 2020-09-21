using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeesProject.EL;
using EmployeesProject.Utils.Enums;
using EmployeesProject.Utils.Helpers;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EmployeesProject.WebApp.ViewModels
{
    public class EmployeeViewModel
    {
        public HelperServiceApi helperApi;
        private string UrlServiceEmployee = "Employee";
        public Employee Employee { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IFormFile FormFile { get; set; }
        public TransactionEnum Transaction { get; set; }
        public EmployeeViewModel()
        {

        }


        public EmployeeViewModel(IOptions<UriHelpers> configuration)
        {

            helperApi = new HelperServiceApi(configuration.Value);

        }



        public async Task Initialize(int id, TransactionEnum transactionEnum)
        {
            Transaction = transactionEnum;
            switch (transactionEnum) {
                case TransactionEnum.Insert:
                    Employees = await GetAll();
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


        public bool InsertExcelToDb(IFormFile file)
        {


            List<Employee> employees = new List<Employee>();
            var fileName = file.FileName;
            // For .net core, the next line requires the NuGet package, 
            // System.Text.Encoding.CodePages
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = file.OpenReadStream()) {
                using (var reader = ExcelReaderFactory.CreateReader(stream)) {

                    //// reader.IsFirstRowAsColumnNames
                    var conf = new ExcelDataSetConfiguration {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration {
                            UseHeaderRow = false
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);

                    // Now you can get data from each sheet by its index or its "name"
                    var dataTable = dataSet.Tables[0];
                    Dictionary<string,int> dictIndexColumns = new Dictionary<string, int>();
                    for(int j = 0; j<dataTable.Columns.Count;j++){ 
                               dictIndexColumns.Add(dataTable.Rows[0][j].ToString(),j);
                               
                            }
                    for(int i = 1; i<dataTable.Rows.Count;i++){ 
                            employees.Add(
                                new Employee{ 
                                    Id=0,
                                    Nombres=dataTable.Rows[i][dictIndexColumns[nameof(Employee.Nombres)]].ToString(),
                                    Apellidos=dataTable.Rows[i][dictIndexColumns[nameof(Employee.Apellidos)]].ToString(),
                                    FechaNacimiento= DateTime.Now,
                                    DUI=dataTable.Rows[i][dictIndexColumns[nameof(Employee.DUI)]].ToString(),
                                    ISSS=0,
                                  NIT=dataTable.Rows[i][dictIndexColumns[nameof(Employee.NIT)]].ToString(),
                                     Telefono=dataTable.Rows[i][dictIndexColumns[nameof(Employee.Telefono)]].ToString(),
                                    }
                                );
                        }

                  var r =   CreateMany(employees).Result;
                }
            }

            return true;
        }

        public async Task<Employee> Insert(Employee _employee)
        {
            var employee = await helperApi.Post(UrlServiceEmployee, _employee);
            return employee;
        }
         public async Task<List<Employee>> CreateMany(List<Employee> _employees)
        {
            var employees = await helperApi.Post($"{UrlServiceEmployee}/PostMany", _employees);
            return employees;
        }

        public async Task<Employee> GetById(int id)
        {
            return null;
        }

        public async Task<object> Execute()
        {
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
