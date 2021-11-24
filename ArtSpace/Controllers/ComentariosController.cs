using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tienda1.Models;

namespace Tienda1.Controllers
{
    public class ComentariosController : Controller
    {
        private contextTienda db = new contextTienda();

        // GET: Comentarios
        public ActionResult Index()
        {
            return View(db.Comentarios.ToList());
        }

        // GET: Comentarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentarios comentarios = db.Comentarios.Find(id);
            if (comentarios == null)
            {
                return HttpNotFound();
            }
            return View(comentarios);
        }

        // GET: Comentarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comentarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_comentario,Id_cliente,id_producto,comentario")] Comentarios comentarios)
        {
            if (ModelState.IsValid)
            {
                db.Comentarios.Add(comentarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comentarios);
        }

        // GET: Comentarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentarios comentarios = db.Comentarios.Find(id);
            if (comentarios == null)
            {
                return HttpNotFound();
            }
            return View(comentarios);
        }

        // POST: Comentarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_comentario,Id_cliente,id_producto,comentario")] Comentarios comentarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comentarios);
        }

        // GET: Comentarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentarios comentarios = db.Comentarios.Find(id);
            if (comentarios == null)
            {
                return HttpNotFound();
            }
            return View(comentarios);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comentarios comentarios = db.Comentarios.Find(id);
            db.Comentarios.Remove(comentarios);
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
        public ActionResult Agregar(int idP)
        {
            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                          where c.email == correo
                          select c).ToList().FirstOrDefault();
            int id = cl.Id_cliente;
            producto pr = (from p in db.producto
                           where p.Id_producto == idP
                           select p).ToList().FirstOrDefault();
            string nombreP = pr.nombre;
            Session["mIdP"] = idP;
            Session["idC"] = id;
            Session["nombre"] = nombreP;
            return View();
        }
        public ActionResult Mostrar()
        {
            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                          where c.email == correo
                          select c).ToList().FirstOrDefault();
            int idC = cl.Id_cliente;
            var query = (from cm in db.Comentarios
                         where cm.Id_cliente == idC
                         select cm);
            List<Comentarios> com = query.ToList();
            Session["misComentario"] = com;
            return View();
        }

        public ActionResult Actualizar(int idCo)
        {
            Session["idCom"] = idCo;

            Comentarios cmm = (from d in db.Comentarios
                               where d.Id_comentario == idCo
                               select d).ToList().FirstOrDefault();
            int idP = cmm.id_producto;
            producto pr = (from b in db.producto
                           where b.Id_producto == idP
                           select b).ToList().FirstOrDefault();
            Session["nomPr"] = pr.nombre;
            return View();
        }

        public ActionResult AActualizar(int idP, string coment)
        {
            Comentarios comen = new Comentarios();

            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                          where c.email == correo
                          select c).ToList().FirstOrDefault();
            int idC = cl.Id_cliente;

            comen.comentario = coment;
            comen.Id_cliente = idC;
            comen.id_producto = idP;

            db.Comentarios.Add(comen);
            db.SaveChanges();

            return RedirectToAction("Mostrar");
        }

        public ActionResult Añadir(int idP, string coment)
        {
            Comentarios comen = new Comentarios();

            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                          where c.email == correo
                          select c).ToList().FirstOrDefault();
            int idC = cl.Id_cliente;

            comen.comentario = coment;
            comen.Id_cliente = idC;
            comen.id_producto = idP;

            db.Comentarios.Add(comen);
            db.SaveChanges();

            return RedirectToAction("Mostrar");
        }
    }
}