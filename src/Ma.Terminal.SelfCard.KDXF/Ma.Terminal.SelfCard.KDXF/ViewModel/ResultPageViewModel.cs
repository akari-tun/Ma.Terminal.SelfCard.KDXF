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
    public class ResultPageViewModel : ViewModelBase
    {
        public Action Close;

        public ResultPageViewModel(Machine machine)
            : base(machine)
        {

        }

        public override void Initialization()
        {
            Task.Run(() =>
            {
                IsRunning = true;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (IsRunning && stopwatch.ElapsedMilliseconds < 3000)
                {
                    Thread.Sleep(100);
                }

                if (Close != null) Application.Current.Dispatcher.Invoke(Close);
            });
        }
    }
}
