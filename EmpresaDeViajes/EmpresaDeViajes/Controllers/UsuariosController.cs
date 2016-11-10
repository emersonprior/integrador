using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmpresaDeViajes.Models;

namespace EmpresaDeViajes.Controllers
{
    public class UsuariosController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();
        
        // GET: Usuarios
        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    return View(db.Usuarios.ToList());
                }

            }
            return RedirectToAction("Login", "Home");            
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Usuario usuario = db.Usuarios.Find(id);
                    if (usuario == null)
                    {
                        return HttpNotFound();
                    }
                    return View(usuario);
                }
            }

            return RedirectToAction("Login", "Home");

        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    return View();
                }
            }
            return RedirectToAction("Login", "Home");
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ci,NombreApellido,Direccion,Telefono,Email,Fec_Nac,Administrador,Password")] Usuario usuario)
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    if (ModelState.IsValid)
                    {
                        usuario.Activo = true;
                        db.Usuarios.Add(usuario);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(usuario);
                }
            }

            return RedirectToAction("Login", "Home");
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ci,NombreApellido,Direccion,Telefono,Email,Fec_Nac,Administrador,Password,Activo")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {          
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            usuario.Activo = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public static IEnumerable<SelectListItem> GetClientes()
        {
            EmpresaDeViajesContext Base = new EmpresaDeViajesContext();

            var Clientes = Base.Usuarios.Where(c => c.Activo == true).Select
                       (c =>
                                new SelectListItem
                                {
                                    Value = c.Id.ToString(),
                                    Text = c.NombreApellido+" Documento:"+c.Ci,
                                });

            return new SelectList(Clientes, "Value", "Text"); ;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
