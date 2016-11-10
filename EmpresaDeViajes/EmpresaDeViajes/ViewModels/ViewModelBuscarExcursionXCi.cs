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
    public class ViewModelBuscarExcursionXCi
    {
        public int Ci { get; set; }
        public List<Excursion> Excursiones { get; set; }
    }
}