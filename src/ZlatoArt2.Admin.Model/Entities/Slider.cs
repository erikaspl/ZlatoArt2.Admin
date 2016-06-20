using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Slider : MongoEntity
    {
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
