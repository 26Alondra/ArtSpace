using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda1.Models;

namespace Tienda1.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        contextTienda db = new contextTienda();
        // GET: Pedidos
        public ActionResult Index()
        {
            string correo = User.Identity.Name;
            cliente cl = (from c in db.cliente
                          where c.email == correo
                          select c).ToList().FirstOrDefault();
            int id = cl.Id_cliente;

            var query = from o in db.Orden
                        where o.id_cliente == id
                        orderby o.fecha_creacion ascending
                        select o;
            List<Orden> ordenes = query.ToList();

            List<PedidoCliente> pedidos = new List<PedidoCliente>();
            PedidoCliente pedido;
            List<orden_producto> ordPed;
            List<ItemPedido> itemPed = new List<ItemPedido>();

            ItemPedido iPed;
            foreach(Orden o in ordenes)
            {
                pedido = new PedidoCliente();
                pedido.orden = o;
                pedido.Fecha = o.fecha_creacion.GetValueOrDefault().ToShortDateString();
                if(o.fecha_envio.HasValue)
                {
                    pedido.envio = o.fecha_envio.GetValueOrDefault().ToShortDateString();

                }
                else
                {
                    pedido.envio = "Proximamente";
                }
                if(o.fecha_entrega.HasValue)
                {
                    pedido.status = o.fecha_entrega.GetValueOrDefault().ToShortDateString();
                }
                else
                {
                    pedido.status = "Sin entregar";
                }
                pedido.Total = o.Total.ToString();
                pedidos.Add(pedido);
                ordPed = (from oP in db.orden_producto
                          where oP.id_orden == o.Id_orden
                          select oP).ToList();
                pedido.ordenProductos = ordPed;
                foreach(orden_producto op in ordPed)
                {
                    iPed = new ItemPedido();
                    iPed.idOrd = op.id_orden;
                    iPed.Product = db.producto.First(p => p.Id_producto == op.id_producto);
                    iPed.Cantidad = Convert.ToInt32(op.cantidad);
                    itemPed.Add(iPed);
                }
            }
            Session["misPedidos"] = pedidos;
            Session["Pedido"] = itemPed;

            return View();
        }
        public ActionResult VerMas(int idOr)
        {
            string correo = User.Identity.Name;
            var cliente = (from c in db.cliente
                           where c.email == correo
                           select c).ToList().FirstOrDefault();

            Orden od = (from or in db.Orden
                        where or.Id_orden == idOr
                        select or).ToList().FirstOrDefault();

            orden_producto orp = (from orpt in db.orden_producto
                                  where orpt.id_orden == idOr
                                  select orpt).ToList().FirstOrDefault();
            producto pd = (from pr in db.producto
                           where pr.Id_producto == orp.id_producto
                           select pr).ToList().FirstOrDefault();

            Paqueteria pa = (from pq in db.Paqueteria
                             where pq.IdPaqueteria == od.id_paqueteria
                             select pq).ToList().FirstOrDefault();

            Session["Oprodu"]= pd.imagen;
            Session["dir"] = cliente.calle + " " + cliente.colonia + " " + cliente.estado + " " + cliente.codigo_postal;
            Session["idOrd"] = idOr;
            Session["OPaq"]=pa.Nombre+" "+pa.RFC+" "+ pa.telContacto;
            Session["can"] = orp.cantidad;


            return View();
        }
    }
}