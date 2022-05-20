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
            fitter.UnderFitters = new List<FitterViewModel>();
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
            List<FitterViewModel> fitterList = new List<FitterViewModel>();//Remove the use of lists here in favor of single object?
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7252/Fitter/GetById?Id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    fitterList = JsonConvert.DeserializeObject<List<FitterViewModel>>(apiResponse);
                }
            }
            //Change into DetailedFitterViewModel
            DetailedFitterViewModel detailedFitterViewModel = new DetailedFitterViewModel();
            detailedFitterViewModel.Fitter = fitterList.FirstOrDefault();
            return View(detailedFitterViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
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

        [HttpPost]
        public async Task<IActionResult> Edit(FitterViewModel fitter)
        {
            List<FitterViewModel> fitterList = new List<FitterViewModel>();
            fitter.UnderFitters = new List<FitterViewModel>();
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(fitter), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("https://localhost:7252/Fitter/" + fitter.Id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    fitterList = JsonConvert.DeserializeObject<List<FitterViewModel>>(apiResponse);
                }
            }
            return View(fitterList.FirstOrDefault());
        }



        [HttpPost]
        public async Task<IActionResult> AddUnderFitter([FromBody] DetailedFitterViewModel detailedUnderFitter)
        {
            List<DetailedFitterViewModel> receivedFitter = new List<DetailedFitterViewModel>();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.GetAsync("https://localhost:7252/Fitter/GetById?Id=" + detailedUnderFitter.OverFitterId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    detailedUnderFitter.Fitter = JsonConvert.DeserializeObject<List<FitterViewModel>>(apiResponse).FirstOrDefault();
                    detailedUnderFitter.Fitter.UnderFitters = new List<FitterViewModel>();
                }

                StringContent content = new StringContent(JsonConvert.SerializeObject(detailedUnderFitter), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7252/Fitter/AddUnderFitter", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedFitter = JsonConvert.DeserializeObject<List<DetailedFitterViewModel>>(apiResponse);
                }
            }
            return RedirectToAction("Details", new { id = receivedFitter.FirstOrDefault().OverFitterId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7252/Fitter/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSubordinate(int underfitterID, int overfitterID)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7252/Fitter/" + underfitterID + ", " + overfitterID))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Details", new { id = overfitterID });

        }
    }
}