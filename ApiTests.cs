using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;


namespace nUnitPlaywrightAPI
{
    [TestFixture]
    public class ApiTests
    {
        private IPlaywright _playwright;
        private HttpClientHelper _httpClientHelper;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _httpClientHelper = new HttpClientHelper();
        }

        [Test]
        public async Task GetPosts_ShouldReturn200()
        {
            var response = await _httpClientHelper.MakeRequestAsync("https://jsonplaceholder.typicode.com/posts");
            Assert.That((int)response.StatusCode, Is.EqualTo(200));

            var jsonData = await response.Content.ReadAsStringAsync();
            Assert.That(jsonData, Does.Contain("\"userId\":"));
        }

        [Test]
        public async Task CreatePost_ShouldReturn201()
        {
            var jsonContent = new StringContent("{\"title\":\"foo\", \"body\":\"bar\", \"userId\":1}", System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClientHelper.MakeRequestAsync("https://jsonplaceholder.typicode.com/posts", HttpMethod.Post, jsonContent);

            Assert.That((int)response.StatusCode, Is.EqualTo(201));
            var jsonData = await response.Content.ReadAsStringAsync();
            Assert.That(jsonData, Does.Contain("\"id\":"));
        }

        [Test]
        public async Task UpdatePost_ShouldReturnUpdatedPost()
        {
            var jsonContent = new StringContent(
                "{\"id\":1, \"title\":\"foo\", \"body\":\"bar\", \"userId\":1}",
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClientHelper.MakeRequestAsync("https://jsonplaceholder.typicode.com/posts/1", HttpMethod.Put, jsonContent);
            
            Assert.That((int)response.StatusCode, Is.EqualTo(200));

            var jsonData = await response.Content.ReadAsStringAsync();

            var updatedPost = JsonConvert.DeserializeObject<dynamic>(jsonData);
            var title = updatedPost.title;

            Assert.Multiple(() =>
            {
                Assert.That(title != null ? (string)title : throw new Exception("Title is null"), Is.EqualTo("foo"));
                Assert.That((string)updatedPost.body, Is.EqualTo("bar"));
                Assert.That((int)updatedPost.userId, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task DeletePost_ShouldReturn200()
        {
            var response = await _httpClientHelper.MakeRequestAsync("https://jsonplaceholder.typicode.com/posts/1", HttpMethod.Delete);
            Assert.That((int)response.StatusCode, Is.EqualTo(200));
        }
    }
}
