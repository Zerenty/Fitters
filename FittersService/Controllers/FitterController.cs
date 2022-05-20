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
        public IEnumerable<Fitter> Post([FromBody] Fitter obj)
        {
            if (ModelState.IsValid == true)
            {
                int insertedFitterID = db.Add(obj);
                if(insertedFitterID != 0)
                {

                    var result = db.GetFitters().Where(model => model.ID == insertedFitterID);

                    return result;
                }
                else
                {
                    //Error handling
                    return null;
                }
            }
            else
            {
                //Error handling
                return null;
            }
        }


        [HttpPut("{id}")]
        public IEnumerable<Fitter> Put(int id, [FromBody] Fitter obj)
        {
            if (ModelState.IsValid == true)
            {
                if (db.Edit(id, obj))
                {

                    var result = db.GetFitters().Where(model => model.ID == obj.ID);

                    return result;
                }
                else
                {
                    //Error handling
                    return null;
                }
            }
            else
            {
                //Error handling
                return null;
            }
        }


        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            if (ModelState.IsValid == true)
            {
                db.DeleteFitter(id);
                return true;
            }
            return false;
        }

        [HttpDelete("{underfitterID}, {overfitterID}")]
        public bool RemoveUnderFitter(int underfitterID, int overfitterID)
        {
            if (ModelState.IsValid == true)
            {
                db.DeleteUnderFitter(underfitterID, overfitterID);
                return true;
            }
            return false;
        }

        [HttpPost(nameof(AddUnderFitter))]
        public IEnumerable<AddUnderFitterDTO> AddUnderFitter([FromBody] AddUnderFitterDTO obj)
        {
            //List to return
            List<AddUnderFitterDTO> result = new List<AddUnderFitterDTO>();
            //Retrieve over fitter
            obj.Fitter = GetById(obj.OverFitterId).FirstOrDefault();

            Fitter newFitter = new Fitter();
            newFitter.FullName = obj.FullName;
            newFitter.PhoneNumber = obj.PhoneNumber;
            newFitter.FitterType = obj.FitterType;

            newFitter.ID = Post(newFitter).FirstOrDefault().ID; //Error handling

            if (newFitter.ID != 0) //Better error handling
            {
                //Return something
                db.AddUnderFitter(obj.OverFitterId, newFitter.ID);
            }

            result.Add(obj);
            return result;
        }
    }
}