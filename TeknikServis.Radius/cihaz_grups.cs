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
    
    public partial class cihaz_grups
    {
        public cihaz_grups()
        {
            this.cihazs = new HashSet<cihaz>();
        }
    
        public int grupid { get; set; }
        public string grupadi { get; set; }
        public Nullable<decimal> kdv { get; set; }
        public Nullable<decimal> oiv { get; set; }
        public Nullable<decimal> otv { get; set; }
    
        public virtual ICollection<cihaz> cihazs { get; set; }
    }
}
