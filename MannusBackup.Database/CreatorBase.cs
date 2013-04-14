using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MannusBackup.Interfaces;

namespace MannusBackup.Database
{
    public abstract class CreatorBase
    {
        protected IRepository _repository;

        public CreatorBase()
        {
            _repository = new Repository();
        }
    }
}