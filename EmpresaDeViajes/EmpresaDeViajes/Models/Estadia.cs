using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EmpresaDeViajes.Models
{
    public class Estadia
    {   
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual Destino Destino { get; set; }
        [Required]
        public int Dias { get; set; }
        [Required]
        public Boolean Activo { get; set; }
    }
}