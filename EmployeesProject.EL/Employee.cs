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
        [Required]
        public string Nombres { get; set;}
        [Required]
        public string Apellidos { get; set; }
        [Display(Name ="Fecha de nacimiento")]
        [Required]
        public DateTime FechaNacimiento { get; set; }
        [Required]
        public string DUI { get; set; }
        [Required]
        public string NIT { get; set; }
        [Required]
        public int ISSS { get; set; }
        [Display(Name ="Teléfono")]
        [Required]
        public string Telefono { get; set; }
    }
}
