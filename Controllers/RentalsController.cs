using Microsoft.AspNetCore.Mvc;
using MovieRentalFrontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace MovieRentalFrontend.Controllers
{
    public class RentalsController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7158/";

        public RentalsController(HttpClient client)
        {
            _client = client;
        }
        public async Task <IActionResult> Index()
        {
            ViewData["Title"] = "Current rentals";

            var response = await _client.GetAsync($"{baseUri}api/Rental/GetRentals");
            var json = await response.Content.ReadAsStringAsync();
            var rentals = JsonConvert.DeserializeObject<List<Rental>>(json);

            return View(rentals);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Rent a movie";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rental rental, int movieId)
        {
            var json = JsonConvert.SerializeObject(rental);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{baseUri}api/Rental/RentMovie/{movieId}", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}api/Rental/GetRentalById/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var rental = JsonConvert.DeserializeObject<Rental>(json);

            return View(rental);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Rental rental)
        {
            var json = JsonConvert.SerializeObject(rental);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{baseUri}api/Rental/UpdateRental/{rental.Id}", content);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{baseUri}api/Rental/DeleteRental/{id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Return(int id)
        {
            var response = await _client.GetAsync($"{baseUri}api/Rental/ReturnMovie/{id}");

            return RedirectToAction("Index");
        }
    }
}
