//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ArtSpace.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class metPago
    {
        public int Id_pago { get; set; }
        public string numero_tarjeta { get; set; }
        public string fecha_vencimiento { get; set; }
        public Nullable<int> id_cliente { get; set; }
    
        public virtual cliente cliente { get; set; }
    }
}
