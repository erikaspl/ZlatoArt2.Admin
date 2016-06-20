using Microsoft.AspNet.TestHost;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class LanguageControllerTests : ControllerTests
    {
        private MongoRepository<Language> _languageRepo;
        private List<Language> _languageList;
        public LanguageControllerTests()
        {
            _languageRepo = new MongoRepository<Language>(_url, CollectionNames.LanguageCollection);
        }

        [SetUp]
        public override void Init()
        {
            base.Init();
            _languageList = new List<Language>(new Language[] {
                new Language() { EnName = "English", Locale = "en", Name = "English", Abbreviation = "en", FlagImage = "Flag image en"},                
                new Language() { EnName = "Russian", Locale = "ru", Name = "Русский", Abbreviation = "ru", FlagImage = "Flag image ru"},                
                new Language() { EnName = "Lithuanian", Locale = "lt", Name = "Lituvių", Abbreviation = "lt", FlagImage = "Flag image lt"}             
            });

            //Insert Batch
            _languageRepo.Add(_languageList);
        }

        [TestFixture]
        public class GetTests : LanguageControllerTests
        {
            [Test, UseDatabase]
            public void GetAllTest()
            {
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var response = client.GetAsync("api/language");

                    var list = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Language>>().Result;

                    Assert.AreEqual(list.Count(), _languageList.Count());

                    Language[] array = list.ToArray();

                    var index = 0;
                    foreach (var artist in _languageList)
                    {
                        var saved = (Language)array.GetValue(index);
                        Assert.IsNotNull(saved.Id);
                        Assert.AreNotEqual(new string('0', 24), saved.Id);
                        Assert.AreEqual(saved.FlagImage, artist.FlagImage);
                        Assert.AreEqual(saved.Locale, artist.Locale);
                        Assert.AreEqual(saved.EnName, artist.EnName);
                        Assert.AreEqual(saved.Abbreviation, artist.Abbreviation);
                        Assert.AreEqual(saved.Name, artist.Name);
                        index++;
                    }
                }
            }
        }

        [TestFixture]
        public class PostTests : LanguageControllerTests
        {
            [Test, UseDatabase]
            public void PostNewLanguageTest()
            {
                using (var client = HttpSelfHost.GetServer().CreateClient())
                {
                    var newLang = new Language()
                    {
                        Id = ObjectId.GenerateNewId().ToString(), 
                        Background = "bkg",
                        Abbreviation = "abbr1",
                        EnName = "enName1",
                        FlagImage = "flagImage1",
                        Locale = "locale1",
                        Name = "name1",
                        Icon = "icon"
                    };
                    var requestMessage = HttpSelfHost.CreateHttpRequestMessage<Language>(newLang);

                    var x = client.PutAsync("api/language", requestMessage).Result;
                    

                    var response = client.GetAsync("api/language");
                    var list = ((StreamContent)(response.Result.Content)).ReadAsAsync<List<Language>>().Result;

                    var actual = list.First(l => l.Locale == newLang.Locale);

                    Assert.AreEqual(actual.Abbreviation, newLang.Abbreviation);
                    Assert.AreEqual(actual.EnName, newLang.EnName);
                    Assert.AreEqual(actual.FlagImage, newLang.FlagImage);
                    Assert.AreEqual(actual.Locale, newLang.Locale);
                    Assert.AreEqual(actual.Name, newLang.Name);
                }
            }
        }
    }
}
