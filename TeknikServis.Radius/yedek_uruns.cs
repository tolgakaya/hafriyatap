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
    
    public partial class yedek_uruns
    {
        public int yedek_id { get; set; }
        public int musteri_id { get; set; }
        public string urun_aciklama { get; set; }
        public System.DateTime tarih_verilme { get; set; }
        public Nullable<System.DateTime> tarih_donus { get; set; }
        public string Firma { get; set; }
        public string kullanici { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual customer customer { get; set; }
    }
}
