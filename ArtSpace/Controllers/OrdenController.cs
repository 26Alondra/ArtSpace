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
    [Authorize]
    public class OrdenController : Controller
    {
        private EsculturasBDEntities1 db = new EsculturasBDEntities1();

        // GET: Orden
        public ActionResult Index()
        {
            var orden = db.Orden.Where(o => o.fecha_envio == null).OrderBy(o => o.fecha_creacion).Include(o => o.Cliente).Include(o => o.dirEntrega).Include(o => o.Paqueteria);
            return View(orden.ToList());
        }

        // GET: Orden
        public ActionResult Index2()
        {
            var orden = db.Orden.Where(o => o.fecha_entrega == null && o.fecha_envio != null).OrderBy(o => o.fecha_creacion).Include(o => o.Cliente).Include(o => o.dirEntrega).Include(o => o.Paqueteria);
            return View(orden.ToList());
        }

        // GET: Orden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: Orden/Details/5
        public ActionResult Details2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: Orden/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre");
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle");
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre");
            return View();
        }

        // POST: Orden/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,fecha_creacion,num_confirmacion,total,id_cliente,id_dirEntrega,id_paqueteria,num_guia,feha_envio,fecha_entrega,estatus")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Orden.Add(orden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // GET: Orden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fecha_creacion,num_confirmacion,total,id_cliente,id_dirEntrega,id_paqueteria,num_guia,feha_envio,fecha_entrega,estatus")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // GET: Orden/Edit/5
        public ActionResult Pendiente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pendiente([Bind(Include = "Id,id_paqueteria,num_guia,fecha_envio")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                Orden o = db.Orden.Find(orden.Id);
                o.id_paqueteria = orden.id_paqueteria;
                o.num_guia = orden.num_guia;
                o.fecha_envio = orden.fecha_envio;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }
        // GET: Orden/Edit/5
        public ActionResult Enviados(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public ActionResult Enviados([Bind(Include = "Id,fecha_entrega")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                Orden o = db.Orden.Find(orden.Id);
                o.fecha_entrega = orden.fecha_entrega;
                db.SaveChanges();
                return RedirectToAction("Index2");
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "id", "nombre", orden.id_cliente);
            ViewBag.id_dirEntrega = new SelectList(db.dirEntrega, "id", "calle", orden.id_dirEntrega);
            ViewBag.id_paqueteria = new SelectList(db.Paqueteria, "Id", "Nombre", orden.id_paqueteria);
            return View(orden);
        }

        // GET: Orden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Orden.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: Orden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orden orden = db.Orden.Find(id);
            db.Orden.Remove(orden);
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
