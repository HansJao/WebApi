using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Adapter;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LineRobotController : Controller
    {
        [HttpPost]
        public IActionResult LinebotAccess()
        {
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
                var switchFunction = Message.Substring(0, 1);
                var result = string.Empty;
                switch (switchFunction)
                {
                    case "!":
                        result = "修改";
                        break;
                    case "?":
                        result = "查詢";
                        break;
                    case "+":
                        result = "新增";
                        break;
                    case "-":
                        result = "刪除";
                        break;
                    default:
                        result = "指令錯誤!! '+':新增 ,'-':刪除 ,'?':查詢 ,'!':修改";
                        break;
                }
                // int value = 0;

                // if (int.TryParse(Message, out value))
                // {
                //     DataAdapter da = new DataAdapter();
                //     result = da.Get(value).FirstOrDefault().Column1;
                // }
                // else
                // {
                //     result = "請輸入數字好ㄇ!!";
                // }

                //回覆用戶
                isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, result, ConfigProvider.ChannelAccessToken);
                //回覆API OK
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}