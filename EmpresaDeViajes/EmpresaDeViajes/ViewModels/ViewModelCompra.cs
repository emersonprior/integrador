using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EmpresaDeViajes.Models;
namespace EmpresaDeViajes.ViewModels
{
    public class ViewModelCompra
    {
        public Compra Compra { get; set; }
        
        public int CostoTotal { get; set; }
        public DateTime Fecha { get; set; }
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public List<Excursion> Excursiones { get; set; }
        public List<Transporte> Transportes { get; set; }
    }
}