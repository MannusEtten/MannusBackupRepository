//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MannusBackup.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class backup_results
    {
        public int id { get; set; }
        public System.DateTime datum { get; set; }
        public string host { get; set; }
        public System.TimeSpan tijd { get; set; }
        public string status { get; set; }
        public string naam { get; set; }
        public string password { get; set; }
        public string size { get; set; }
        public string sizeingb { get; set; }
    }
}
