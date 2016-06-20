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
    public class SliderRepositoryTests : MongoRepositoryTests
    {
        private IMongoRepository<Slider> _sliderRepo;

        public SliderRepositoryTests()
        {
            _sliderRepo = new MongoRepository<Slider>(_url, CollectionNames.SliderCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var slider = new Slider()
            {
                ImageSrc = "image source",
                ImageAlt = "image alt",
                DisplayOrder = 10,
                IsActive = true
            };

            await _sliderRepo.Add(slider);

            Assert.IsNotNull(slider.Id);

            // fetch it back 
            var alreadyAddedSlider = _sliderRepo.Where(c => c.ImageSrc == "image source").Single();

            Assert.IsNotNull(alreadyAddedSlider);
            Assert.AreEqual(alreadyAddedSlider.ImageSrc, slider.ImageSrc);
            Assert.AreEqual(alreadyAddedSlider.ImageAlt, slider.ImageAlt);
            Assert.AreEqual(alreadyAddedSlider.DisplayOrder, slider.DisplayOrder);
            Assert.AreEqual(alreadyAddedSlider.IsActive, slider.IsActive);

            var ImageSrcNewName = "New source for image";
            alreadyAddedSlider.ImageSrc = ImageSrcNewName;

            await _sliderRepo.Update(alreadyAddedSlider);

            var updatedMessage = await _sliderRepo.GetById(alreadyAddedSlider.Id);

            Assert.IsNotNull(updatedMessage);
            Assert.AreEqual(updatedMessage.ImageSrc, ImageSrcNewName);
        }

        [Test]
        public void BatchTest()
        {
            var sliderList = new List<Slider>(new Slider[] {
                new Slider() { ImageSrc = "ImageSrc1", IsActive = true },
                new Slider() { ImageSrc = "ImageSrc2", IsActive = false },
                new Slider() { ImageSrc = "ImageSrc3", IsActive = true },
                new Slider() { ImageSrc = "ImageSrc4", IsActive = false },
                new Slider() { ImageSrc = "ImageSrc5", IsActive = true },
            });

            //Insert Batch
            _sliderRepo.Add(sliderList);

            var count = _sliderRepo.Count();

            Assert.AreEqual(count, sliderList.Count);

            foreach (Slider slider in sliderList)
            {
                Assert.AreNotEqual(new string('0', 24), slider.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var slider in sliderList)
            {
                slider.ImageSrc = "ImageSrc1" + counter;
                counter++;
            }

            _sliderRepo.Update(sliderList);

            counter = 1;
            foreach (var slider in sliderList)
            {
                Assert.AreEqual(slider.ImageSrc, "ImageSrc1" + counter);
                counter++;
            }

            //Delete by criteria
            _sliderRepo.Delete(a => a.IsActive.Equals(true));

            var countAfterDel = _sliderRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
