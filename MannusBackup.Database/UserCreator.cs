using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MannusBackup.Database
{
    public class UserCreator
    {
        private MannusBackupEntities _db;

        public UserCreator()
        {
            _db = new MannusBackupEntities();
        }

        public void CreateUser(string username)
        {
            var user = new User();
            user.UserName = username;
            _db.AddTobackup_users(user);
            _db.SaveChanges();
        }
    }
}
