using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.Model;
using Ma.Terminal.SelfCard.KDXF.View;
using Ma.Terminal.SelfCard.KDXF.ViewModel;
using Ma.Terminal.SelfCard.KDXF.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ma.Terminal.SelfCard.KDXF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        List<IPageViewInterface> _pages;
        bool isRunning = true;

        public MainWindow()
        {
            InitializeComponent();

            _pages = new List<IPageViewInterface>()
            {
                new MainPageView().Init()
            };

            MainFrame.Content = _pages[0];

#if DEBUG
            MainFrame.Height = 1080;
            MainFrame.Width = 607;
#else
            MainFrame.Height = 1920;
            MainFrame.Width = 1080;
#endif

            Task.Run(async () =>
            {
                Requester api = Ioc.Default.GetRequiredService<Requester>();
                Machine machine = Ioc.Default.GetRequiredService<Machine>();
                ItemsConfig config = Ioc.Default.GetRequiredService<ItemsConfig>();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                int tick = 30 * 60 * 1000;

                while (isRunning)
                {
                    if (stopwatch.ElapsedMilliseconds > tick)
                    {
                        try
                        {
                            await api.ReportDevice(new
                            {
                                Deviceid = machine.MachineNo,
                                config.Discards,
                                config.Cards,
                                config.Status,
                                Msg = config.Cards <= 0 ? "卡片不足" : string.Empty,
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex.Message);
                            _logger.Error(ex.StackTrace);
                        }

                        stopwatch.Restart();
                    }

                    Thread.Sleep(1000);
                }
            });
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
#if DEBUG
            this.Topmost = false;
#else
            this.Topmost = true;
#endif
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
        }

        protected override void OnClosed(EventArgs e)
        {
            isRunning = false;
            _pages.ForEach(p => p.ViewModel.IsRunning = false);
            base.OnClosed(e);
        }
    }
}
