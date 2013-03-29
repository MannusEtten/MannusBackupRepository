using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MannusBackup.Database
{
    public class ProfileCreator
    {
        private MannusBackupEntities _db;

        public ProfileCreator()
        {
            _db = new MannusBackupEntities();
        }


        public void CreateProfile(User user, EnumProfileType profileType)
        {
//            var profile = CreateNewClientProfile();
//            user.Profiles.Add(profile);
//            _db.AddTobackup_users(user);
//            _db.SaveChanges();
        }

        private Profile CreateNewClientProfile()
        {
            var profile = new Profile();
            profile.ProfileType = EnumProfileType.Client.ToString();

            // TODO: toevoegen van alle properties
            var prop1 = new ProfileProperty();
            prop1.Name = "NumberOfLocalBackups";
            prop1.Value = "5";

            profile.ProfileProperties.Add(prop1);
            return profile;
        }
    }
}