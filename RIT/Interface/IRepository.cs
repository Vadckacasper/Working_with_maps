using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIT.Interfece
{
    interface IRepository<T>
    {
        public IEnumerable<T> GetAll();
        public bool Update(T entity);
        public bool Insert(T entity);
        public bool Delete(int id);
    }
}
