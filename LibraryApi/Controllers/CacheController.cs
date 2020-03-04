using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class CacheController : Controller
    {
        IDistributedCache Cache;
        public CacheController(IDistributedCache cache)
        {
            Cache = cache;
        }

        [HttpGet("/time")]
        [ResponseCache(Duration = 130, Location = ResponseCacheLocation.Any)]
        public ActionResult<string> GetTime()
        {
            return Ok(new { data = $"The time is {DateTime.Now.ToLongTimeString()}" });
        }

        [HttpGet("/time2")]
        public async Task<ActionResult> GetTheTimeFromDistributedCache()
        {
            var time = await Cache.GetAsync("time");
            string newTime = null;
            if(time == null)
            {
                newTime = DateTime.Now.ToLongTimeString();
                var encodedTime = Encoding.UTF8.GetBytes(newTime);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                await Cache.SetAsync("time", encodedTime, options);
            }
            else
            {
                newTime = Encoding.UTF8.GetString(time);
            }
            return Ok($"Ok, it is now {newTime}");
        }


    }
}
