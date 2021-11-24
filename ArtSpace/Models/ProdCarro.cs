using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtSpace.Models
{
    public class ProdCarro
    {
        private contextArtSpace db = new contextArtSpace();
        private List<producto> products;
        public ProdCarro()
        {
            products = db.producto.ToList();
        }

        public List<producto> findAll()
        {
            return this.products;
        }

        public producto find(int id)
        {
            producto pp = this.products.Single(p => p.Id_producto.Equals(id));
            return pp;
        }
    }
}