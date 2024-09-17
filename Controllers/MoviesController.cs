using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieRentalFrontend.Models;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Text;
namespace MovieRentalFrontend.Controllers
{
    public class MoviesController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7158/";

        public MoviesController (HttpClient client)
        {
            _client = client;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Available movies";

            var response = await _client.GetAsync($"{baseUri}api/Movie/GetMovies"); 
            var json = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(json);

            return View(movieList);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "New Movie";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{baseUri}api/Movie/AddMovie", content);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"{baseUri}api/Movie/GetMovieById/{id}");
            var json = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(json);

            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movie movie)
        {
            var json = JsonConvert.SerializeObject(movie);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _client.PutAsync($"{baseUri}api/Movie/UpdateMovie/{movie.Id}", content);
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{baseUri}api/Movie/DeleteMovie/{id}");
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Search(int? id, string title, string genre)
        {
            List<Movie> movies = new List<Movie>();

            if (id.HasValue)
            {
                var response = await _client.GetAsync($"{baseUri}api/Movie/SearchMovieId/{id.Value}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var movie = JsonConvert.DeserializeObject<Movie>(json);
                    if (movie != null)
                    {
                        movies.Add(movie);
                    }
                }
            }

            else if (!string.IsNullOrWhiteSpace(title))
            {
                var response = await _client.GetAsync($"{baseUri}api/Movie/SearchMovieTitle/{title}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var movie = JsonConvert.DeserializeObject<Movie>(json);
                    if (movie != null)
                    {
                        movies.Add(movie);
                    }
                }
            }

            else if (!string.IsNullOrWhiteSpace(genre))
            {
                var response = await _client.GetAsync($"{baseUri}api/Movie/SearchMovieGenre/{genre}");

                if (response.IsSuccessStatusCode)
                {

                    var json = await response.Content.ReadAsStringAsync();
                    movies = JsonConvert.DeserializeObject<List<Movie>>(json);
                }
            }

            return View(movies);
        }
    }
}
