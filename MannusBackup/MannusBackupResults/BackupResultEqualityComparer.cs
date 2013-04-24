using System.Collections.Generic;
using System;

namespace MannusBackup.BackupResults
{
    internal class BackupResultEqualityComparer : IEqualityComparer<BackupResult>
    {
        public bool Equals(BackupResult x, BackupResult y)
        {
            return string.Equals(x.Naam, y.Naam, StringComparison.InvariantCulture) && string.Equals(x.Host, y.Host, StringComparison.InvariantCulture);
        }

        public int GetHashCode(BackupResult obj)
        {
            return obj.Naam.ToLowerInvariant().GetHashCode() + obj.Host.ToLowerInvariant().GetHashCode();
        }
    }
}