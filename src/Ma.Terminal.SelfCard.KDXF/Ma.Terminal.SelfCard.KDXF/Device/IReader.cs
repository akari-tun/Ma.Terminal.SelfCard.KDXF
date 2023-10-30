using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfCard.KDXF.Device
{
    public interface IReader
    {
        string LastError { get; set; }
        void Read(Action<bool, IReader, string> callback);
    }
}
