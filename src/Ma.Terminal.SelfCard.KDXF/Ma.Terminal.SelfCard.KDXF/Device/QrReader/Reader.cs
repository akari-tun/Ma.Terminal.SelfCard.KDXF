using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfCard.KDXF.Device.QrReader
{
    public class Reader : IReader
    {
        public string LastError { get; set; }
        public void Read(Action<bool, IReader, string> callback)
        {
            LastError = string.Empty;
            //这里写操作二维码读头的逻辑，并返回结果
            callback?.Invoke(false, this, "12345678");

        }
    }
}
