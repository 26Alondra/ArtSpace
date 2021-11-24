﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda1.Models;

namespace Tienda1.Controllers
{
    [Authorize]
    public class ComentarioController : Controller
    {
        contextTienda db = new contextTienda();
        // GET: Comentarios
        public ActionResult Index()
        {
            return View();
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
            string nombreP =pr.nombre;
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
            var query= (from cm in db.Comentarios
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

        public ActionResult Añadir(int idP,string coment)
        {
            Comentarios comen = new Comentarios();

            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                          where c.email == correo
                          select c).ToList().FirstOrDefault();
            int idC = cl.Id_cliente;

            comen.comentario = coment;
            comen.Id_cliente =idC;
            comen.id_producto = idP;

            db.Comentarios.Add(comen);
            db.SaveChanges();

            return RedirectToAction("Mostrar");
        }
    }
}