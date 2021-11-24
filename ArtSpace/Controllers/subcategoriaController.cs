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
    public class subcategoriaController : Controller
    {
        private contextoTienda db = new contextoTienda();

        // GET: subcategoria
        public ActionResult Index()
        {
            var subcategoria = db.subcategoria.Include(s => s.categoria);
            return View(subcategoria.ToList());
        }

        // GET: subcategoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategoria subcategoria = db.subcategoria.Find(id);
            if (subcategoria == null)
            {
                return HttpNotFound();
            }
            return View(subcategoria);
        }

        // GET: subcategoria/Create
        public ActionResult Create()
        {
            ViewBag.id_categoria = new SelectList(db.categoria, "id_categoria", "nombre");
            return View();
        }

        // POST: subcategoria/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_subcategoria,nombre,descripcion,id_categoria")] subcategoria subcategoria)
        {
            if (ModelState.IsValid)
            {
                db.subcategoria.Add(subcategoria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_categoria = new SelectList(db.categoria, "id_categoria", "nombre", subcategoria.id_categoria);
            return View(subcategoria);
        }

        // GET: subcategoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategoria subcategoria = db.subcategoria.Find(id);
            if (subcategoria == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_categoria = new SelectList(db.categoria, "id_categoria", "nombre", subcategoria.id_categoria);
            return View(subcategoria);
        }

        // POST: subcategoria/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_subcategoria,nombre,descripcion,id_categoria")] subcategoria subcategoria)
        {
            if (ModelState.IsValid)
            {
                int id = subcategoria.id_categoria;
                var sub = db.subcategoria.Find(id);
                string nom = sub.nombre;
                sub.nombre = nom;
                sub.descripcion = subcategoria.descripcion;
                sub.id_categoria = subcategoria.id_categoria;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_categoria = new SelectList(db.categoria, "id_categoria", "nombre", subcategoria.id_categoria);
            return View(subcategoria);
        }

        // GET: subcategoria/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subcategoria subcategoria = db.subcategoria.Find(id);
            if (subcategoria == null)
            {
                return HttpNotFound();
            }
            return View(subcategoria);
        }

        // POST: subcategoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subcategoria subcategoria = db.subcategoria.Find(id);
            db.subcategoria.Remove(subcategoria);
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
