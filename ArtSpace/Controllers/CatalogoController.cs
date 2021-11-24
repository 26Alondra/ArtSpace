using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArtSpace.Models;

namespace ArtSpace.Controllers
{
    public class CatalogoController : Controller
    {
        private contextArtSpace db = new contextArtSpace();
       
        // GET: Catalogo
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult buscarProd(string nomBuscar)
        {
            ViewBag.Searchkey = nomBuscar;
            using (db)
            {
                var query = from st in db.producto
                            where st.nombre.Contains(nomBuscar)
                            select st;
                var listProd = query.ToList();
                ViewBag.Listado = listProd;
            }
            return View();
        }

        public ActionResult prodCategoria(int idCat)
        {
            List<producto> mercancia = null;
            List<subcategoria> subcat = null;
            var query = from p in db.producto join sub in db.subcategoria
                        on p.id_subcategoria equals sub.Id_subcategoria
                        where sub.id_categoria == idCat
                        select p;
            var subcatego = from sub in db.subcategoria
                        join catego in db.categoria
                        on sub.id_categoria equals catego.Id_categoria
                        where sub.id_categoria == idCat
                        select sub;
            subcat = subcatego.ToList();
            if (idCat == 1)
            {
                mercancia = query.ToList();
                ViewBag.Catego = "Esculturas relogiosas";
            }
            if (idCat == 2)
            {
                mercancia = query.ToList();
                ViewBag.Catego = "Esculturas abstractas";

            }
            if (idCat == 3)
            {
                mercancia = query.ToList();
                ViewBag.Catego = "Esculturas profanas";
            }
            if (idCat == 4)
            {
                mercancia = query.ToList();
                ViewBag.Catego = "Esculturas realistas";
            }
            ViewBag.productos = mercancia;
            ViewBag.subcategoria = subcat;
            return View();
        }

        public ActionResult prodSubcategoria(int idsubcatego, int idCategoria, string nomSubCatego)
        {
            List<subcategoria> subcat = null;
            List<producto> bussubcatego = null;
            var subcatego = from sub in db.subcategoria
                            join catego in db.categoria
                            on sub.id_categoria equals catego.Id_categoria
                            where sub.id_categoria == idCategoria
                            select sub;
            subcat = subcatego.ToList();
            var querysub = from p in db.producto
                        join sub in db.subcategoria
                        on p.id_subcategoria equals sub.Id_subcategoria
                        where sub.id_categoria == idCategoria && p.id_subcategoria == idsubcatego
                        select p;
            bussubcatego = querysub.ToList();
            ViewBag.productosSub = bussubcatego;
            ViewBag.subcategoSelect = nomSubCatego;
            ViewBag.prodSub = subcat;
            if (idsubcatego == 1)
            {
                ViewBag.CategoSubcatego = "realistas";
            }
            if (idsubcatego == 2)
            {
                ViewBag.CategoSubcatego = "realistas";

            }
            if (idsubcatego == 3)
            {
                ViewBag.CategoSubcatego = "realistas";
            }
            if (idsubcatego == 4)
            {
                ViewBag.CategoSubcatego = "realistas";
            }
            return View();
        }

        public ActionResult verProducto(int idProducto)
        {
            List<producto> productoVer = null;
            var query = from p in db.producto
                        where p.Id_producto == idProducto
                        select p;
            productoVer = query.ToList();
            ViewBag.productoSeleccionado = productoVer;
            return View();
        }
    }
}