using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public class MainPageViewModel : ViewModelBase
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

        public MainPageViewModel(Machine machine)
        {
            _machine = machine;

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

        public override void Initialization()
        {
            var machine = Ioc.Default.GetRequiredService<Machine>();
        }
    }
}
