﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using blog_webapi_vue.Models;

using Microsoft.AspNetCore.Mvc;

namespace blog_webapi_vue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// This method is used to fetch all the records
        /// </summary>
        /// <returns>A list of records.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="love">Love instance</param>
        [HttpPost]
        public void Post([FromBody] Love love) { }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) { }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) { }
    }
}