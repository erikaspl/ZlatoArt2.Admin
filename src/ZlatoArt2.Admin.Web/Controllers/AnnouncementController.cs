using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Web.Controllers
{
    [Route("api/[controller]")]
    public class AnnouncementController : Controller
    {
        private IMongoRepository<Announcement> _announcementRepo;
        public AnnouncementController(IMongoRepository<Announcement> announcementRepo)
        {
            _announcementRepo = announcementRepo;
        }

        [HttpGet]
        public async Task<ObjectResult> Get(string languageId)
        {
            var announcements = await _announcementRepo.GetAll();
            return new HttpOkObjectResult(announcements);
        }


        [HttpPost]
        public async Task<ObjectResult> Post([FromBody] Announcement announcement )
        {
            await _announcementRepo.Update(announcement);
            return new HttpOkObjectResult("Success");
        }
    }
}
