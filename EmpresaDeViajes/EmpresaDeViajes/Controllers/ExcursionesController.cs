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
    public class ExcursionesController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();

        // GET: Excursiones
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    return View(db.Excursiones.Where(m => m.Activo == true && m.Cliente.Administrador == true).ToList());
                }
            }

            return RedirectToAction("Login", "Home");
        }

        public ActionResult BuscarClienteExcursion()
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    var model = new ViewModels.ViewModelBuscarExcursionXCi()
                    {
                        Excursiones = new List<Excursion>(),
                        Ci = 0
                    };
                    return View(model);
                }
            }

            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuscarClienteExcursion(ViewModels.ViewModelBuscarExcursionXCi NuevaBusqueada) {

            List<Excursion> ExcursionesPorCI = db.Excursiones.Where(m => m.Cliente.Ci == NuevaBusqueada.Ci).ToList();
            if (ExcursionesPorCI.Count() == 0) { return RedirectToAction("BuscarClienteExcursion", "Excursiones"); }
            NuevaBusqueada.Excursiones = ExcursionesPorCI;
            return View(NuevaBusqueada);

        }
          
        // GET: Excursiones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursion excursion = db.Excursiones.Find(id);
            if (excursion == null)
            {
                return HttpNotFound();
            }
            return View(excursion);
        }

        // GET: Excursiones/Create
        public ActionResult Create()
        {
            Excursion nuevaExcursion = new Excursion();
            if (Session["NuevaExcursion"] == null) { Session["NuevaExcursion"] = nuevaExcursion; }
            if (Session["NuevaExcursion"] != null) {nuevaExcursion = (Excursion)Session["NuevaExcursion"];}
            if (Session["EstadiaNueva"] != null) {
                Estadia nuevaEstadia =  db.Estadias.Find((int)Session["EstadiaNueva"]);
                Boolean agregarEstadia = true;
                foreach (Estadia e in nuevaExcursion.ExcursionEstadias) {
                    if (e.Id == nuevaEstadia.Id) { agregarEstadia = false; }
                }
                if(agregarEstadia == true) { 
                nuevaExcursion.ExcursionEstadias.Add(nuevaEstadia);
                }
                Session["EstadiaNueva"] = null;
                Session["NuevaExcursion"] = nuevaExcursion;
            }
            if (Session["EstadiaQuitar"] != null)
            {
                Estadia QuitarEstadia = db.Estadias.Find((int)Session["EstadiaQuitar"]);
                foreach (Estadia e in nuevaExcursion.ExcursionEstadias) {
                    if (e.Id == QuitarEstadia.Id) {
                       QuitarEstadia =e;
                    }
                }
                nuevaExcursion.ExcursionEstadias.Remove(QuitarEstadia);
                int i = nuevaExcursion.ExcursionEstadias.Count();
                Session["EstadiaQuitar"] = null;
                Session["NuevaExcursion"] = nuevaExcursion;
            }
            if (Session["TransporteNuevo"] != null)
            {
                Transporte nuevoTransporte = db.Transportes.Find((int)Session["TransporteNuevo"]);
                Boolean agregarTransporte = true;
                foreach (Transporte t in nuevaExcursion.ExcursionesTransportes) {
                    if (t.Id == nuevoTransporte.Id) {
                        agregarTransporte = false;
                    }
                }
                if(agregarTransporte == true) { 
                nuevaExcursion.ExcursionesTransportes.Add(nuevoTransporte);
                }
                Session["TransporteNuevo"] = null;
                Session["NuevaExcursion"] = nuevaExcursion;
            }
            if (Session["TransporteQuitar"] != null)
            {
                Transporte QuitarTransporte = db.Transportes.Find((int)Session["TransporteQuitar"]);
                foreach (Transporte t in nuevaExcursion.ExcursionesTransportes)
                {
                    if (t.Id == QuitarTransporte.Id)
                    {
                        QuitarTransporte = t;
                    }
                }
                nuevaExcursion.ExcursionesTransportes.Remove(QuitarTransporte);
                Session["TransporteQuitar"] = null;
                Session["NuevaExcursion"] = nuevaExcursion;
            }
            var model = new ViewModels.ViewModelExcursion()
            {
                Excursion = nuevaExcursion,
                Estadias = EstadiasController.GetEstadias(),
                Transportes = TransportesController.GetTransportes(),
                Clientes = UsuariosController.GetClientes()
            };
            return View(model);
        }

        // POST: Excursiones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModels.ViewModelExcursion nuevaExcursion)
        {
            Excursion aux = (Excursion)Session["NuevaExcursion"];
            Excursion nueva = new Excursion();
            foreach (Estadia e in aux.ExcursionEstadias) {
                int ID = e.Id;
                Estadia AuxEstadia = db.Estadias.Find(ID);
                nueva.ExcursionEstadias.Add(AuxEstadia);
            }
            foreach (Transporte t in aux.ExcursionesTransportes)
            {
                int ID = t.Id;
                Transporte AuxTransporte = db.Transportes.Find(ID);
                nueva.ExcursionesTransportes.Add(AuxTransporte);
            }

            int estadias=0;int transportes=0; int duracion = 0;
            foreach (Estadia E in aux.ExcursionEstadias)
            {
                duracion = (E.Dias) + duracion;
            }
            foreach (Estadia E in aux.ExcursionEstadias) {
                estadias = ( (E.Destino.Costo) * (E.Dias))+estadias;
            }
            foreach (Transporte T in aux.ExcursionesTransportes)
            {
                transportes = T.Costo + transportes;
            }
            nueva.Costo = estadias + transportes;
            nueva.Activo = true;
            nueva.Duración = duracion;
            nueva.Descripcion = nuevaExcursion.Descripcion;
            nueva.Nombre = nuevaExcursion.Nombre;
            nueva.Cliente = db.Usuarios.Find(nuevaExcursion.Excursion.Cliente.Id);
            if (nueva.ExcursionEstadias.Count() == 0 && nueva.ExcursionesTransportes.Count() == 0)
            {
                return RedirectToAction("Create");
            }
            if (nueva.Cliente==null || nueva.Nombre == null || nueva.Descripcion == null)
            {
                return RedirectToAction("Create");
            }
            Session["NuevaExcursion"] = null;
            db.Excursiones.Add(nueva);
                db.SaveChanges();
                return RedirectToAction("Index");
            
        }

        // GET: Excursiones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursion excursion = db.Excursiones.Find(id);
            if (excursion == null)
            {
                return HttpNotFound();
            }
            Session["IdClienteExcursion"]= excursion.Cliente.Id;
            return View(excursion);
        }

        // POST: Excursiones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Descripcion,Duración,Costo,Activo")] Excursion excursion)
        {
            Excursion original = db.Excursiones.Find(excursion.Id);
            original.Activo = false;
            original.Cliente = db.Usuarios.Find((int)Session["IdClienteExcursion"]);
            Session["IdClienteExcursion"] = null;
            db.Entry(original).State = EntityState.Modified;
            db.SaveChanges();
            excursion.Activo = true;
            excursion.Cliente = original.Cliente;
            excursion.ExcursionEstadias = original.ExcursionEstadias;
            excursion.ExcursionesTransportes = original.ExcursionesTransportes;
            excursion.Duración = original.Duración;
            excursion.Costo = original.Costo;
            db.Excursiones.Add(excursion);
            db.SaveChanges();
            return RedirectToAction("Index");
           
        }

        // GET: Excursiones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Excursion excursion = db.Excursiones.Find(id);
            if (excursion == null)
            {
                return HttpNotFound();
            }
            return View(excursion);
        }

        // POST: Excursiones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        { 
            Excursion aux = db.Excursiones.Find(id);
            aux.Activo = false;
            aux.Cliente = db.Usuarios.Find(aux.Cliente.Id);
            db.Entry(aux).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult TomarEstadia(int id) {
            Session["EstadiaNueva"] = id;
            return RedirectToAction("Create");
        }
        public ActionResult TomarTransporte(int id)
        {
            Session["TransporteNuevo"] = id;
            return RedirectToAction("Create");
        }
        public ActionResult QuitarEstadia(int id) {
            Session["EstadiaQuitar"] = id;
            return RedirectToAction("Create");
        }
        public ActionResult QuitarTransporte(int Id)
       { 
            Session["TransporteQuitar"] = Id;
            return RedirectToAction("Create");
        }
        public static List<Excursion> GetExcursiones()
        {
            EmpresaDeViajesContext Base = new EmpresaDeViajesContext();
            List<Excursion> A = Base.Excursiones.Where(m => m.Activo == true).ToList();
            return A;
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
