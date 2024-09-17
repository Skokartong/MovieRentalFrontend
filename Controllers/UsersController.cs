using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieRentalFrontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace MovieRentalFrontend.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7158/";

        public UsersController(HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Users";

            var response = await _client.GetAsync($"{baseUri}api/User/GetUsers");
            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(json);

            return View(users);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Create User";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{baseUri}api/User/AddUser", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}api/User/GetUserById/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<Rental>(json);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{baseUri}api/User/UpdateUser/{user.Id}", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{baseUri}api/User/DeleteUser/{id}");

            return RedirectToAction("Index");
        }
    }
}
