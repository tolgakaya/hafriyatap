//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeknikServis.Radius
{
    using System;
    using System.Collections.Generic;
    
    public partial class cihaz_stoks
    {
        public int ID { get; set; }
        public int cihaz_id { get; set; }
        public string cihaz_adi { get; set; }
        public string aciklama { get; set; }
        public string seri_no { get; set; }
        public int garanti_suresi { get; set; }
        public decimal giris { get; set; }
        public decimal cikis { get; set; }
        public decimal bakiye { get; set; }
        public Nullable<decimal> son_alis_fiyati { get; set; }
        public Nullable<decimal> satis_fiyati { get; set; }
        public System.DateTime tarih { get; set; }
        public string Firma { get; set; }
    
        public virtual cihaz cihaz { get; set; }
    }
}
