using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIT.Data
{
    public class MyDbContext
    {
        public readonly RepositoryMarker DbMarker;

        public MyDbContext()
        {
            DbMarker = new RepositoryMarker(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
    }
}
