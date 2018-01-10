using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Web;
using System.IO;
using WebApi.Adapter;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            DataAdapter da = new DataAdapter();
            var result = da.Get(1);
            var jsonString = JsonConvert.SerializeObject(result);
            return jsonString;
        }

        // GET api/values
        [HttpPost]
        [Route("Insert")]
        public int Insert()
        {
            DataAdapter da = new DataAdapter();
            var result = da.Insert("A","CVC",132,"U206d25c2ea6bd87c17655609a1c37cb8");
            return result;
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

        [HttpPost]
        [Route("TestPost")]
        public IActionResult TestPost()
        {
            string ChannelAccessToken = "MN840PMoy1tFbzfSVyoF51rG8vujtBRPHNf3m6r6JcPhJ2gicgwJD2gxM3vDpTK1oys8DElYuy29lx76Xl6/+hQHdjbX2tGtsW5p0dK14UP7A2vL24rIibhvqHiI3fgD6Vmixh7LYMCwPPiWjLBTowdB04t89/1O/w1cDnyilFU=";
            string postData = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    postData = reader.ReadToEnd();
                }
                //取得 http Post RawData(should be JSON)
                //postData = Request.Body.ReadAsync;//Request.Content.ReadAsStringAsync().Result;
                //剖析JSON
                var ReceivedMessage = isRock.LineBot.Utility.Parsing(postData);
                //回覆訊息
                string Message;
                Message = ReceivedMessage.events[0].message.text;
                int value = 0;
                var result = string.Empty;
                if (int.TryParse(Message, out value))
                {
                    DataAdapter da = new DataAdapter();
                    result = da.Get(value).FirstOrDefault().Column1;
                }
                else
                {
                    result = "請輸入數字好ㄇ!!";
                }

                //回覆用戶
                isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, result, ChannelAccessToken);
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                var test = ex;
                return Ok();
            }
        }
    }
}
