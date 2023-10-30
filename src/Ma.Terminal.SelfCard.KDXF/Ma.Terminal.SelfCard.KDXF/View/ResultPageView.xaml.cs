using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ma.Terminal.SelfCard.KDXF.View
{
    /// <summary>
    /// ResultPageView.xaml 的交互逻辑
    /// </summary>
    public partial class ResultPageView : Page, IPageViewInterface
    {
        ResultPageViewModel _viewModel = null;
        public IViewModel ViewModel => _viewModel;

        public Action<IPageViewInterface> OnClose { get; set; }

        public ResultPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<ResultPageViewModel>();
            DataContext = _viewModel;

            _viewModel.Close = () => OnClose?.Invoke(this);
        }

        public IPageViewInterface Init()
        {
            _viewModel.Initialization();
            return this;
        }
    }
}
