using ArtSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSpace.Controllers
{
    public class EnviosController : Controller
    {
        private EsculturasBDEntities1 db = new EsculturasBDEntities1();

        // GET: Envios
        public ActionResult Index()
        {
            return View(db.Paqueteria.ToList());
        }
    }
}