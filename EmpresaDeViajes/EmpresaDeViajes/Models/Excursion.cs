using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmpresaDeViajes.Models
{
    public class Excursion
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int Duración { get; set; }
        [Required]
        public int Costo { get; set; }
        [Required]
        public virtual Usuario Cliente { get; set; }
        [Required]
        public Boolean Activo { get; set; }
        [Required]
        public virtual ICollection<Estadia> ExcursionEstadias { get; set; }
        [Required]
        public virtual ICollection<Transporte> ExcursionesTransportes { get; set; }
        public Excursion()
        {
            this.ExcursionEstadias = new List<Estadia>();
            this.ExcursionesTransportes = new List<Transporte>();
        }
    }
}