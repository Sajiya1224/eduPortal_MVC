using eduPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eduPortal.DataAccess.Repository.IRepository
{
    public interface ISectionReposirory: IRepository<Section>
    {
        void Update(Section obj);
    }
}
