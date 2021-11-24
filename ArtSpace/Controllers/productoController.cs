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
    public class productoController : Controller
    {
        private contextoTienda db = new contextoTienda();

        // GET: producto
        public ActionResult Index()
        {
            var producto = db.producto.Include(p => p.subcategoria);
            return View(producto.ToList());
        }

        // GET: producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: producto/Create
        public ActionResult Create()
        {
            ViewBag.id_subcategoria = new SelectList(db.subcategoria, "id_subcategoria", "nombre");
            return View();
        }

        // POST: producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_producto,nombre,descripcion,precio,imagen,existencia,stock,id_subcategoria")] producto producto)
        {
            if (ModelState.IsValid)
            {
                db.producto.Add(producto);
                db.SaveChanges();
                int id = producto.id_producto;
                var prod = db.producto.Find(id);
                DateTime hoy = DateTime.Now;
                prod.ult_act = hoy;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_subcategoria = new SelectList(db.subcategoria, "id_subcategoria", "nombre", producto.id_subcategoria);
            return View(producto);
        }

        // GET: producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_subcategoria = new SelectList(db.subcategoria, "id_subcategoria", "nombre", producto.id_subcategoria);
            return View(producto);
        }

        // POST: producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_producto,nombre,descripcion,precio,ult_act,imagen,existencia,stock,id_subcategoria")] producto producto)
        {
            if (ModelState.IsValid)
            {
                int id = producto.id_producto;
                var prod = db.producto.Find(id);
                decimal? precio_ant = prod.precio;
                decimal? precio_act = producto.precio;
                string nom = prod.nombre;
                prod.nombre = nom;
                prod.descripcion = producto.descripcion;
                prod.precio = producto.precio;
                prod.imagen = producto.imagen;
                prod.existencia = producto.existencia;
                prod.stock = producto.stock;
                prod.id_subcategoria = producto.id_subcategoria;

                //db.Entry(producto).State = EntityState.Modified;
                if(precio_act != precio_ant)
                {
                    DateTime hoy = DateTime.Now;
                    prod.ult_act = hoy;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_subcategoria = new SelectList(db.subcategoria, "id_subcategoria", "nombre", producto.id_subcategoria);
            return View(producto);
        }

        // GET: producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            producto producto = db.producto.Find(id);
            db.producto.Remove(producto);
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
