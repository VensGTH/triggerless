﻿using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Triggerless.Models;
using Triggerless.Services.Server;

namespace Triggerless.API.Controllers
{
    public class RadioController : BaseController
    {

        [Route("api/radio/hours/{hours}"), HttpGet]
        public async Task<HttpResponseMessage> Get(double hours)
        {
            var response = await BootstersDbService.GetSongs(hours);
            return GetResponseFromObject(response);
        }

        [Route("api/radio/post"), HttpPost]

        public async Task<HttpResponseMessage> Post(TriggerlessRadioSong post)
        {
            var response = await BootstersDbService.PostSong(post);
            return GetResponseFromObject(response);

        }

        // Important to note: IIS7 URLs cannot have a plus (+) symbol in the route
        // So don't escape the title, and scrub title of any plusses if they're there.

        [Route("api/radio/tune/{title}"), HttpGet]
        public async Task<HttpResponseMessage> Tune(string title)
        {
            var response = await BootstersDbService.PostSong(new TriggerlessRadioSong { title = title });
            return GetResponseFromObject(response);
        }
    }
}