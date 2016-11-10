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
    public class ComprasController : Controller
    {
        private EmpresaDeViajesContext db = new EmpresaDeViajesContext();

        // GET: Compras
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    return View(db.Compras.ToList());
                }
            }

            return RedirectToAction("Login", "Home");
        }

        // GET: Compras/Details/5
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
                    Compra compra = db.Compras.Find(id);
                    if (compra == null)
                    {
                        return HttpNotFound();
                    }
                    return View(compra);
                }
            }

                return RedirectToAction("Login", "Home");
          }

        // GET: Compras/Create
        public ActionResult Create()
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    Compra nuevaCompra = new Compra();
                    if (Session["NuevaCompra"] == null) { Session["NuevaCompra"] = nuevaCompra; }
                    if (Session["NuevaCompra"] != null) { nuevaCompra = (Compra)Session["NuevaCompra"]; }
                    if (Session["ExcursionNueva"] != null)
                    {
                        Excursion nuevaExcursion = db.Excursiones.Find((int)Session["ExcursionNueva"]);
                        Boolean agregarExcursion = true;
                        foreach (Excursion e in nuevaCompra.CompraExcursion)
                        {
                            if (e.Id == nuevaExcursion.Id) { agregarExcursion = false; }
                        }
                        if (agregarExcursion == true)
                        {
                            nuevaCompra.CompraExcursion.Add(nuevaExcursion);
                        }
                        Session["ExcursionNueva"] = null;
                        Session["NuevaCompra"] = nuevaCompra;
                    }
                    if (Session["ExcursionQuitar"] != null)
                    {
                        Excursion QuitarExcursion = db.Excursiones.Find((int)Session["ExcursionQuitar"]);
                        foreach (Excursion e in nuevaCompra.CompraExcursion)
                        {
                            if (e.Id == QuitarExcursion.Id)
                            {
                                QuitarExcursion = e;
                            }
                        }
                        nuevaCompra.CompraExcursion.Remove(QuitarExcursion);
                        int i = nuevaCompra.CompraExcursion.Count();
                        Session["ExcursionQuitar"] = null;
                        Session["NuevaCompra"] = nuevaCompra;
                    }
                    if (Session["TransporteNuevo"] != null)
                    {
                        Transporte nuevoTransporte = db.Transportes.Find((int)Session["TransporteNuevo"]);
                        Boolean agregarTransporte = true;
                        foreach (Transporte t in nuevaCompra.CompraTransporte)
                        {
                            if (t.Id == nuevoTransporte.Id)
                            {
                                agregarTransporte = false;
                            }
                        }
                        if (agregarTransporte == true)
                        {
                            nuevaCompra.CompraTransporte.Add(nuevoTransporte);
                        }
                        Session["TransporteNuevo"] = null;
                        Session["NuevaCompra"] = nuevaCompra;
                    }
                    if (Session["TransporteQuitar"] != null)
                    {
                        Transporte QuitarTransporte = db.Transportes.Find((int)Session["TransporteQuitar"]);
                        foreach (Transporte t in nuevaCompra.CompraTransporte)
                        {
                            if (t.Id == QuitarTransporte.Id)
                            {
                                QuitarTransporte = t;
                            }
                        }
                        nuevaCompra.CompraTransporte.Remove(QuitarTransporte);
                        Session["TransporteQuitar"] = null;
                        Session["NuevaCompra"] = nuevaCompra;
                    }
                    var model = new ViewModels.ViewModelCompra()
                    {
                        Compra = nuevaCompra,
                        Excursiones = ExcursionesController.GetExcursiones(),
                        Transportes = TransportesController.GetTransportes(),
                        Clientes = UsuariosController.GetClientes()
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Login", "Home");
        }

        // POST: Compras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewModels.ViewModelCompra nuevaCompra)
        {
            if (System.Web.HttpContext.Current.Session["ingreso"] != null)
            {
                if ((Boolean)Session["ingreso"] == true)
                {
                    Compra aux = (Compra)Session["NuevaCompra"];
                    Compra nueva = new Compra();
                    foreach (Excursion e in aux.CompraExcursion)
                    {
                        int ID = e.Id;
                        Excursion AuxExcursion = db.Excursiones.Find(ID);
                        nueva.CompraExcursion.Add(AuxExcursion);
                    }
                    foreach (Transporte t in aux.CompraTransporte)
                    {
                        int ID = t.Id;
                        Transporte AuxTransporte = db.Transportes.Find(ID);
                        nueva.CompraTransporte.Add(AuxTransporte);
                    }
                    int Excursiones = 0; int Transportes = 0;
                    foreach (Excursion E in aux.CompraExcursion)
                    {
                        Excursiones = (E.Costo) + Excursiones;
                    }
                    foreach (Transporte T in aux.CompraTransporte)
                    {
                        Transportes = T.Costo + Transportes;
                    }
                    nueva.CostoTotal = Excursiones + Transportes;
                    nueva.Fecha = DateTime.Now;
                    nueva.Cliente = db.Usuarios.Find(nuevaCompra.Compra.Cliente.Id);

                    if (nueva.CompraExcursion.Count() ==0 && nueva.CompraTransporte.Count() == 0)
                    {
                        return RedirectToAction("Create");
                    }
                    Session["NuevaCompra"] = null;
                    db.Compras.Add(nueva);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult TomarExcursion(int Id)
        {
            Session["ExcursionNueva"] =Id;
            return RedirectToAction("Create");
        }
        public ActionResult TomarTransporte(int id)
        {
            Session["TransporteNuevo"] = id;
            return RedirectToAction("Create");
        }
        public ActionResult QuitarExcursion(int Id)
        {
            Session["ExcursionQuitar"] = Id;
            return RedirectToAction("Create");
        }
        public ActionResult QuitarTransporte(int Id)
        {
            Session["TransporteQuitar"] = Id;
            return RedirectToAction("Create");
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
