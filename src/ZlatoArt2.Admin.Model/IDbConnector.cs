using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model
{
    public interface IDbConnector
    {
        IMongoDatabase GetDatabase();
        string GetConnectionString();
    }
}
