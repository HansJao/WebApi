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
        [Route("LinebotAccess")]
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
                        var count = SearchArea(Message.Substring(1));
                        result = "查詢" + count;
                        break;
                    case "+":
                        var splitMessage = Message.Split(' ');
                        if (splitMessage.Count() < 4)
                        {
                            result = "少了某些參數";
                        }
                        else
                        {
                            Insert(splitMessage[0].Substring(1), splitMessage[1], Convert.ToInt32(splitMessage[2]), splitMessage[3]);
                            result = "新增成功";
                        }
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
                return Ok(ex);
            }
        }


        private void Insert(string area, string name, int quantity, string userName)
        {

            DataAdapter da = new DataAdapter();
            var result = da.Insert(area, name, quantity, userName);
            //return result;
        }

        private int SearchArea(string area)
        {

            DataAdapter da = new DataAdapter();
            var result = da.SearchArea(area);
            return result;
        }
    }
}