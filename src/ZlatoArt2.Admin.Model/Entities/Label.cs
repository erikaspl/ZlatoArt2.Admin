using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Label : MongoEntity
    {
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
    }
}
