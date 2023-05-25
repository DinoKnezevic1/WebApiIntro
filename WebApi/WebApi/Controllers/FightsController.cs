using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class FightsController : ApiController
    {
        public IHttpActionResult Get(int id)
        {
            Fight fight = FightsRepository.GetFight(id);
            if (fight != null)
            {
                return Ok(fight);
            }
            else
            {
                return NotFound();
            }
        }
        //[System.Web.Http.Route("api/fights/getall")]
        public IHttpActionResult Get(bool includeAddress = false)
        {
            IEnumerable<Fight> fights = null;

            fights = FightsRepository.GetFights();

            if (fights.Count() != 0)
            {
                return Ok(fights);
            }
            return NotFound();
        }

        public IHttpActionResult PostNewFight(Fight fight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            FightsRepository.AddFight(fight);
            return Ok();
        }
        //I used postman to get a fight with id 1 and then copied that response and edited it in body for post request and then tried get for that new fight

        public IHttpActionResult PutFight(int id, Fight fight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }
            Fight currentFight = FightsRepository.GetFight(id);
            if (currentFight != null) { 
            bool updateStatus = FightsRepository.UpdateFight(fight);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        public IHttpActionResult DeleteFight(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid id");
            }
            FightsRepository.DeleteFight(id);
            return Ok();
        }

    }
}