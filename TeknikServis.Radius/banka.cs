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
    
    public partial class banka
    {
        public banka()
        {
            this.musteriodemelers = new HashSet<musteriodemeler>();
            this.pos_tanims = new HashSet<pos_tanims>();
        }
    
        public int banka_id { get; set; }
        public string banka_adi { get; set; }
        public string aciklama { get; set; }
        public string Firma { get; set; }
        public decimal giris { get; set; }
        public decimal cikis { get; set; }
        public decimal bakiye { get; set; }
    
        public virtual ICollection<musteriodemeler> musteriodemelers { get; set; }
        public virtual ICollection<pos_tanims> pos_tanims { get; set; }
    }
}
