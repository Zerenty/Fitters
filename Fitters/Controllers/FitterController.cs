using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fitters.Models;
using Newtonsoft.Json;
using System.Text;

namespace Fitters.Controllers
{
    public class FitterController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<FitterViewModel> fitterList = new List<FitterViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7252/Fitter"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    fitterList = JsonConvert.DeserializeObject<List<FitterViewModel>>(apiResponse);
                }
            }
            return View(fitterList);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(FitterViewModel fitter)
        {
            FitterViewModel receivedFitter = new FitterViewModel();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(fitter), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7252/Fitter", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedFitter = JsonConvert.DeserializeObject<FitterViewModel>(apiResponse);
                }
            }
            return View(receivedFitter);
        }

        public async Task<IActionResult> Details(int id)
        {

            //Retrieve Fitter by ID
            List<FitterViewModel> fitterList = new List<FitterViewModel>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7252/Fitter/GetById?Id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    fitterList = JsonConvert.DeserializeObject<List<FitterViewModel>>(apiResponse);
                }
            }

            return View(fitterList.FirstOrDefault());
        }
    }
}
