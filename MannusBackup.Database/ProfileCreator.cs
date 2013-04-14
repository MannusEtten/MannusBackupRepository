using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MannusBackup.Interfaces;

namespace MannusBackup.Database
{
    public class ProfileCreator : CreatorBase
    {
        public void CreateProfile(EnumProfileType profileType)
        {
            var profile = CreateNewClientProfile();
            _repository.Create<Profile>(profile);
            _repository.SaveChanges();
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