using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Web.Controllers
{
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {
        private IMongoRepository<Language> _languageRepo;
        public LanguageController(IMongoRepository<Language> languageRepo)
        {
            _languageRepo = languageRepo;
        }

        [HttpGet]
        public async Task<ObjectResult> Get()
        {
            var lang = await _languageRepo.GetAll();
            return new HttpOkObjectResult(lang);
        }

        [HttpPut]
        public ObjectResult Put([FromBody] Language language)
        {
            //var command = new AddNewLanguage { NewLanguage = language };
            //BusConfig.Bus.Send(BusConfig.BusEndpointName, command);
            _languageRepo.Add(language);
            return new HttpOkObjectResult("Success");
        }

        [HttpPost]
        public async Task<ObjectResult> Post([FromBody] Language language)
        {
            await _languageRepo.Update(language);
            return new HttpOkObjectResult("Success");
        }
    }
}