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
    public class ViewModelExcursion
    {
        public Excursion Excursion { get; set; }
        public IEnumerable<SelectListItem> Clientes { get; set; }
        public List<Estadia> Estadias { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<Transporte> Transportes { get; set; }
    }
}