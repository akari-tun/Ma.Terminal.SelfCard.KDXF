using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public abstract class ViewModelBase : ObservableObject, IViewModel
    {
        private Machine _machine;

        string _time;
        public string Time
        {
            get => _time;
            set
            {
                SetProperty(ref _time, value);
            }
        }

        string _machineNo;
        public string MachineNo
        {
            get => _machineNo;
            set
            {
                SetProperty(ref _machineNo, value);
            }
        }

        public bool IsRunning { get; set; }
        public object Data { get; set; }

        public ViewModelBase(Machine machine)
        {
            _machine = machine;
            _machineNo = machine.MachineNo;

            Task.Run(() =>
            {
                IsRunning = true;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (IsRunning)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Time = DateTime.Now.ToString("HH:mm:ss");
                        }));

                        stopwatch.Restart();
                    }

                    Thread.Sleep(100);
                }
            });
        }

        public abstract void Initialization();

        public string GetString(string key) => ResourceManager.Instance.GetString(key);

    }
}
