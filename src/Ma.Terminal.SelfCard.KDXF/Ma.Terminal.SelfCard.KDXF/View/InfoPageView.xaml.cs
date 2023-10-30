using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.Controls;
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
    /// InfoPageView.xaml 的交互逻辑
    /// </summary>
    public partial class InfoPageView : Page, IPageViewInterface
    {
        InfoPageViewModel _viewModel = null;
        public IViewModel ViewModel => _viewModel;

        public Action<IPageViewInterface> OnClose { get; set; }

        public InfoPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<InfoPageViewModel>();
            DataContext = _viewModel;
        }

        public IPageViewInterface Init()
        {
            _viewModel.Initialization();
            return this;
        }

        private void XButton_OnClick(ClickEffectGrid sender)
        {
            OnClose?.Invoke(this);
        }

        private void Issue_OnClick(ClickEffectGrid sender)
        {
            _viewModel.IssueCard();
        }
    }
}
