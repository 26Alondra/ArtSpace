using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda1.Models;

namespace Tienda1.Controllers
{
    public class CarroController : Controller
    {
        // GET: Carro
        public ActionResult Index()
        {
            if (Session["cart"] == null)
                Session["cart"] = new List<Item>();
            return View();
        }

        public ActionResult Agregar(int id)
        {
            int num = 0;
            if (Session["itemTotal"] == null)
            {
                num = 0;
                Session["itemTotal"] = num;
            }
            else
            {
                num = int.Parse(Session["itemTotal"].ToString());
                num = num + 1;
                Session["itemTotal"] = num;
            }

            ProdCarro carro = new ProdCarro();
            if(Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                producto p = carro.find(id);
                string nam = p.nombre;
                cart.Add(new Item { Product = carro.find(id), Cantidad = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<Item> cart = (List<Item>)Session["cart"];
                int index = isExist(id);
                if(index != -1)
                {
                    cart[index].Cantidad++;
                }
                else
                {
                    producto p = carro.find(id);
                    string nam = p.nombre;
                    cart.Add(new Item { Product = carro.find(id), Cantidad = -1 });
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Quitar(int id)
        {
            int num = 0;
            if (Session["itemTotal"] == null)
            {
                num = 0;
                Session["itemTotal"] = num;
            }
            else
            {
                num = int.Parse(Session["itemTotal"].ToString());
                num = num - 1;
                Session["itemTotal"] = num;
            }

            List<Item> cart = (List<Item>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return RedirectToAction("Index");
        }
        private int isExist(int id)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id_producto.Equals(id))
                    return i;
            return -1;
        }
    }
}