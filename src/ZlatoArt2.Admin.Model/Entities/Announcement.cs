using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Announcement : MongoEntity, ILanguageEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Text { get; set; }

        [BsonSerializer(typeof(ObjectIdSerializer))]
        public string LanguageId { get; set; }
    }
}
