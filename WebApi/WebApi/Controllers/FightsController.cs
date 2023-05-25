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
        
        // GET api/fights
        //[System.Web.Http.Route("api/fights/all")]
        //public IEnumerable<Fight> Get()
        //{
        //    return FightsRepository.GetFights();
        //}

        // GET api/fights/
        //[System.Web.Http.Route("api/fights/{id}")]
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

    }
}