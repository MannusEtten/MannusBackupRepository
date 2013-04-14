using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MannusBackup.Database
{
    public class ConfigurationPropertyCreator : CreatorBase
    {
        public void CreateConfigurationProperty(string name)
        {
            var configurationProperty = new ConfigurationProperty();
            configurationProperty.Name = name;
            _repository.Create<ConfigurationProperty>(configurationProperty);
            _repository.SaveChanges();
        }
    }
}