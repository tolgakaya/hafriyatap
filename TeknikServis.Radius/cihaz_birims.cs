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
    
    public partial class cihaz_birims
    {
        public cihaz_birims()
        {
            this.cihazs = new HashSet<cihaz>();
            this.cihazs1 = new HashSet<cihaz>();
            this.makine_masrafs = new HashSet<makine_masrafs>();
        }
    
        public int id { get; set; }
        public string birim { get; set; }
        public bool iptal { get; set; }
    
        public virtual ICollection<cihaz> cihazs { get; set; }
        public virtual ICollection<cihaz> cihazs1 { get; set; }
        public virtual ICollection<makine_masrafs> makine_masrafs { get; set; }
    }
}