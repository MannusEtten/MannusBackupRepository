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
    
    public partial class Profile
    {
        public Profile()
        {
            this.Properties = new HashSet<ProfileProperty>();
            this.ConfigurationGroups = new HashSet<ProfileConfigurationGroup>();
        }
    
        public int Id { get; set; }
        public string ProfileType { get; set; }
    
        public virtual ICollection<ProfileProperty> Properties { get; set; }
        public virtual ICollection<ProfileConfigurationGroup> ConfigurationGroups { get; set; }
    }
}
