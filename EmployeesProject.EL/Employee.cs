using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EmployeesProject.EL
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{ get; set;}
         [Display(Name ="Nombres",Prompt ="Ingrese el campo Nombres")]
        [Required(ErrorMessage ="El campo Nombres es requerido.")]
        public string Nombres { get; set;}
         [Display(Name ="Apellidos",Prompt ="Ingrese el campo Apellidos")]
        [Required(ErrorMessage ="El campo Apellidos es requerido.")]
        public string Apellidos { get; set; }
        [Display(Name ="Fecha de nacimiento",Prompt ="Ingrese el campo Fecha de nacimiento")]
        [Required(ErrorMessage ="El campo Fecha de nacimiento es requerido.")]
        public DateTime FechaNacimiento { get; set; }
          [Display(Name ="DUI",Prompt ="Ingrese el campo DUI, Ej. 00000000-0")]
        [Required(ErrorMessage ="El campo DUI es requerido.")]
         [RegularExpression(@"\d{8}-\d{1}$", 
         ErrorMessage = "El formato del campo DUI no es correcto, Ej. 00000000-0")]
        public string DUI { get; set; }
          [Display(Name ="NIT",Prompt ="Ingrese el campo NIT, Ej. 9105-000000-518-6")]
        [Required(ErrorMessage ="El campo NIT es requerido.")]
         [RegularExpression(@"\d{4}-\d{6}-\d{3}-\d{1}$", 
         ErrorMessage = "El formato del campo NIT no es correcto, Ej. 9105-000000-518-6")]
        public string NIT { get; set; }
          [Display(Name ="ISSS",Prompt ="Ingrese el campo ISSS, Ej. 012345678")]
        [Required(ErrorMessage ="El campo ISSS es requerido.")]
       
        [StringLength(9, MinimumLength=2,ErrorMessage ="Longitud de campo no válida mínimo 2 caracteres, máximo 9.")]
        public string ISSS { get; set; }
        [Display(Name ="Teléfono",Prompt ="Ingresar Teléfono Ej. (+503) 2222-2222")]
        [Required(ErrorMessage ="El campo Teléfono es requerido.")]
         [RegularExpression(@"\(\+503\) \d{4}-\d{4}$", 
         ErrorMessage = "El formato del campo Teléfono no es correcto, Ej. (+503) 2222-2222")]
        public string Telefono { get; set; }
    }
}
