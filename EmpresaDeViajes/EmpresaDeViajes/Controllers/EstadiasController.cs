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
    public class EstadiasController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();

        // GET: Estadias
        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    List<Estadia> aux = db.Estadias.Where(m => m.Activo == true).ToList();
                    return View(aux);
                }
            }

            return RedirectToAction("Login", "Home");
        
        }

        // GET: Estadias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = db.Estadias.Find(id);
            if (estadia == null)
            {
                return HttpNotFound();
            }
            return View(estadia);
        }

        // GET: Estadias/Create
        public ActionResult Create()
        {
             var model = new ViewModels.ViewModelEstadia()
            {
                Estadia = new Estadia(),
                Destinos = DestinosController.GetDestinos()

            };
            return View(model);
        }

        // POST: Estadias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModels.ViewModelEstadia NuevaEstadia)
        {
            NuevaEstadia.Estadia.Destino = db.Destinos.Find(NuevaEstadia.Estadia.Destino.Codigo);
            NuevaEstadia.Estadia.Activo = true;
            db.Estadias.Add(NuevaEstadia.Estadia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Estadias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = db.Estadias.Find(id);
            if (estadia == null)
            {
                return HttpNotFound();
            }
            return View(estadia);
        }

        // POST: Estadias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estadia estadia = db.Estadias.Find(id);
            estadia.Activo = false;
            estadia.Destino = db.Destinos.Find(estadia.Destino.Codigo);
            db.Entry(estadia).State = EntityState.Modified;
            db.SaveChanges();
            DesactivarExcursionesConEstadia(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public static List<Estadia> GetEstadias()
        {
            EmpresaDeViajesContext Base = new EmpresaDeViajesContext();
            List<Estadia> A = Base.Estadias.Where(m => m.Destino.Activo == true && m.Activo == true).ToList();
            return A;
        }
        public void DesactivarExcursionesConEstadia(int IdEstadia)
        {
            Excursion aux = new Excursion();
            List<Excursion> ExcursionesActivas = db.Excursiones.Where(m => m.Activo == true).ToList();
            List<Excursion> ExcursionesDesactivadar = new List<Excursion>();
            foreach (Excursion ex in ExcursionesActivas)
            {
                foreach (Estadia es in ex.ExcursionEstadias)
                {
                    if (es.Id == IdEstadia)
                    {
                        ExcursionesDesactivadar.Add(ex);
                    }

                }
            }
            foreach (Excursion ED in ExcursionesDesactivadar)
            {
                aux = db.Excursiones.Find(ED.Id);
                if (aux.Activo == true)
                {
                    aux.Activo = false;
                    aux.Cliente = db.Usuarios.Find(aux.Cliente.Id);
                    db.Entry(aux).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}
