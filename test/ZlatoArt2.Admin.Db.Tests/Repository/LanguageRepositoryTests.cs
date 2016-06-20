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
    public class LanguageRepositoryTests : MongoRepositoryTests
    {
        private IMongoRepository<Language> _languageRepo;

        public LanguageRepositoryTests()
        {
            _languageRepo = new MongoRepository<Language>(_url, CollectionNames.ImageCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var language = new Language()
            {
                Abbreviation = "language abbreviation",
                Name = "language name",
                EnName = "language en name",
                FlagImage = "flag image link",
                Locale = "en"
            };

            await _languageRepo.Add(language);

            Assert.IsNotNull(language.Id);

            // fetch it back 
            var alreadyAddedLanguage = _languageRepo.Where(c => c.Name == "language name").Single();

            Assert.IsNotNull(alreadyAddedLanguage);
            Assert.AreEqual(alreadyAddedLanguage.Name, language.Name);
            Assert.AreEqual(alreadyAddedLanguage.Abbreviation, language.Abbreviation);
            Assert.AreEqual(alreadyAddedLanguage.EnName, language.EnName);
            Assert.AreEqual(alreadyAddedLanguage.FlagImage, language.FlagImage);
            Assert.AreEqual(alreadyAddedLanguage.Locale, language.Locale);

            var languageNewName = "New name for image";
            alreadyAddedLanguage.Name = languageNewName;

            await _languageRepo.Update(alreadyAddedLanguage);

            var updatedImage = await _languageRepo.GetById(alreadyAddedLanguage.Id);

            Assert.IsNotNull(updatedImage);
            Assert.AreEqual(updatedImage.Name, languageNewName);
        }

        [Test]
        public void BatchTest()
        {
            var languageList = new List<Language>(new Language[] {
                new Language() { Name = "Image1", FlagImage = "link1" },
                new Language() { Name = "Image2", FlagImage = "link2" },
                new Language() { Name = "Image3", FlagImage = "link1" },
                new Language() { Name = "Image4", FlagImage = "link2" },
                new Language() { Name = "Image5", FlagImage = "link1" },
            });


            //Insert Batch
            _languageRepo.Add(languageList);

            var count = _languageRepo.Count();

            Assert.AreEqual(count, languageList.Count);

            foreach (Language language in languageList)
            {
                Assert.AreNotEqual(new string('0', 24), language.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var language in languageList)
            {
                language.Name = "Image1" + counter;
                counter++;
            }

            _languageRepo.Update(languageList);

            counter = 1;
            foreach (var image in languageList)
            {
                Assert.AreEqual(image.Name, "Image1" + counter);
                counter++;
            }

            //Delete by criteria
            _languageRepo.Delete(a => a.FlagImage.StartsWith("link1"));

            var countAfterDel = _languageRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
