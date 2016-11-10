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
    public class ViewModelEstadia
    {
        public Estadia Estadia { get; set; }
        public IEnumerable<SelectListItem> Destinos { get; set; }
        
    }
}