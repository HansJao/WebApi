﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
        //   isRock.LineBot.Utility.PushMessage(
        //       "Udb332b1f0570d0d9258c5961a6300554", "要傳送的訊息testtest", "xW4rg6ixb5g3CGkPS1460f+BojsYHKIe9xtFmuV3ZwDdUR2JKVY+md4mmXt6n0nvoys8DElYuy29lx76Xl6/+hQHdjbX2tGtsW5p0dK14UORQ0PGJOGSF/8SPwGkUafmGHXkQkYjmDfHqQv6sXsN/AdB04t89/1O/w1cDnyilFU=");  
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
