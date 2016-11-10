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
    public class DestinosController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();

        // GET: Destinos
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    return View(db.Destinos.Where(m => m.Activo == true).ToList());
                }
            }
            return RedirectToAction("Login", "Home");
        }

        // GET: Destinos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destino destino = db.Destinos.Find(id);
            if (destino == null)
            {
                return HttpNotFound();
            }
            return View(destino);
        }

        // GET: Destinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Destinos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Activo,Nombre,Pais,Descripcion,Costo,Costa,Tierra,Aire")] Destino destino)
        {
            if (ModelState.IsValid)
            {
                destino.Activo = true;
                db.Destinos.Add(destino);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(destino);
        }

        // GET: Destinos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destino destino = db.Destinos.Find(id);
            if (destino == null)
            {
                return HttpNotFound();
            }
            return View(destino);
        }

        // POST: Destinos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Activo,Nombre,Pais,Descripcion,Costo,Costa,Tierra,Aire")] Destino destino)
        {
            Destino aux = destino;
            aux.Activo = true;
            destino = db.Destinos.Find(destino.Codigo);
            destino.Activo = false;
            db.Entry(destino).State = EntityState.Modified;
            db.SaveChanges();
            db.Destinos.Add(aux);
            db.SaveChanges();
            // evito tener transportes, estadias, excursiones disponibles con destinos viejos (no disponibles)
            DesactivarTransportesAsociados(destino.Codigo);
            DesactivarEstadiasAsociadas(destino.Codigo);
            DesactivarExcursionesAsociados(destino.Codigo);
            return RedirectToAction("Index");
        }

        // GET: Destinos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destino destino = db.Destinos.Find(id);
            if (destino == null)
            {
                return HttpNotFound();
            }
            return View(destino);
        }

        // POST: Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Destino destino = db.Destinos.Find(id);
            destino.Activo = false;
            db.SaveChanges();
            DesactivarEstadiasAsociadas(id);
            DesactivarTransportesAsociados(id);
            DesactivarExcursionesAsociados(id);
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

        // para obtenerlos destino y asi agregarselos a un transporte 
        public static IEnumerable<SelectListItem> GetDestinos()
        {
            EmpresaDeViajesContext Base = new EmpresaDeViajesContext();

            var Destinos = Base.Destinos.Where(m => m.Activo == true).Select
                       (a =>
                                new SelectListItem
                                {
                                    Value = a.Codigo.ToString(),
                                    Text = a.Nombre,
                                });

            return new SelectList(Destinos, "Value", "Text"); ;

        }
        public void DesactivarEstadiasAsociadas(int CodigoDestino)
        {
            Estadia aux = new Estadia();
            List<Estadia> Estadias = db.Estadias.Where(m => m.Destino.Codigo == CodigoDestino&&m.Activo==true).ToList();
            foreach (Estadia e in Estadias)
            {
                aux = db.Estadias.Find(e.Id);
                aux.Activo = false;
                db.Entry(aux).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void DesactivarTransportesAsociados(int CodigoDestino)
        {
            Transporte aux = new Transporte();
            List<Transporte> Transportes = db.Transportes.Where(m => m.CiudadOrigen.Codigo == CodigoDestino || m.CiudadDestino.Codigo == CodigoDestino).ToList();
            foreach (Transporte t in Transportes)
            {
                aux = db.Transportes.Find(t.Id);
                aux.CiudadOrigen = db.Destinos.Find(t.CiudadOrigen.Codigo);
                aux.CiudadDestino = db.Destinos.Find(t.CiudadDestino.Codigo);
                if(aux.Activo == true) {
                    aux.Activo = false;
                    db.Entry(aux).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public void DesactivarExcursionesAsociados(int CodigoDestino){
            Excursion aux = new Excursion();
            List<Excursion> ExcursionesActivas = db.Excursiones.Where(m => m.Activo == true).ToList();
            List<Excursion> ExcursionesDesactivadar = new List<Excursion>();
            foreach (Excursion ex in ExcursionesActivas) {
                foreach (Estadia es in ex.ExcursionEstadias) {
                    if (es.Destino.Codigo == CodigoDestino)
                    {
                        ExcursionesDesactivadar.Add(ex);
                    }

                }
                foreach (Transporte tr in ex.ExcursionesTransportes) {
                    if (tr.CiudadOrigen.Codigo == CodigoDestino || tr.CiudadDestino.Codigo == CodigoDestino)
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
        public void ActualizarEstadiasActivas(Destino destinoViejo, Destino destinoNuevo) {
            Estadia nueva = new Estadia();
            List<Estadia> activas = db.Estadias.Where(m => m.Activo == true && m.Destino == destinoViejo).ToList();
            List<Estadia> nuevas = new List<Estadia>();
            foreach (Estadia e in activas) {
                nueva.Dias = e.Dias;
                nueva.Activo = true;
                nueva.Destino = destinoNuevo;
                e.Activo = false;
                db.Estadias.Add(nueva);
                db.SaveChanges();
                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
