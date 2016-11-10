using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaDeViajes.Models
{
    public class Destino
    {
           
        [Required]
        [Key]
        public int Codigo { get; set; }
        [Required]
        public Boolean Activo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Pais { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int Costo { get; set; }
        [Required]
        public Boolean Costa { get; set; }
        [Required]
        public Boolean Tierra { get; set; }
        [Required]
        public Boolean Aire { get; set; }

    }
}