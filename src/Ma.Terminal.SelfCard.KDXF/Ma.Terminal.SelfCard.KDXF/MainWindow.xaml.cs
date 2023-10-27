using Ma.Terminal.SelfCard.KDXF.View;
using Ma.Terminal.SelfCard.KDXF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        List<IPageViewInterface> _pages;

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
            _pages.ForEach(p => p.ViewModel.IsRunning = false);
            base.OnClosed(e);
        }
    }
}
