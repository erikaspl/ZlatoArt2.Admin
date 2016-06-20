using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Artist : MongoEntity
    {
        public int ArtistId { get; set; }
        public string Text { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public string ImageLink { get; set; }
        public short DisplayOrder { get; set; }
        public string Hash { get; set; }
    }
}
