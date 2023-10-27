using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public abstract class ViewModelBase : ObservableObject, IViewModel
    {
        public bool IsRunning { get; set; }

        public ViewModelBase()
        {

        }

        public abstract void Initialization();

        public string GetString(string key) => ResourceManager.Instance.GetString(key);

    }
}
