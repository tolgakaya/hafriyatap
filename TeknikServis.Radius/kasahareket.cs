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
    
    public partial class kasahareket
    {
        public int ID { get; set; }
        public string Firma { get; set; }
        public decimal giris { get; set; }
        public decimal cikis { get; set; }
        public bool iptal { get; set; }
        public Nullable<int> Odeme_ID { get; set; }
        public decimal aktif_bakiye { get; set; }
        public string KasaTur { get; set; }
        public string islem { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
        public int Musteri_ID { get; set; }
        public string inserted { get; set; }
        public string updated { get; set; }
        public string deleted { get; set; }
    
        public virtual customer customer { get; set; }
        public virtual musteriodemeler musteriodemeler { get; set; }
    }
}
