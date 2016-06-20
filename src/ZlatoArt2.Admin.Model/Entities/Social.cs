using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Social : MongoEntity
    {
        public string LrgImage { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string MidImage { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
    }
}
