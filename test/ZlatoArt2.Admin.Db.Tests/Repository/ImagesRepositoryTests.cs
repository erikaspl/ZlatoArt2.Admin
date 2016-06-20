using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Db.Mongo;
using ZlatoArt2.Admin.Db.Mongo.Repositories;
using ZlatoArt2.Admin.Model;
using ZlatoArt2.Admin.Model.Entities;

namespace ZlatoArt2.Admin.Db.Tests.Repository
{
    public class ImagesRepositoryTests : MongoRepositoryTests
    {
        private IMongoRepository<Image> _imageRepo;

        public ImagesRepositoryTests()
        {            
            _imageRepo = new MongoRepository<Image>(_url, CollectionNames.ImageCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var image = new Image()
            {
                Name = "image name",
                Link = "image link"  
            };

            await _imageRepo.Add(image);

            Assert.IsNotNull(image.Id);

            // fetch it back 
            var alreadyAddedImage = _imageRepo.Where(c => c.Name == "image name").Single();

            Assert.IsNotNull(alreadyAddedImage);
            Assert.AreEqual(alreadyAddedImage.Name, image.Name);
            Assert.AreEqual(alreadyAddedImage.Link, image.Link);

            var imageNewName = "New name for image";
            alreadyAddedImage.Name = imageNewName;

            await _imageRepo.Update(alreadyAddedImage);

            var updatedImage = await _imageRepo.GetById(alreadyAddedImage.Id);

            Assert.IsNotNull(updatedImage);
            Assert.AreEqual(updatedImage.Name, imageNewName);
        }

        [Test]
        public void BatchTest()
        {
            var imageList = new List<Image>(new Image[] {
                new Image() { Name = "Image1", Link = "link1" },
                new Image() { Name = "Image2", Link = "link2" },
                new Image() { Name = "Image3", Link = "link1" },
                new Image() { Name = "Image4", Link = "link2" },
                new Image() { Name = "Image5", Link = "link1" },
            });


            //Insert Batch
            _imageRepo.Add(imageList);

            var count = _imageRepo.Count();

            Assert.AreEqual(count, imageList.Count);

            foreach (Image image in imageList)
            {
                Assert.AreNotEqual(new string('0', 24), image.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var image in imageList)
            {
                image.Name = "Image1" + counter;
                counter++;
            }

            _imageRepo.Update(imageList);

            counter = 1;
            foreach (var image in imageList)
            {
                Assert.AreEqual(image.Name, "Image1" + counter);
                counter++;
            }

            //Delete by criteria
            _imageRepo.Delete(a => a.Link.StartsWith("link1"));

            var countAfterDel = _imageRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
