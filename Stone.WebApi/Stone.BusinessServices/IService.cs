using Stone.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessServices
{
    public interface IService
    {
        Entity GetById(int id);
        IEnumerable<Entity> GetAll();
        int Create(Entity entity);
        bool Update(int id, Entity entity);
        bool Delete(int id);
    }
}
