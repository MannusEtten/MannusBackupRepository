using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MannusBackup.Database
{
    public class ProfileCreator
    {
        private MannusEntities _db;
        public ProfileCreator()
        {
            _db = new MannusEntities();
        }


        public void CreateProfile(EnumProfileType profileType)
        {
            var profile = CreateNewClientProfile();
//            user.Profiles.
 //           user.Profiles.Add(profile);
  //          user.
//            _db.AddTobackup_users(user);
            _db.backup_profile.Add(profile);
            _db.SaveChanges();
        }

        private Profile CreateNewClientProfile()
        {
            var profile = new Profile();
            profile.ProfileType = EnumProfileType.Client.ToString();

            // TODO: toevoegen van alle properties
            var prop1 = new ProfileProperty();
            prop1.Name = "NumberOfLocalBackups";
            prop1.Value = "5";
            profile.Properties.Add(prop1);
            return profile;
        }
    }
}