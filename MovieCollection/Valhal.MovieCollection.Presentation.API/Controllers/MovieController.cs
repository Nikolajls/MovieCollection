using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Valhal.MovieCollection.DTO;

namespace Valhal.MovieCollection.Presentation.API.Controllers
{
    public class MovieController : ApiController
    {

        [ResponseType(typeof(IEnumerable<MovieGet>))]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var res = new List<MovieGet>() { new MovieGet() { Title = "TEST" } };
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
    }
}