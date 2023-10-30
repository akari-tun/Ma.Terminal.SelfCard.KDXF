using Ma.Terminal.SelfCard.KDXF.Model;
using Ma.Terminal.SelfCard.KDXF.WebApi.Entities;
using Ma.Terminal.SelfCard.KDXF.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ma.Terminal.SelfCard.KDXF.WebApi
{
    public class Requester
    {
        const string USER_INFO = "/checkin/userinfo";
        const string USER_INFO_FOR_QR = "/checkin/userinfoForQr";
        const string CARD_NOTIFY = "/checkin/cardNotify";
        const string CARD_LOST = "/checkin/cardLoss";
        const string DEVICE_REPORT = "/checkin/deviceReport";

        private Machine _machine;
        private JsonSerializerOptions _options;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private SM2Crypto _sm2;

        public string LastMessage { get; set; }
        public int LastCode { get; set; }

        public Requester(Machine machine)
        {
            _machine = machine;
            _options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _sm2 = new SM2Crypto(machine.Key, string.Empty, SM2Crypto.Mode.C1C3C2, true);
        }

        public async Task<UserInfo> GetUserInfo(object data)
        {
            #region TestCode
#if Debug
            //LastCode = 0;
            //return new UserInfo()
            //{
            //    Userid = "11",
            //    Phone = "18912345678",
            //    Username = "张三",
            //    Classname = "国家局党校进修班",
            //    Branchinfo = "第一支部第二小组",
            //    Roominfo = "一号住宿楼1101",
            //    Roomexpiration = "1694999802000",
            //    Roomaddrnumber = "01010100100"
            //};
#endif
            #endregion

            var url = $"{_machine.ApiUrl}{USER_INFO}";
            return await Post<UserInfo, object>(data, url);
        }

        public async Task<UserInfo> GetUserInfoForQr(object data)
        {
            var url = $"{_machine.ApiUrl}{USER_INFO_FOR_QR}";
            return await Post<UserInfo, object>(data, url);
        }

        public async Task<string> NotifyCard(object data)
        {
            var url = $"{_machine.ApiUrl}{CARD_NOTIFY}";
            return await Post<string, object>(data, url);
        }

        public async Task<string> LostCard(object data)
        {
            var url = $"{_machine.ApiUrl}{CARD_LOST}";
            return await Post<string, object>(data, url);
        }

        public async Task<string> ReportDevice(object data)
        {
            var url = $"{_machine.ApiUrl}{DEVICE_REPORT}";
            return await Post<string, object>(data, url);
        }

        private async Task<T> Post<T,Q>(Q data, string url)
        {
            LastCode = -1;
            LastMessage = string.Empty;

            string content = Newtonsoft.Json.JsonConvert.SerializeObject(GetContent(data));
            _logger.Trace($"Request {url} -> {content}");

            var respone = await HttpUtility.HttpPostResponseAsync(
                url,
                content,
                10000,
                Encoding.UTF8,
                "application/json");

            if (respone.IsSuccessStatusCode)
            {
                var resp = await respone.Content.ReadAsStringAsync();

                _logger.Trace($"Response {url} -> {resp}");

                var entity = JsonSerializer.Deserialize<ApiRespone<T>>(resp, _options);
                if (entity != null)
                {
                    LastCode = entity.Code;
                    LastMessage = entity.Msg;
                    if (entity.Code == 0) return entity.Data;
                }
                else
                {
                    LastMessage = $"后台接口返回数据解析失败。";
                }
            }
            else
            {
                _logger.Trace($"Response {url} -> StatusCode:{respone.StatusCode}");
                LastMessage = $"后台服务请求失败，返回码：[{respone.StatusCode}]";
            }

            return default(T);
        }

        private ApiRequest GetContent(object data)
        {
            var dataJson = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            var dataBytes = Encoding.UTF8.GetBytes(dataJson);
            var encData = _sm2.Encrypt(dataBytes);

            var request = new ApiRequest()
            {
                Data = Convert.ToBase64String(encData),
                Appid = "vC5rNcpegZmYBG41"
            };

            return request;
        }
    }
}
