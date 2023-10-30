using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfCard.KDXF.WebApi.Entities
{
    public class UserInfo
    {
        /*
            {
            "userid": "11", 
            "phone": "18912345678", 
            "username": "张三", 
            "classname": "国家局党校进修班", 
            "branchinfo": "第一支部第二小组", 
            "roominfo": "一号住宿楼1101", 
            "roomexpiration": "1694999802000", 
            "roomaddrnumber": "01010100100" 
            }
        */
        public string Userid { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Classname { get; set; }
        public string Branchinfo { get; set; }
        public string Roominfo { get; set; }
        public string Roomexpiration { get; set; }
        public string Roomaddrnumber { get; set; }
    }
}
