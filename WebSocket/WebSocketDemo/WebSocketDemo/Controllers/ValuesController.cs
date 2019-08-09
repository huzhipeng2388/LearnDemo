using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace WebSocketDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async void GetAsync(int id)
        {
            try {
                await WebSocketXX.SendAsyncAll(new { Message = "成功！" });
            }
            catch (Exception e) {

            }
            
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] string value)
        {
            await WebSocketXX.SendAsyncAll(new {Message = "成功！" });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
