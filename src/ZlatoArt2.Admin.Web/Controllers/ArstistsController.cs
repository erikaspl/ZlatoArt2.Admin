using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Web.Controllers
{
    [Route("api/[controller]")]
    public class ArtistsController : Controller
    {
        private IMongoRepository<Artist> _artistRepo;
        public ArtistsController(IMongoRepository<Artist> artistRepo)
        {
            _artistRepo = artistRepo;
        }

        [HttpGet]
        public async Task<ObjectResult> Get()
        {
            var artists = await _artistRepo.GetAll();
            return new HttpOkObjectResult(artists);
        }

        [HttpGet("artistId/{artistId}")]
        public ObjectResult Get(int artistId)
        {
            var artists = _artistRepo.Where(a => a.ArtistId == artistId).ToList();
            return new HttpOkObjectResult(artists);
        }

        [HttpGet("id/{id}")]
        public async Task<ObjectResult> Get(string id)
        {
            if (!_artistRepo.IsObjectIdValid(id))
            {
                return new BadRequestObjectResult("Failed");
            }
            var artist = await _artistRepo.GetById(id);
            return new HttpOkObjectResult(artist);
        }
    }
}