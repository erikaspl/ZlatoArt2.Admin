using MongoDB.Driver;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ZlatoArt2.Admin.Db.Mongo;
using ZlatoArt2.Admin.Db.Mongo.Repositories;
using ZlatoArt2.Admin.Integration.Tests.Infrastructure;
using ZlatoArt2.Admin.Model.Entities;
using ZlatoArt2.Admin.Web.Tests.Infrastructure;

namespace ZlatoArt2.Admin.Web.Tests.Controllers
{
    public class ArtistsControllerTests : ControllerTests
    {
        private MongoRepository<Artist> _artistRepo;
        private List<Artist> _artistList;
        public ArtistsControllerTests()
        {
            _artistRepo = new MongoRepository<Artist>(_url, CollectionNames.ArtistCollection);
        }

        [SetUp]
        public override void Init()
        {
            base.Init();
            _artistList = new List<Artist>(new Artist[] {
                new Artist() { FirstName = "Artist1", MiddleName = "group1", LastName = "LastName1", ArtistId = 1, Language = "en" },
                new Artist() { FirstName = "Artist2", MiddleName = "group2", LastName = "LastName2", ArtistId = 2, Language = "en" },
                new Artist() { FirstName = "Artist3", MiddleName = "group1", LastName = "LastName3", ArtistId = 3, Language = "en" },
                new Artist() { FirstName = "Artist4", MiddleName = "group2", LastName = "LastName4", ArtistId = 4, Language = "en" },
                new Artist() { FirstName = "Artist5", MiddleName = "group1", LastName = "LastName5", ArtistId = 5, Language = "en" },
                new Artist() { FirstName = "Artist1", MiddleName = "group1", LastName = "LastName1", ArtistId = 1, Language = "ru" },
                new Artist() { FirstName = "Artist2", MiddleName = "group2", LastName = "LastName2", ArtistId = 2, Language = "ru" },
                new Artist() { FirstName = "Artist3", MiddleName = "group1", LastName = "LastName3", ArtistId = 3, Language = "ru" },
                new Artist() { FirstName = "Artist4", MiddleName = "group2", LastName = "LastName4", ArtistId = 4, Language = "ru" },
                new Artist() { FirstName = "Artist5", MiddleName = "group1", LastName = "LastName5", ArtistId = 5, Language = "ru" },
            });

            //Insert Batch
            _artistRepo.Add(_artistList);
        }

        [TestFixture]
        public class GetTests : ArtistsControllerTests
        {
            [Test, UseDatabase]
            public void GetAllTest()
            {
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync("api/artists");

                    var list = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Artist>>().Result;

                    Assert.AreEqual(list.Count(), _artistList.Count());

                    Artist[] array = list.ToArray();

                    var index = 0;
                    foreach (var artist in _artistList)
                    {
                        var saved = (Artist)array.GetValue(index);
                        Assert.IsNotNull(saved.Id);
                        Assert.AreNotEqual(new string('0', 24), saved.Id);
                        Assert.AreEqual(saved.FirstName, artist.FirstName);
                        Assert.AreEqual(saved.MiddleName, artist.MiddleName);
                        Assert.AreEqual(saved.LastName, artist.LastName);
                        Assert.AreEqual(saved.ArtistId, artist.ArtistId);
                        index++;
                    }
                }
            }

            [Test, UseDatabase]
            public void GetByArtistIdTest_FindExisting()
            {
                int targetId = 5;
                List<Artist> targetArtists = _artistList.FindAll(a => a.ArtistId == targetId);
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artists/artistId/{0}", targetId));

                    var artists = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Artist>>().Result;
                    Artist[] array = artists.ToArray();

                    var index = 0;
                    foreach (var artist in targetArtists)
                    {
                        var saved = (Artist)array.GetValue(index);
                        Assert.IsNotNull(saved.Id);
                        Assert.AreNotEqual(new string('0', 24), saved.Id);
                        Assert.AreEqual(saved.FirstName, artist.FirstName);
                        Assert.AreEqual(saved.MiddleName, artist.MiddleName);
                        Assert.AreEqual(saved.LastName, artist.LastName);
                        Assert.AreEqual(saved.ArtistId, artist.ArtistId);
                        index++;
                    }
                }
            }

            [Test, UseDatabase]
            public void GetByArtistIdTest_EmptyList()
            {
                int targetId = 115;
                List<Artist> targetArtists = _artistList.FindAll(a => a.ArtistId == targetId);
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artists/artistId/{0}", targetId));

                    var artists = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Artist>>().Result;
                    Assert.AreEqual(artists.Count, 0);
                }
            }

            [Test, UseDatabase]
            public void GetByIdTest_FindExisting()
            {
                var savedArtist = _artistRepo.First();
                string targetId = savedArtist.Id;
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artists/id/{0}", targetId));

                    var artist = ((StreamContent)(response.Result.Content)).ReadAsAsync<Artist>().Result;

                    Assert.AreEqual(savedArtist.FirstName, artist.FirstName);
                    Assert.AreEqual(savedArtist.MiddleName, artist.MiddleName);
                    Assert.AreEqual(savedArtist.LastName, artist.LastName);
                    Assert.AreEqual(savedArtist.ArtistId, artist.ArtistId);
                    Assert.AreEqual(savedArtist.Id, artist.Id);
                    
                }
            }

            [Test, UseDatabase]
            public void GetByIdTest_BadRequest()
            {
                int targetId = 115;
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artists/id/{0}", targetId));

                    Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
