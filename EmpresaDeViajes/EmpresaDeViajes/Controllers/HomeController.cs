using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmpresaDeViajes.Models;

namespace EmpresaDeViajes.Controllers
{
    public class HomeController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {

            Session["ingreso"] = false;
            Session["Funcionario"] = null;
            return RedirectToAction("Login","Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,Password")] Usuario funcionario)
        {
            if (Valido(funcionario.Id, funcionario.Password) != null)
            {

                if (Rol(funcionario.Id) == true)
                {
                    Session["ingreso"] = true;
                    return RedirectToAction("Index", "Usuarios");
                }
            }
         
                return RedirectToAction("Login", "Home");
            
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public Usuario Valido(int Numero, string contra)
        {
            Usuario B = null;

            Usuario funcionarioLogin = db.Usuarios.Find(Numero);

            if (funcionarioLogin != null)
            {
                if (funcionarioLogin.Password == contra)
                {
                    B = new Usuario();
                    B = funcionarioLogin;
                    Session["ingreso"] = true;
                    Session["Funcionario"] = B;
                }
            }
            return B;
        }
        public Boolean Rol(int NroFuncionario)
        {
            Usuario funcionarioRol = db.Usuarios.Find(NroFuncionario);
            return funcionarioRol.Administrador;
        }

    }
}