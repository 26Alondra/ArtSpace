using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda1.Models
{
    public class PedidoCliente
    {
        private contextTienda db = new contextTienda();
        private List<orden_producto> detalle_orden;

        public PedidoCliente()
        {
            detalle_orden = db.orden_producto.ToList();
        }

        public Orden orden
        {
            get;
            set;
        }
        public string Fecha
        {
            get;
            set;
        }
        public string envio
        {
            get;
            set;
        }
        public string status
        {
            get;
            set;
        }
        public string estatus
        {
            get;
            set;
        }
        public string Total
        {
            get;
            set;
        }
        public List<orden_producto> ordenProductos
        {
            get;
            set;
        }
    }
}