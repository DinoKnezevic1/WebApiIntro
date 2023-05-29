using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class FightsController : ApiController
    {

        public HttpResponseMessage Get()
        {
            IEnumerable<Fight> fights = null;

            fights = FightsRepository.GetFights();

            if (fights.Count() != 0)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK,fights);
            }
            return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }
        public HttpResponseMessage Get(int id)
        {
            Fight fight = FightsRepository.GetFight(id);
            if (fight != null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, fight);
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }
        }

        public HttpResponseMessage PostNewFight(Fight fight)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest,"Invalid data");
            }
            FightsRepository.AddFight(fight);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Successfully posted a new fight.");
        }

        //I used postman to get a fight with id 1 and then copied that response and edited it in body for post request and then tried get for that new fight
        //public HttpResponseMessage PostNewFight([FromBody] Fight fight)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Invalid data");
        //    }
        //    FightsRepository.AddFight(fight);
        //    return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Successfully posted a new fight.");
        //}

        public HttpResponseMessage PutFight(int id, Fight fight)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Invalid data");
            }
            Fight currentFight = FightsRepository.GetFight(id);
            if (currentFight != null) { 
            bool updateStatus = FightsRepository.UpdateFight(fight);
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Successfully updated.");
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }
        }

        public HttpResponseMessage DeleteFight(int id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "Invalid fight id");
            }
            FightsRepository.DeleteFight(id);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, "Successfully deleted.");
        }

    }
}