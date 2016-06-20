using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Image : MongoEntity
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
