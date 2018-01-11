using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebApi.Adapter;
using WebApi.DataClass;

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
                Message = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(ReceivedMessage.events[0].message.text));
                var switchFunction = Message.Substring(0, 1);
                var result = string.Empty;
                switch (switchFunction)
                {
                    case "!":
                        result = "修改";
                        break;
                    case "?":
                        var reply = SearchArea(Message.Substring(1));
                        result = "查詢結果:\n" + reply;
                        break;
                    case "+":
                        var splitMessage = Message.Split(' ');
                        if (splitMessage.Count() < 3)
                        {
                            result = "少了某些參數";
                        }
                        else
                        {
                            Insert(splitMessage[0].Substring(1), splitMessage[1], Convert.ToInt32(splitMessage[2]), ReceivedMessage.events[0].source.userId);
                            result = "新增成功";
                        }
                        break;
                    case "-":
                        result = "刪除";
                        break;
                    default:
                        result = "指令錯誤!! \n'+':新增 \n'-':刪除 \n'?':查詢 \n'!':修改";
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

        private string SearchArea(string area)
        {

            DataAdapter da = new DataAdapter();
            var result = da.SearchArea(area);
            var replyMessage = string.Empty;
            foreach (var textile in result)
            {
                replyMessage += string.Concat("地點:", textile.Area, "\n",
                                             "名稱:", textile.Name, "\n",
                                             "數量:", textile.Quantity, "\n",
                                             "更新時間:", textile.ModifyDate.ToString("yyyy/MM/dd hh:mm:ss"), "\n",
                                             "修改人員:", textile.ModifyUser, "\n", "---------------------", "\n");
            }
            return replyMessage;
        }
    }
}