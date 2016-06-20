using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZlatoArt2.Admin.Model.Entities
{
    public class Language : MongoEntity
    {
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string FlagImage { get; set; }
        public string Locale { get; set; }
        public string Background { get; set; }
        public string Icon { get; set; }
    }
}
