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
    
    public partial class pos_tanims
    {
        public pos_tanims()
        {
            this.musteriodemelers = new HashSet<musteriodemeler>();
            this.poshesaps = new HashSet<poshesap>();
        }
    
        public int pos_id { get; set; }
        public string pos_adi { get; set; }
        public int banka_id { get; set; }
        public int ikinci_sure { get; set; }
        public decimal ikinci_komisyon { get; set; }
        public string Firma { get; set; }
    
        public virtual banka banka { get; set; }
        public virtual ICollection<musteriodemeler> musteriodemelers { get; set; }
        public virtual ICollection<poshesap> poshesaps { get; set; }
    }
}
