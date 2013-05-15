using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MannusBackup.Interfaces;

namespace MannusBackup.Database
{
    public class ConfigurationRepository
    {
        private Profile _profile;

        public ConfigurationRepository(Profile profile)
        {
            _profile = profile;
        }

        public SqlYogConfiguration GetDatabaseTaskConfiguration()
        {
            var result = new SqlYogConfiguration();
            var generalSqlYogProfileProperty = GetGeneralSqlYogProfileProperty();
            var configurations = GetConfigurationGroups(ProfileProperties.SqlYog);
            result.Configurations = configurations;
            result.SqlYogProfileProperty = generalSqlYogProfileProperty;
            return result;
        }

        private IEnumerable<ProfileConfigurationGroup> GetConfigurationGroups(ProfileProperties type)
        {
            var result = _profile.ConfigurationGroups.Where(g => string.Equals(g.Type, type.ToString(), StringComparison.InvariantCultureIgnoreCase));
            return result;
        }

        private ProfileProperty GetGeneralSqlYogProfileProperty()
        {
            var sqlYogProfileProperty = _profile.Properties.Where(p => p.Name.Equals(ProfileProperties.SqlYog.ToString())).First();
            return sqlYogProfileProperty;
        }
    }
}