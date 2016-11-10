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
    public class ViewModelTransporte
    {
        public Transporte Transporte { get; set; }
        public IEnumerable<SelectListItem> Destinos { get; set; }
        public IEnumerable<SelectListItem> Tipos { get; set; }
    }
}