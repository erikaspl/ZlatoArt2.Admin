using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Db.Mongo;
using ZlatoArt2.Admin.Db.Mongo.Repositories;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Db.Tests.Repository
{
    public class AnnouncementReposiotryTests : MongoRepositoryTests
    {
        private IMongoRepository<Announcement> _announcementRepo;
        private IMongoRepository<Language> _languageRepo;

        public AnnouncementReposiotryTests()
        {
            _announcementRepo = new MongoRepository<Announcement>(_url, CollectionNames.AnnouncementCollection);
            _languageRepo = new MongoRepository<Language>(_url, CollectionNames.LanguageCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var announcement = new Announcement()
            {
                Name = "Announcement header",
                IsActive = true,
                Text = "Announcement text",
                LanguageId = ObjectId.GenerateNewId().ToString()
            };

            await _announcementRepo.Add(announcement);

            Assert.IsNotNull(announcement.Id);

            // fetch it back 
            var alreadyAddedAnn = _announcementRepo.Where(c => c.Name == "Announcement header").Single();

            Assert.IsNotNull(alreadyAddedAnn);
            Assert.AreEqual(alreadyAddedAnn.Name, announcement.Name);
            Assert.AreEqual(alreadyAddedAnn.IsActive, announcement.IsActive);
            Assert.AreEqual(alreadyAddedAnn.LanguageId, announcement.LanguageId);
            Assert.AreEqual(alreadyAddedAnn.Text, announcement.Text);

            var annNewName = "New name for Announcement";
            alreadyAddedAnn.Name = annNewName;

            await _announcementRepo.Update(alreadyAddedAnn);

            await _announcementRepo.GetById(alreadyAddedAnn.Id).ContinueWith((task) =>
            {
                var updatedArtist = task.Result;
                Assert.IsNotNull(updatedArtist);
                Assert.AreEqual(updatedArtist.Name, annNewName);
            });
        }

        [Test]
        public void BatchTest()
        {

            var language = new Language()
            {
                Abbreviation = "language abbreviation",
                Name = "language name",
                EnName = "language en name",
                FlagImage = "flag image link",
                Locale = "en"
            };

            _languageRepo.Add(language);

            var announcementList = new List<Announcement>(new Announcement[] {
                new Announcement() { Name = "Announcement1", Text = "group1", LanguageId = language.Id },
                new Announcement() { Name = "Announcement2", Text = "group2", LanguageId = language.Id  },
                new Announcement() { Name = "Announcement3", Text = "group1", LanguageId = language.Id  },
                new Announcement() { Name = "Announcement4", Text = "group2", LanguageId = language.Id  },
                new Announcement() { Name = "Announcement5", Text = "group1", LanguageId = language.Id  },
            });

            //Insert Batch
            _announcementRepo.Add(announcementList);

            var count = _announcementRepo.Count();

            Assert.AreEqual(count, announcementList.Count);

            foreach (Announcement announcement in announcementList)
            {
                Assert.AreNotEqual(new string('0', 24), announcement.Id);
            }


            //Update Batch
            foreach (var announcement in announcementList)
            {
                announcement.IsActive = false;
            }

            _announcementRepo.Update(announcementList);

            foreach (var announcement in announcementList)
            {
                Assert.AreEqual(announcement.IsActive, false);
            }

            //Delete by criteria
            _announcementRepo.Delete(a => a.Text.StartsWith("group1"));

            var countAfterDel = _announcementRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
