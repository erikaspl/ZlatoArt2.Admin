using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Artwork : MongoEntity
    {
        public int ArtworkId { get; set; }
        public string Name { get; set; }
        public string ThumbLink { get; set; }
        public bool IsSold { get; set; }
        public string ImageLink { get; set; }
        public int DisplayOrder { get; set; }
        public int ArtistId { get; set; }
        public string Language { get; set; }
    }
}
