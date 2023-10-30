using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfCard.KDXF.Device.CardReader
{
    public class Reader : IReader
    {
        public string LastError { get; set; }
        public void Read(Action<bool, IReader, string> callback)
        {
            LastError = string.Empty;
            //这里写操作发卡器读卡的逻辑，并返回结果
            string uuid = "100000001";

            callback?.Invoke(true, this, uuid);
        }

        public bool Issue(string uuid)
        {
            LastError = string.Empty;
            return true;
        }
    }
}
