using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Db.Mongo;
using ZlatoArt2.Admin.Db.Mongo.Repositories;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Db.Tests.Repository
{
    public class SocialRepositoryTests : MongoRepositoryTests
    {
        private IMongoRepository<Social> _socialRepo;

        public SocialRepositoryTests()
        {
            _socialRepo = new MongoRepository<Social>(_url, CollectionNames.SocialCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var social = new Social()
            {
                LrgImage = "large image",
                Link = "link to social",
                Name = "social name",
                MidImage = "mid image",
                Title = "social title",
                Alt = "F"
            };

            await _socialRepo.Add(social);

            Assert.IsNotNull(social.Id);

            // fetch it back 
            var alreadyAddedSocial = _socialRepo.Where(c => c.Name == "social name").Single();

            Assert.IsNotNull(alreadyAddedSocial);
            Assert.AreEqual(alreadyAddedSocial.LrgImage, social.LrgImage);
            Assert.AreEqual(alreadyAddedSocial.Link, social.Link);
            Assert.AreEqual(alreadyAddedSocial.MidImage, social.MidImage);
            Assert.AreEqual(alreadyAddedSocial.Name, social.Name);
            Assert.AreEqual(alreadyAddedSocial.Title, social.Title);
            Assert.AreEqual(alreadyAddedSocial.Alt, social.Alt);

            var socialNewName = "New name for social";
            alreadyAddedSocial.Name = socialNewName;

            await _socialRepo.Update(alreadyAddedSocial);

            var updatedSocial = await _socialRepo.GetById(alreadyAddedSocial.Id);

            Assert.IsNotNull(updatedSocial);
            Assert.AreEqual(updatedSocial.Name, socialNewName);
        }

        [Test]
        public void BatchTest()
        {
            var socialList = new List<Social>(new Social[] {
                new Social() { Name = "FullName1", Alt = "text1" },
                new Social() { Name = "FullName2", Alt = "text2" },
                new Social() { Name = "FullName3", Alt = "text1" },
                new Social() { Name = "FullName4", Alt = "text2" },
                new Social() { Name = "FullName5", Alt = "text1" },
            });

            //Insert Batch
            _socialRepo.Add(socialList);

            var count = _socialRepo.Count();

            Assert.AreEqual(count, socialList.Count);

            foreach (Social social in socialList)
            {
                Assert.AreNotEqual(new string('0', 24), social.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var social in socialList)
            {
                social.Name = "FullName1" + counter;
                counter++;
            }

            _socialRepo.Update(socialList);

            counter = 1;
            foreach (var message in socialList)
            {
                Assert.AreEqual(message.Name, "FullName1" + counter);
                counter++;
            }

            //Delete by criteria
            _socialRepo.Delete(a => a.Alt.StartsWith("text1"));

            var countAfterDel = _socialRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
