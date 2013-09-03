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
            prop1.Description = "Aantal backups lokaal bewaren";
            var prop2 = new ProfileProperty();
            prop2.Name = "SqlYog";
            prop2.Value = @"C:\Program Files (x86)\SQLyog\SJA.exe";
            prop2.Description = "Locatie SqlYog programma";
            profile.Name = "unit test";
            profile.Properties.Add(prop1);
            profile.Properties.Add(prop2);
            return profile;
        }
    }
}