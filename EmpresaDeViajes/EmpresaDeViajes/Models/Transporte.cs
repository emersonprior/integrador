using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaDeViajes.Models
{
    public class Transporte
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public Boolean Activo { get; set; }
        [Required]
        public virtual Destino CiudadOrigen { get; set; }
        [Required]
        public virtual Destino CiudadDestino { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Costo { get; set; }
    }
}