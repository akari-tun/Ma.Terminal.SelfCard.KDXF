using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Ma.Terminal.SelfCard.KDXF.WebApi.Entities
{
    public class ApiRespone<T>
    {
        /// <summary>
        /// 0 表示成功
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg { get; set; }
        public T Data { get; set; }
    }
}
