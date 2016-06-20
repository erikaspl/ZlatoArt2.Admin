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
    public class LabelReposiotryTests : MongoRepositoryTests
    {
        private IMongoRepository<Label> _labelRepo;

        public LabelReposiotryTests()
        {            
            _labelRepo = new MongoRepository<Label>(_url, CollectionNames.LabelCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var label = new Label()
            {
                LabelId = 100,
                LabelName = "label name",
                Text = "label text",
                LanguageId = 12
            };

            await _labelRepo.Add(label);

            Assert.IsNotNull(label.Id);

            // fetch it back 
            var alreadyAddedLabel = _labelRepo.Where(c => c.LabelName == "label name").Single();

            Assert.IsNotNull(alreadyAddedLabel);
            Assert.AreEqual(alreadyAddedLabel.LabelId, label.LabelId);
            Assert.AreEqual(alreadyAddedLabel.Text, label.Text);
            Assert.AreEqual(alreadyAddedLabel.LanguageId, label.LanguageId);

            var labelNewName = "New name for image";
            alreadyAddedLabel.LabelName = labelNewName;

            await _labelRepo.Update(alreadyAddedLabel);

            var updatedImage = await _labelRepo.GetById(alreadyAddedLabel.Id);

            Assert.IsNotNull(updatedImage);
            Assert.AreEqual(updatedImage.LabelName, labelNewName);
        }

        [Test]
        public void BatchTest()
        {
            var labelList = new List<Label>(new Label[] {
                new Label() { LabelName = "Label1", Text = "text1" },
                new Label() { LabelName = "Label2", Text = "text2" },
                new Label() { LabelName = "Label3", Text = "text1" },
                new Label() { LabelName = "Label4", Text = "text2" },
                new Label() { LabelName = "Label5", Text = "text1" },
            });


            //Insert Batch
            _labelRepo.Add(labelList);

            var count = _labelRepo.Count();

            Assert.AreEqual(count, labelList.Count);

            foreach (Label label in labelList)
            {
                Assert.AreNotEqual(new string('0', 24), label.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var label in labelList)
            {
                label.LabelName = "Label1" + counter;
                counter++;
            }

            _labelRepo.Update(labelList);

            counter = 1;
            foreach (var label in labelList)
            {
                Assert.AreEqual(label.LabelName, "Label1" + counter);
                counter++;
            }

            //Delete by criteria
            _labelRepo.Delete(a => a.Text.StartsWith("text1"));

            var countAfterDel = _labelRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
