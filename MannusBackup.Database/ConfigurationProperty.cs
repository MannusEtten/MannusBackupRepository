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
    
    public partial class ConfigurationProperty
    {
        public ConfigurationProperty()
        {
            this.backup_profile_configuration = new HashSet<ProfileConfiguration>();
        }
    
        public int id { get; set; }
        public string Name { get; set; }
        public int groupid { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<ProfileConfiguration> backup_profile_configuration { get; set; }
        public virtual ConfigurationPropertyGroup backup_configuration_group { get; set; }
    }
}
