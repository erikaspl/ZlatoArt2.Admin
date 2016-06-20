using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Db.Mongo;
using ZlatoArt2.Admin.Db.Mongo.Repositories;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Db.Tests.Repository
{
    [TestFixture]
    public class ArtistRepositoryTests : MongoRepositoryTests
    {        
        private IMongoRepository<Artist> _artistRepo;

        public ArtistRepositoryTests()
        {
            _artistRepo = new MongoRepository<Artist>(_url, CollectionNames.ArtistCollection);
        }

        [Test]
        public void AddAndUpdateTest()
        {           
            var artist = new Artist(){
                ArtistId = 1000,
                FirstName = "New Artist first name",
                MiddleName = "New Artist middle name",
                LastName = "New Artist last name", 
                DisplayName = "New Artist display",
                Text = "About the artist",
                LanguageId = 1,
                Language = "en",
                IsActive = true,
                ImageLink = "ArtistImageLing",
                DisplayOrder = 1,
                Hash = "newartsithash"          
            };

            _artistRepo.Add(artist);

            Assert.IsNotNull(artist.Id);

            // fetch it back 
            var alreadyAddedArtist = _artistRepo.Where(c => c.FirstName == "New Artist first name").Single();

            Assert.IsNotNull(alreadyAddedArtist);
            Assert.AreEqual(alreadyAddedArtist.FirstName, artist.FirstName);
            Assert.AreEqual(alreadyAddedArtist.LastName, artist.LastName);
            Assert.AreEqual(alreadyAddedArtist.MiddleName, artist.MiddleName);
            Assert.AreEqual(alreadyAddedArtist.ArtistId, artist.ArtistId);
            Assert.AreEqual(alreadyAddedArtist.DisplayName, artist.DisplayName);
            Assert.AreEqual(alreadyAddedArtist.DisplayOrder, artist.DisplayOrder);
            Assert.AreEqual(alreadyAddedArtist.IsActive, artist.IsActive);
            Assert.AreEqual(alreadyAddedArtist.Language, artist.Language);
            Assert.AreEqual(alreadyAddedArtist.LanguageId, artist.LanguageId);
            Assert.AreEqual(alreadyAddedArtist.ImageLink, artist.ImageLink);
            Assert.AreEqual(alreadyAddedArtist.Hash, artist.Hash);
            Assert.AreEqual(alreadyAddedArtist.Text, artist.Text);

            var artistNewName = "New name for artist";
            alreadyAddedArtist.FirstName = artistNewName;

            _artistRepo.Update(alreadyAddedArtist);

            _artistRepo.GetById(alreadyAddedArtist.Id).ContinueWith((task) =>
            {
                var updatedArtist = task.Result;
                Assert.IsNotNull(updatedArtist);
                Assert.AreEqual(updatedArtist.FirstName, artistNewName);
            });            
        }

        [Test]
        public void BatchTest()
        {
            var artistList = new List<Artist>(new Artist[] {
                new Artist() { FirstName = "Artist1", MiddleName = "group1" },
                new Artist() { FirstName = "Artist2", MiddleName = "group2" },
                new Artist() { FirstName = "Artist3", MiddleName = "group1" },
                new Artist() { FirstName = "Artist4", MiddleName = "group2" },
                new Artist() { FirstName = "Artist5", MiddleName = "group1" },
            });


            //Insert Batch
            _artistRepo.Add(artistList);

            var count = _artistRepo.Count();

            Assert.AreEqual(count, artistList.Count);

            foreach (Artist artist in artistList)
            {
                Assert.AreNotEqual(new string('0', 24), artist.Id);
            }


            //Update Batch
            foreach (var artist in artistList)
            {
                artist.LastName = artist.FirstName;
            }

            _artistRepo.Update(artistList);

            foreach(var artist in artistList)
            {
                Assert.AreEqual(artist.FirstName, artist.LastName);
            }

            //Delete by criteria
            _artistRepo.Delete(a => a.MiddleName.StartsWith("group1"));

            var countAfterDel = _artistRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
