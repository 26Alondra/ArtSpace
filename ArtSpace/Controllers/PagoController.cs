using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Tienda1.Models;

namespace Tienda1.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private contextTienda db = new contextTienda();
        private string NumConfirPago;

        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CrearOrden()
        {
            if(!User.Identity.IsAuthenticated)
            {
                Session["CrearOrden"] = "pend";
                return RedirectToAction("Login", "Account");
            }

            string correo = User.Identity.Name;

            //var orden = new orden();
           // var db = new contextTienda();
            string fechaCreacion = DateTime.Today.ToShortDateString();
            string fechaProbEntrega = DateTime.Today.AddDays(3).ToShortDateString();
            var cliente = (from c in db.cliente
                           where c.email == correo
                           select c).ToList().FirstOrDefault();

            Session["dirCliente"] = cliente.calle + " " + cliente.colonia + " " + cliente.estado+ " "+ cliente.codigo_postal;
            Session["fechaOrden"] = fechaCreacion;
            Session["fPEntreg"] = fechaProbEntrega;

            if (cliente.num_tarj.StartsWith("4"))
                Session["tTarj"] = "1";
            if (cliente.num_tarj.StartsWith("5"))
                Session["tTarj"] = "2";
            if (cliente.num_tarj.StartsWith("3"))
                Session["tTarj"] = "3";
            Session["nTarj"] = cliente.num_tarj;
            return View();
        }
        public ActionResult Pagar(string tipoPago)
        {
            string correo = User.Identity.Name;

            DateTime fechaCreacion = DateTime.Today;
            DateTime fechaProbEntrega = fechaCreacion.AddDays(3);
            var cliente = (from c in db.cliente
                           where c.email == correo
                           select c).ToList().FirstOrDefault();
            int idClient = cliente.Id_cliente;

            if(tipoPago.Equals("T"))
            {
                if(!validaPago(cliente))
                {
                    return RedirectToAction("pagoNoAceptado");
                }
                else
                {
                    var dirEnt = (from d in db.dirEntrega
                                  where d.id_cliente == cliente.Id_cliente
                                  select d).ToList().FirstOrDefault();

                    int idDir = dirEnt.Id_dirEnt;
                    return RedirectToAction("pagoAceptado", routeValues: new { idC = idClient, idD = idDir });
                }
            }
            if(tipoPago.Equals("P"))
            {
                var dirEnt = (from d in db.dirEntrega
                              where d.id_cliente == cliente.Id_cliente
                              select d).ToList().FirstOrDefault();

                int idDir = dirEnt.Id_dirEnt;
                validaPago(cliente);
                return RedirectToAction("pagoPaypal", routeValues: new { idC = idClient, idD = idDir });

            }
            return View();
        }
        //11:39
        public ActionResult pagoNoAceptado()
        {

            return View();
        }

        public ActionResult pagoAceptado(int idC, int idD)
        {
            //Aceptado el pago creamos los datos de la orden
            Orden orden_cliente = new Orden();
            int id = 0;
            if(!(db.Orden.Max(o =>(int?)o.Id_orden)== null))
            {
                id = db.Orden.Max(o => o.Id_orden);
            }
            else
            {
                id = 0;
            }
            id++;
            orden_cliente.Id_orden = id;
            orden_cliente.fecha_creacion = DateTime.Today;
            orden_cliente.num_confirmacion = Convert.ToDecimal(Session["nConfirma"].ToString());
            var carro = Session["cart"] as List<Item>;
            var total = carro.Sum(item => item.Product.precio * item.Cantidad);
            orden_cliente.Total = Convert.ToDecimal(total);
            orden_cliente.id_cliente = idC;
            db.Orden.Add(orden_cliente);
            db.SaveChanges();

            orden_producto ordenProd;
            List<orden_producto> listaProdOrd = new List<orden_producto>();
            foreach(Item linea in carro)
            {
                ordenProd = new orden_producto();
                ordenProd.id_orden = orden_cliente.Id_orden;
                ordenProd.id_producto = linea.Product.Id_producto;
                ordenProd.cantidad = linea.Cantidad;
                db.orden_producto.Add(ordenProd);


            }
            db.SaveChanges();

            Session["cart"]= null;
            Session["nConfirma"] = null;
            Session["itemTotal"] = null;
            return View();
        }

        public ActionResult pagoPaypal(int idC, int idD)
        {
            Session["idDir"] = idC;
            Session["idClient"] = idD;
            return View();
        }

        public ActionResult pagandoPaypal(int idC, int idD)
        {
            Session["idDir"] = idC;
            Session["idClient"] = idD;

            return View();
        }

        private bool validaPago(cliente cliente)
        {
            bool retorna = true;

            int randomvalue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] val = new byte[6];
                crypto.GetBytes(val);
                randomvalue = BitConverter.ToInt32(val, 1);
            }

            NumConfirPago = Math.Abs(randomvalue * 1000).ToString();
            Session["nConfirma"] = NumConfirPago;
            return retorna;
        }
    }
}