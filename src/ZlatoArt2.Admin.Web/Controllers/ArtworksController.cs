using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Web.Controllers
{
    [Route("api/[controller]")]
    public class ArtworksController : Controller
    {
        private IMongoRepository<Artwork> _artworkRepo;
        public ArtworksController(IMongoRepository<Artwork> artworkRepo)
        {
            _artworkRepo = artworkRepo;
        }

        [HttpGet]
        public async Task<ObjectResult> Get()
        {
            var artworks = await _artworkRepo.GetAll();
            return new HttpOkObjectResult(artworks);
        }

        [HttpGet("artworkId/{artworkId}")]
        public ObjectResult Get(int artistId)
        {
            var artworks = _artworkRepo.Where(a => a.ArtistId == artistId).ToList();
            return new HttpOkObjectResult(artworks);
        }

        [HttpGet("id/{id}")]
        public async Task<ObjectResult> Get(string id)
        {
            if (!_artworkRepo.IsObjectIdValid(id))
            {
                return new BadRequestObjectResult("Failed");
            }
            var artwork = await _artworkRepo.GetById(id);
            return new HttpOkObjectResult(artwork);
        }
    }
}