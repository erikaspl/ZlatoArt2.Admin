using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Db.Mongo;
using ZlatoArt2.Admin.Db.Mongo.Repositories;
using ZlatoArt2.Admin.Integration.Tests.Infrastructure;
using ZlatoArt2.Admin.Model.Entities;
using ZlatoArt2.Admin.Web.Tests.Infrastructure;

namespace ZlatoArt2.Admin.Web.Tests.Controllers
{
    public class ArtworksControllerTests : ControllerTests
    {
        private MongoRepository<Artwork> _artworkRepo;
        private List<Artwork> _artworkList;
        public ArtworksControllerTests()
        {
            _artworkRepo = new MongoRepository<Artwork>(_url, CollectionNames.ArtworkCollection);
        }

        [SetUp]
        public override void Init()
        {
            base.Init();
            _artworkList = new List<Artwork>(new Artwork[] {
                new Artwork() { Name = "Artist1", ArtistId = 1, IsSold = false },
                new Artwork() { Name = "Artist2", ArtistId = 2, IsSold = false },
                new Artwork() { Name = "Artist3", ArtistId = 1, IsSold = false },
                new Artwork() { Name = "Artist4", ArtistId = 2, IsSold = false },
                new Artwork() { Name = "Artist5", ArtistId = 1, IsSold = false },
            });


            //Insert Batch
            _artworkRepo.Add(_artworkList);
        }

        [TestFixture]
        public class GetTests : ArtworksControllerTests
        {
            [Test, UseDatabase]
            public void GetAllTest()
            {
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync("api/artworks");

                    var list = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Artwork>>().Result;

                    Assert.AreEqual(list.Count(), _artworkList.Count());

                    Artwork[] array = list.ToArray();

                    var index = 0;
                    foreach (var artwork in _artworkList)
                    {
                        var saved = (Artwork)array.GetValue(index);
                        Assert.IsNotNull(saved.Id);
                        Assert.AreNotEqual(new string('0', 24), saved.Id);
                        Assert.AreEqual(saved.Name, artwork.Name);
                        Assert.AreEqual(saved.ArtistId, artwork.ArtistId);
                        Assert.AreEqual(saved.IsSold, artwork.IsSold);
                        index++;
                    }
                }
            }

            [Test, UseDatabase]
            public void GetByArtistIdTest_FindExisting()
            {
                int targetId = 1;
                List<Artwork> targetArtists = _artworkList.FindAll(a => a.ArtworkId == targetId);
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artworks/artworkId/{0}", targetId));

                    var artworks = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Artwork>>().Result;
                    Artwork[] array = artworks.ToArray();

                    var index = 0;
                    foreach (var artist in targetArtists)
                    {
                        var saved = (Artwork)array.GetValue(index);
                        Assert.IsNotNull(saved.Id);
                        Assert.AreNotEqual(new string('0', 24), saved.Id);
                        Assert.AreEqual(saved.Name, artist.Name);
                        Assert.AreEqual(saved.ArtistId, artist.ArtistId);
                        Assert.AreEqual(saved.IsSold, artist.IsSold);
                        index++;
                    }
                }
            }

            [Test, UseDatabase]
            public void GetByIdTest_EmptyList()
            {
                int targetId = 115;
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artworks/artworkId/{0}", targetId));

                    var artworks = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Artwork>>().Result;
                    Assert.AreEqual(artworks.Count, 0);
                }
            }

            [Test, UseDatabase]
            public void GetByIdTest_FindExisting()
            {
                var savedArtwork = _artworkRepo.First();
                string targetId = savedArtwork.Id;
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artworks/id/{0}", targetId));

                    var artwork = ((StreamContent)(response.Result.Content)).ReadAsAsync<Artwork>().Result;

                    Assert.AreEqual(savedArtwork.Name, artwork.Name);
                    Assert.AreEqual(savedArtwork.ArtistId, artwork.ArtistId);
                    Assert.AreEqual(savedArtwork.IsSold, artwork.IsSold);
                    Assert.AreEqual(savedArtwork.Id, artwork.Id);
                }
            }

            [Test, UseDatabase]
            public void GetByIdTest_BadRequest()
            {
                int targetId = 201;
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync(string.Format("api/artworks/id/{0}", targetId));

                    Assert.AreEqual(response.Result.StatusCode, HttpStatusCode.BadRequest);
                }
            }
        }
    }
}
