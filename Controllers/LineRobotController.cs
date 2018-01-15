using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Adapter;
using WebApi.DataClass;
using WebApi.DataClass.Enumeration;

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
                var switchFunction = Message.Split(' ');
                var result = string.Empty;
                switch (ConvertToNarrow(switchFunction.FirstOrDefault()).ToLower())
                {
                    case "!":
                        result = "修改";
                        break;
                    case "?倉庫":
                        var areaName = switchFunction[1];
                        var areaResult = SearchArea(areaName);
                        result = "查詢結果:\n" + areaResult;
                        break;
                    case "?名稱":
                        var Namename = switchFunction[1];
                        var nameResult = SearchName(Namename);
                        result = "查詢結果:\n" + nameResult;
                        break;
                    case "+":
                        if (switchFunction.Count() != 7)
                        {
                            result = "輸入的參數錯誤!!";
                        }
                        else
                        {
                            var area = switchFunction[1];
                            var name = switchFunction[2];
                            var color = switchFunction[3];
                            var position = switchFunction[4];
                            var quantity = switchFunction[5];
                            var memo = switchFunction[6];
                            var userName = GetUserName(ReceivedMessage.events[0].source.userId);
                            var successInsert = Insert(area, name, color, position, Convert.ToInt32(quantity), userName, memo);
                            if (successInsert)
                                result = "新增成功";
                            else
                                result = "新增失敗";
                        }
                        break;
                    case "-":
                        var id = Convert.ToInt32(switchFunction[1]);
                        var successDelete = DeleteById(id);
                        if (successDelete)
                            result = "刪除成功";
                        else
                            result = "刪除失敗";
                        break;
                    case "help":
                        result = @"======查詢指令======\n?倉庫 [倉庫名稱] \n?名稱 [布種名稱] \n======新增指令======\n+ [倉庫名稱] [布種名稱] [顏色] [儲位] [數量] [備註] \n======修改指令======\n! [顆顆,還沒做]\n======刪除指令======\n- [編號]";
                        break;
                    default:
                        return Ok();
                }
                if (!ConfigProvider.IsDevelopment)
                {
                    //回覆用戶
                    isRock.LineBot.Utility.ReplyMessage(ReceivedMessage.events[0].replyToken, result, ConfigProvider.ChannelAccessToken);
                    return Ok();
                }
                else
                {
                    //回覆API OK
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        private string ConvertToNarrow(string text)
        {
            var narrow = string.Empty;
            narrow = text.Replace('？', '?').Replace('＋', '+').Replace('－', '-').Replace('！', '!');
            return narrow;
        }
        private string GetUserName(string userID)
        {
            if (ConfigProvider.UserIDList.Count() == 0 || ConfigProvider.UserIDList.Where(w => w.Value == userID).Count() == 0)
            {
                return userID;
            }
            var user = ConfigProvider.UserIDList.Where(w => w.Value == userID).FirstOrDefault();
            return user.Key;
            // switch (userID)
            // {
            //     case UserEnum.UserName.Hans:
            //         return "晟瀚";
            //     case UserEnum.UserName.Syuan:
            //         return "韻琁";
            //     default:
            //         return userID;
            // }
        }


        private bool Insert(string area, string name, string color, string position, int quantity, string userName, string memo)
        {

            DataAdapter da = new DataAdapter();
            var result = da.Insert(area, name, color, position, quantity, userName, memo);
            return result == 1;
        }

        private bool DeleteById(int id)
        {
            DataAdapter da = new DataAdapter();
            var result = da.DelectByID(id);
            return result == 1;
        }

        private string SearchArea(string area)
        {
            DataAdapter da = new DataAdapter();
            var result = da.SearchArea(area);
            var replyMessage = GetReplyMessage(result);
            return replyMessage;
        }

        private string GetReplyMessage(IEnumerable<TextileStore> textileStoreList)
        {
            var replyMessage = string.Empty;
            foreach (var textile in textileStoreList)
            {
                replyMessage += string.Concat("編號:", textile.ID, "\n",
                                              "地點:", textile.Area, "\n",
                                              "名稱:", textile.Name, "\n",
                                              "顏色:", textile.Color, "\n",
                                              "儲位:", textile.Position, "\n",
                                              "數量:", textile.Quantity, "\n",
                                              "備註:", textile.Memo, "\n",
                                              "更新時間:", textile.ModifyDate.ToString("yyyy/MM/dd HH:mm:ss"), "\n",
                                              "修改人員:", textile.ModifyUser, "\n", "---------------------", "\n");
            }
            return replyMessage;
        }

        private string SearchName(string name)
        {
            DataAdapter da = new DataAdapter();
            var result = da.SearchName(name);
            var replyMessage = GetReplyMessage(result);
            return replyMessage;
        }
    }
}