using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Setting : MongoEntity
    {
        public string ImageLocation { get; set; }
        public int Type { get; set; }
    }
}
