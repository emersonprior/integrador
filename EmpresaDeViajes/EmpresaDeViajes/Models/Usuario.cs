using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaDeViajes.Models
{
    public class Usuario
    {

        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public int Ci { get; set; }
        [Required]
        public string NombreApellido { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Fec_Nac { get; set; }
        [Required]
        public Boolean Administrador { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Boolean Activo { get; set; }


    }
}