using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using ClosedXML.Excel;
using EmployeesProject.EL;
using EmployeesProject.EL.Filters;
using EmployeesProject.Utils.Enums;
using EmployeesProject.Utils.Helpers;
using ExcelDataReader;
using FastMember;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
                        Employees = await GetAll();
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
                    Dictionary<string, int> dictIndexColumns = new Dictionary<string, int>();
                    for (int j = 0; j < dataTable.Columns.Count; j++) {
                        if (dataTable.Rows[0][j].ToString() == "Fecha de Nacimiento") {
                            dictIndexColumns.Add(nameof(Employee.FechaNacimiento), j);
                        }
                        else {
                            dictIndexColumns.Add(dataTable.Rows[0][j].ToString(), j);
                        }
                    }


                    for (int i = 1; i < dataTable.Rows.Count; i++) {
                        employees.Add(
                           new Employee {
                               Id = 0,
                               Nombres = dataTable.Rows[i][dictIndexColumns[nameof(Employee.Nombres)]].ToString(),
                               Apellidos = dataTable.Rows[i][dictIndexColumns[nameof(Employee.Apellidos)]].ToString(),
                               FechaNacimiento = Convert.ToDateTime(dataTable.Rows[i][dictIndexColumns[nameof(Employee.FechaNacimiento)]]),
                               DUI = dataTable.Rows[i][dictIndexColumns[nameof(Employee.DUI)]].ToString(),
                               ISSS = dataTable.Rows[i][dictIndexColumns[nameof(Employee.ISSS)]].ToString(),
                               NIT = dataTable.Rows[i][dictIndexColumns[nameof(Employee.NIT)]].ToString(),
                               Telefono = dataTable.Rows[i][dictIndexColumns[nameof(Employee.Telefono)]].ToString(),
                           }
                           );
                    }

                    employees = CreateMany(employees).Result;
                }
            }

            return !employees.Where(c => c.Id == 0).Any();
        }


        public byte[] ExportToExcel(EmployeeFilter filter = null)
        {
            var employees = GetAll().Result.ToList();
            if (filter != null) {
                employees = employees.Where(c => CalculateAge(c.FechaNacimiento) > filter.Age).ToList();
            }
            try {
                using (var workbook = new XLWorkbook()) {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Sheet1");
                    worksheet.Cell(1, 1).Value = nameof(Employee.Nombres);
                    worksheet.Cell(1, 2).Value = nameof(Employee.Apellidos);
                    worksheet.Cell(1, 3).Value = nameof(Employee.FechaNacimiento);
                    worksheet.Cell(1, 4).Value = nameof(Employee.DUI);
                    worksheet.Cell(1, 5).Value = nameof(Employee.NIT);
                    worksheet.Cell(1, 6).Value = nameof(Employee.ISSS);
                    worksheet.Cell(1, 7).Value = nameof(Employee.Telefono);
                    for (int index = 1; index <= employees.Count; index++) {
                        worksheet.Cell(index + 1, 1).Value =
                        employees[index - 1].Nombres;
                        worksheet.Cell(index + 1, 2).Value =
                        employees[index - 1].Apellidos;
                        worksheet.Cell(index + 1, 3).Value =
                        employees[index - 1].FechaNacimiento;
                        worksheet.Cell(index + 1, 4).Value =
                       employees[index - 1].DUI;
                        worksheet.Cell(index + 1, 5).Value =
                       employees[index - 1].NIT;
                        worksheet.Cell(index + 1, 6).Value =
                       employees[index - 1].ISSS;
                        worksheet.Cell(index + 1, 7).Value =
                       employees[index - 1].Telefono;
                    }
                    using (var stream = new MemoryStream()) {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return content;

                    }
                }
            }
            catch (Exception ex) {
                throw;
            }
        }

        public byte[] ExportToPdf(EmployeeFilter filter = null)
        {
            DataTable dtEmployee = new DataTable();
            var employees = GetAll().Result;
           
              byte[] getContent = new byte[1];
            if (filter != null) {
                employees = employees.Where(c => CalculateAge(c.FechaNacimiento) > filter.Age).ToList();
            }
             if (!employees.Any()) {
                var employ =new List<Employee>();
                employ.Add(new Employee{Nombres="",Apellidos="",DUI="",NIT="",ISSS="",Telefono="" });
                employees=employ;
            }
            using (var reader = ObjectReader.Create(employees)) {
                dtEmployee.Load(reader);
            }

            if (dtEmployee.Rows.Count > 0) {
                int pdfRowIndex = 1;

                string filename = "PruebasDetalle-" + DateTime.Now.ToString("dd-MM-yyyy hh_mm_s_tt");
                string filepath = "" + filename + ".pdf";
                var document = new iTextSharp.text.Document(PageSize.A4, 5f, 5f, 10f, 10f);
                document.AddTitle($"Reporte de Edad mayor a {filter.Age} años");
                MemoryStream fs = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
                Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);

                float[] columnDefinitionSize = { 2F, 2F, 2F, 2F, 2F, 2F, 2F  };
                PdfPTable table;
                PdfPCell cell;

                table = new PdfPTable(columnDefinitionSize) {
                    WidthPercentage = 100
                };

                cell = new PdfPCell {
                    BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0)
                };

                table.AddCell(new Phrase(nameof(Employee.Nombres), font1));
                table.AddCell(new Phrase(nameof(Employee.Apellidos), font1));
                table.AddCell(new Phrase(nameof(Employee.FechaNacimiento), font1));
                table.AddCell(new Phrase(nameof(Employee.DUI), font1));
                table.AddCell(new Phrase(nameof(Employee.NIT), font1));
                table.AddCell(new Phrase(nameof(Employee.ISSS), font1));
                table.AddCell(new Phrase(nameof(Employee.Telefono), font1));
                table.HeaderRows = 1;

                foreach (DataRow data in dtEmployee.Rows) {
                    table.AddCell(new Phrase(data[nameof(Employee.Nombres)].ToString(), font2));
                    table.AddCell(new Phrase(data[nameof(Employee.Apellidos)].ToString(), font2));
                    table.AddCell(new Phrase(data[nameof(Employee.FechaNacimiento)].ToString(), font2));
                    table.AddCell(new Phrase(data[nameof(Employee.DUI)].ToString(), font2));
                    table.AddCell(new Phrase(data[nameof(Employee.NIT)].ToString(), font2));
                    table.AddCell(new Phrase(data[nameof(Employee.ISSS)].ToString(), font2));
                    table.AddCell(new Phrase(data[nameof(Employee.Telefono)].ToString(), font2));

                    pdfRowIndex++;
                }

                document.Add(table);
                document.Close();
               
                writer.Close();
               
              
               
                 getContent = fs.ToArray();
                fs.Dispose();
 

                
            }
            return getContent;


        }

        private int CalculateAge(DateTime fechaNac)
        {

            var today = DateTime.Today;

            var age = today.Year - fechaNac.Year;


            if (fechaNac.Date > today.AddYears(-age)) age--;

            return age;
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
              var entity =await helperApi.Get<Employee>($"{UrlServiceEmployee}/",id);
          
           return entity;
        }

          public async Task<bool> Update(Employee _employee)
        {
              var entity = await helperApi.Put(UrlServiceEmployee, _employee);
          
           return entity;
        }

           public async Task<Employee> Delete(int id)
        {
               var entity =await helperApi.Delete<Employee>($"{UrlServiceEmployee}/",id);
          
           return entity;
        }

        public async Task<object> Execute()
        {
            switch (Transaction) {
                case TransactionEnum.Insert:
                    return await Insert(Employee);
                case TransactionEnum.Update:
                    return await Update(Employee);
                case TransactionEnum.Get:
                    return null;
                     case TransactionEnum.Delete:
                    return await Delete(Employee.Id);
               

                case TransactionEnum.GetAll:
                    return null;

                default:
                    return null;
            }
        }
    }
}
