using FittersService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.OData;

namespace FittersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FitterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<FitterController> _logger;

        public FitterController(ILogger<FitterController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        DbContext db = new DbContext();

        [EnableQuery]
        [HttpGet]
        public IEnumerable<Fitter> Get()
        {
            return db.GetFitters().ToList();
        }

        [HttpGet(nameof(GetById))]
        public IEnumerable<Fitter> GetById(int Id)
        {
            var result = db.GetFitters().Where(model => model.ID == Id);

            return result;
        }
        [HttpPost]
        public void Post([FromBody] Fitter obj)
        {
            if (ModelState.IsValid == true)
            {
                db.Add(obj);
            }
            else
            {

            }
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Fitter obj)
        {
            if (ModelState.IsValid == true)
            {
                db.Edit(id, obj);
            }
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (ModelState.IsValid == true)
            {
                db.DeleteFitter(id);
            }
        }
    }
}