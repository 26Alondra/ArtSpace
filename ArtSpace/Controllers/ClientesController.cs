using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArtSpace.Models;

namespace ArtSpace.Controllers
{
    public class ClientesController : Controller
    {
        private contextArtSpace db = new contextArtSpace();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string nombre, string email, string calle, string colonia, string estado, string codigo_postal, string municipio, string numTarjeta, string Mes, string Anio, string CVV)
        {
            cliente cliente = new cliente();
            int id = 0;
            if(!(db.cliente.Max(c => (int?)c.Id_cliente) == null))
            {
                id = db.cliente.Max(c => c.Id_cliente);
            }
            else
            {
                id = 0;
            }
            id++;
            if (Tarjeta(numTarjeta, Mes, Anio, CVV))
            {
                if (validaPago(nombre, calle, colonia, estado, numTarjeta, Mes, Anio, CVV))
                {
                    cliente.Id_cliente = id;
                    cliente.nombre = nombre;
                    cliente.email = Session["correo"].ToString();
                    cliente.calle = calle;
                    cliente.municipio = municipio;
                    cliente.colonia = colonia;
                    cliente.estado = estado;
                    cliente.codigo_postal = codigo_postal;
                    dir_entrega dir = new dir_entrega();
                    dir.estado = estado;
                    dir.municipio = municipio;
                    dir.colonia = colonia;
                    dir.calle = calle;
                    dir.codigo_postal = codigo_postal;
                    dir.id_cliente = id;
                    metPago met = new metPago();
                    met.numero_tarjeta = numTarjeta;
                    met.fecha_vencimiento = Mes +"/"+ Anio;
                    met.id_cliente = id;
                    db.cliente.Add(cliente);
                    db.dir_entrega.Add(dir);
                    db.metPago.Add(met);
                    db.SaveChanges();
                    string[] nombres = cliente.nombre.ToString().Split(' ');
                    Session["name"] = nombres[0];
                    Session["usr"] = cliente.nombre;
                    if (Session["CrearOrden"] != null)
                    {
                        if (Session["CrearOrden"].Equals("pend")){
                            return RedirectToAction("CrearOrden", "Pago");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Invalida");
                }
            }
            else
            {
                return RedirectToAction("Invalida");
            }
            return View();
        }

        private bool Tarjeta(string numTarjeta, string Mes, string Anio, string CVV)
        {
            bool retorna = true;
            return retorna;
        }

        private bool validaPago(string nombre, string calle, string colonia, string estado, string numTarjeta, string Mes, string Anio, string CVV)
        {
            bool retorna = true;
            return retorna;
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_cliente,nombre,email,calle,colonia,estado,codigo_postal,municipio")] cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cliente cliente = db.cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cliente cliente = db.cliente.Find(id);
            db.cliente.Remove(cliente);
            db.SaveChanges();
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
    }
}
