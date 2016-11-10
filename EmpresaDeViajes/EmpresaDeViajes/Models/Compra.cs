using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaDeViajes.Models
{
    public class Compra
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int CostoTotal { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public virtual Usuario Cliente { get; set; }
        [Required]
        public virtual ICollection<Excursion> CompraExcursion { get; set; }
        [Required]
        public virtual ICollection<Transporte> CompraTransporte { get; set; }
        public Compra() {
            this.CompraExcursion = new List<Excursion>();
            this.CompraTransporte = new List<Transporte>();
        }

    }
}