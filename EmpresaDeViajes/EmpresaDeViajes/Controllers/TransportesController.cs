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
    public class TransportesController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();

        // GET: Transportes
        public ActionResult Index()
        {

            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    List<Transporte> aux = db.Transportes.Where(m => m.Activo == true).ToList();
                    foreach (Transporte t in aux)
                    {
                        t.CiudadOrigen = (db.Destinos.Find(t.CiudadOrigen.Codigo));
                        t.CiudadDestino = (db.Destinos.Find(t.CiudadDestino.Codigo));
                    }
                    return View(aux);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        // GET: Transportes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transporte transporte = db.Transportes.Find(id);
            if (transporte == null)
            {
                return HttpNotFound();
            }
            return View(transporte);
        }

        // GET: Transportes/Create
        public ActionResult Create()
        {
            List<SelectListItem>tipos = new List<SelectListItem>();
            tipos.Add(new SelectListItem() { Text = "Aire", Value = "Aire" });
            tipos.Add(new SelectListItem() { Text = "Costa", Value = "Costa" });
            tipos.Add(new SelectListItem() { Text = "Tierra", Value = "Tierra" });
            var model = new ViewModels.ViewModelTransporte()
            {
                Transporte = new Transporte(),
                Destinos = DestinosController.GetDestinos(),
                Tipos = tipos
            };
            return View(model);
        }
        // POST: Transportes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModels.ViewModelTransporte NuevoTransporte)
        {
            NuevoTransporte.Transporte.Activo = true;
            NuevoTransporte.Transporte.CiudadOrigen = db.Destinos.Find(NuevoTransporte.Transporte.CiudadOrigen.Codigo);
            NuevoTransporte.Transporte.CiudadDestino = db.Destinos.Find(NuevoTransporte.Transporte.CiudadDestino.Codigo);
            string Medio = NuevoTransporte.Transporte.Tipo;
            if (NuevoTransporte.Transporte.CiudadOrigen.Aire == true && NuevoTransporte.Transporte.CiudadDestino.Aire == true)
                if (Medio == "Aire")
                {
                    db.Transportes.Add(NuevoTransporte.Transporte);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            if (NuevoTransporte.Transporte.CiudadOrigen.Costa == true && NuevoTransporte.Transporte.CiudadDestino.Costa == true)
                if (Medio == "Costa")
                {
                    db.Transportes.Add(NuevoTransporte.Transporte);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            if (NuevoTransporte.Transporte.CiudadOrigen.Tierra == true && NuevoTransporte.Transporte.CiudadDestino.Tierra == true)
                if (Medio == "Tierra")
                {
                    db.Transportes.Add(NuevoTransporte.Transporte);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            return RedirectToAction("Index");
        }

        // GET: Transportes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transporte transporte = db.Transportes.Find(id);
            if (transporte == null)
            {
                return HttpNotFound();
            }
            return View(transporte);
        }

        // POST: Transportes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Activo,Costo,Tipo")] Transporte transporte)
        {

            Transporte original = db.Transportes.Find(transporte.Id);
            original.Activo = false;
            original.CiudadDestino = db.Destinos.Find(original.CiudadDestino.Codigo);
            original.CiudadOrigen = db.Destinos.Find(original.CiudadOrigen.Codigo);// evitar error de entity como q es otro
            db.Entry(original).State = EntityState.Modified;
            db.SaveChanges();
            transporte.CiudadOrigen = original.CiudadOrigen;
            transporte.CiudadDestino = original.CiudadDestino;
            transporte.Activo = true;
            db.Transportes.Add(transporte);
            db.SaveChanges();
            ReconstruirExcursionesActivasCambioCosto(original, transporte);
            return RedirectToAction("Index");
        }


        // GET: Transportes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transporte transporte = db.Transportes.Find(id);
            if (transporte == null)
            {
                return HttpNotFound();
            }
            return View(transporte);
        }

        // POST: Transportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transporte aux = db.Transportes.Find(id);
            aux.Activo = false;
            aux.CiudadOrigen = db.Destinos.Find(aux.CiudadOrigen.Codigo);
            aux.CiudadDestino = db.Destinos.Find(aux.CiudadDestino.Codigo);
            db.SaveChanges();
            DesactivarExcursionesConTransporte(id);
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
        public static List<Transporte> GetTransportes()
        {
            EmpresaDeViajesContext Base = new EmpresaDeViajesContext();
            List<Transporte> A = Base.Transportes.Where(m => m.Activo == true && m.CiudadOrigen.Activo==true && m.CiudadDestino.Activo==true).ToList();
            return A;
        }

        public void DesactivarExcursionesConTransporte(int TransporteId)
        {
            Excursion aux = new Excursion();
            List<Excursion> ExcursionesActivas = db.Excursiones.Where(m => m.Activo == true).ToList();
            List<Excursion> ExcursionesDesactivar = new List<Excursion>();
            foreach (Excursion ex in ExcursionesActivas)
            {
                foreach (Transporte tr in ex.ExcursionesTransportes)
                {
                    if (tr.Id== TransporteId)
                    {
                        ExcursionesDesactivar.Add(ex);
                    }
                }
            }
            foreach (Excursion ED in ExcursionesDesactivar)
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
        public void ReconstruirExcursionesActivasCambioCosto(Transporte Original, Transporte Nuevo)
        {
            List<Excursion> ExcursionesActivas = db.Excursiones.Where(m => m.Activo == true).ToList();
            List<Excursion> ExcursionesModificar = new List<Excursion>();
            int estadia = 0; int transporte = 0;
            Excursion encontrada = new Excursion();
            Excursion aux = new Excursion();
            Excursion auxNueva = new Excursion();
            foreach (Excursion e in ExcursionesActivas)
            {
                encontrada = e;
                encontrada = db.Excursiones.Find(e.Id);
                encontrada.Cliente = db.Usuarios.Find(e.Cliente.Id);
                foreach (Transporte t in e.ExcursionesTransportes)
                {
                    if (t.Id == Original.Id)
                    {
                        ExcursionesModificar.Add(e);
                    }
                }
            }
            foreach (Excursion m in ExcursionesModificar)
            {
                aux = db.Excursiones.Find(m.Id);
                aux.Activo = false;
                aux.Cliente = db.Usuarios.Find(m.Cliente.Id);
                db.Entry(aux).State = EntityState.Modified;
                db.SaveChanges();
                auxNueva.Activo = true;
                auxNueva.Nombre = aux.Nombre;
                auxNueva.Cliente = db.Usuarios.Find(aux.Cliente.Id);
                auxNueva.Descripcion = aux.Descripcion;
                auxNueva.Duración = aux.Duración;
                auxNueva.ExcursionEstadias = aux.ExcursionEstadias;
                auxNueva.ExcursionesTransportes = aux.ExcursionesTransportes;
                auxNueva.ExcursionesTransportes.Remove(Original);
                auxNueva.ExcursionesTransportes.Add(Nuevo);
                foreach (Transporte t in auxNueva.ExcursionesTransportes)
                {
                    transporte = t.Costo + transporte;
                }
                foreach (Estadia E in auxNueva.ExcursionEstadias)
                {
                    estadia = ((E.Destino.Costo) * (E.Dias)) + estadia;
                }
                auxNueva.Costo = estadia + transporte;
                db.Excursiones.Add(auxNueva);
                db.SaveChanges();
                estadia = 0; transporte = 0;
            }
        }
    }
}
