using System;
using System.Collections.Generic;
using System.Text;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public interface IPageViewInterface
    {
        IViewModel ViewModel { get; }
        IPageViewInterface Init();
    }
}
