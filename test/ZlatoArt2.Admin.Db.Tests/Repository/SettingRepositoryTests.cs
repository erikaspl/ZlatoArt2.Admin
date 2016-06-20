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
    public class SettingRepositoryTests : MongoRepositoryTests
    {
        private IMongoRepository<Setting> _settingsRepo;

        public SettingRepositoryTests()
        {
            _settingsRepo = new MongoRepository<Setting>(_url, CollectionNames.SettingCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var setting = new Setting()
            {
                ImageLocation = "Image Location",
                Type = 100
            };

            await _settingsRepo.Add(setting);

            Assert.IsNotNull(setting.Id);

            // fetch it back 
            var alreadyAddedSetting = _settingsRepo.Where(c => c.ImageLocation == "Image Location").Single();

            Assert.IsNotNull(alreadyAddedSetting);
            Assert.AreEqual(alreadyAddedSetting.ImageLocation, setting.ImageLocation);
            Assert.AreEqual(alreadyAddedSetting.Type, setting.Type);

            var messageNewName = "New image location";
            alreadyAddedSetting.ImageLocation = messageNewName;

            await _settingsRepo.Update(alreadyAddedSetting);

            var updatedMessage = await _settingsRepo.GetById(alreadyAddedSetting.Id);

            Assert.IsNotNull(updatedMessage);
            Assert.AreEqual(updatedMessage.ImageLocation, messageNewName);
        }

        [Test]
        public void BatchTest()
        {
            var settingList = new List<Setting>(new Setting[] {
                new Setting() { ImageLocation = "ImageLocation1", Type = 1 },
                new Setting() { ImageLocation = "ImageLocation2", Type = 2 },
                new Setting() { ImageLocation = "ImageLocation3", Type = 1 },
                new Setting() { ImageLocation = "ImageLocation4", Type = 2 },
                new Setting() { ImageLocation = "ImageLocation5", Type = 1 },
            });

            //Insert Batch
            _settingsRepo.Add(settingList);

            var count = _settingsRepo.Count();

            Assert.AreEqual(count, settingList.Count);

            foreach (Setting setting in settingList)
            {
                Assert.AreNotEqual(new string('0', 24), setting.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var setting in settingList)
            {
                setting.ImageLocation = "ImageLocation1" + counter;
                counter++;
            }

            _settingsRepo.Update(settingList);

            counter = 1;
            foreach (var setting in settingList)
            {
                Assert.AreEqual(setting.ImageLocation, "ImageLocation1" + counter);
                counter++;
            }

            //Delete by criteria
            _settingsRepo.Delete(a => a.Type.Equals(1));

            var countAfterDel = _settingsRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }

    }
}
