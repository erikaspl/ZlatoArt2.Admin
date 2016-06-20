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
    public class MessageRepositoryTests : MongoRepositoryTests
    {
        private IMongoRepository<Message> _messageRepo;

        public MessageRepositoryTests()
        {
            _messageRepo = new MongoRepository<Message>(_url, CollectionNames.ImageCollection);
        }

        [Test]
        public async Task AddAndUpdateTest()
        {
            var message = new Message()
            {
                FullName = "full name",
                Email = "full.name@gmail.com",
                Subject = "message subject",
                Text = "message content"
            };

            await _messageRepo.Add(message);

            Assert.IsNotNull(message.Id);

            // fetch it back 
            var alreadyAddedMessage = _messageRepo.Where(c => c.FullName == "full name").Single();

            Assert.IsNotNull(alreadyAddedMessage);
            Assert.AreEqual(alreadyAddedMessage.FullName, message.FullName);
            Assert.AreEqual(alreadyAddedMessage.Subject, message.Subject);
            Assert.AreEqual(alreadyAddedMessage.Email, message.Email);
            Assert.AreEqual(alreadyAddedMessage.Text, message.Text);

            var messageNewName = "New name for message";
            alreadyAddedMessage.FullName = messageNewName;

            await _messageRepo.Update(alreadyAddedMessage);

            var updatedMessage = await _messageRepo.GetById(alreadyAddedMessage.Id);

            Assert.IsNotNull(updatedMessage);
            Assert.AreEqual(updatedMessage.FullName, messageNewName);
        }

        [Test]
        public void BatchTest()
        {
            var messageList = new List<Message>(new Message[] {
                new Message() { FullName = "FullName1", Text = "text1" },
                new Message() { FullName = "FullName2", Text = "text2" },
                new Message() { FullName = "FullName3", Text = "text1" },
                new Message() { FullName = "FullName4", Text = "text2" },
                new Message() { FullName = "FullName5", Text = "text1" },
            });

            //Insert Batch
            _messageRepo.Add(messageList);

            var count = _messageRepo.Count();

            Assert.AreEqual(count, messageList.Count);

            foreach (Message message in messageList)
            {
                Assert.AreNotEqual(new string('0', 24), message.Id);
            }


            //Update Batch
            var counter = 1;
            foreach (var message in messageList)
            {
                message.FullName = "FullName" + counter;
                counter++;
            }

            _messageRepo.Update(messageList);

            counter = 1;
            foreach (var message in messageList)
            {
                Assert.AreEqual(message.FullName, "FullName" + counter);
                counter++;
            }

            //Delete by criteria
            _messageRepo.Delete(a => a.Text.StartsWith("text1"));

            var countAfterDel = _messageRepo.Count();
            Assert.AreEqual(countAfterDel, 2);
        }
    }
}
