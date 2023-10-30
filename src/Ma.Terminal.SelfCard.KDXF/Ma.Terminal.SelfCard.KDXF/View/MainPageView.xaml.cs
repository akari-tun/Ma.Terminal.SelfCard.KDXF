using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.Controls;
using Ma.Terminal.SelfCard.KDXF.Model;
using Ma.Terminal.SelfCard.KDXF.ViewModel;
using System;
using System.Collections.Generic;
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

namespace Ma.Terminal.SelfCard.KDXF.View
{
    /// <summary>
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPageView : Page, IPageViewInterface
    {
        MainPageViewModel _viewModel = null;
        public IViewModel ViewModel => _viewModel;

        public MainPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<MainPageViewModel>();
            DataContext = _viewModel;

            var signIn = new SignInPageView();
            var info = new InfoPageView();
            var result = new ResultPageView();
            var needKnow = new NeedKnowPageView();

            signIn.OnClose = SubPageClose;
            info.OnClose = SubPageClose;
            result.OnClose = SubPageClose;
            needKnow.OnClose = SubPageClose;

            (signIn.ViewModel as SignInPageViewModel).NavigationTo = NavigationToTO;
            (info.ViewModel as InfoPageViewModel).NavigationTo = NavigationToTO;

            _viewModel.FunPages = new Dictionary<FunctionEnum, IPageViewInterface>()
            {
                { FunctionEnum.SIGN, signIn },
                { FunctionEnum.INFO, info },
                { FunctionEnum.NEEDKNOW, needKnow },
                { FunctionEnum.LOST, null },
                { FunctionEnum.UNLOST, null },
                { FunctionEnum.RESULT, result },
            };
        }

        public IPageViewInterface Init()
        {
            _viewModel.Initialization();
            return this;
        }

        private void ClickEffectGrid_OnClick(ClickEffectGrid sender)
        {
            FunctionEnum func = (FunctionEnum)Convert.ToInt32(sender.Tag);
            SecondyFrame.Content = _viewModel.ShowSubPage(func);
        }

        private void NavigationToTO(FunctionEnum func, object data)
        {
            SecondyFrame.Content = _viewModel.ShowSubPage(func, data);
        }

        private void SubPageClose(IPageViewInterface page)
        {
            page.ViewModel.Data = null;
            page.ViewModel.IsRunning = false;

            SecondyFrame.Content = null;

            _viewModel.ContainerVisibility = Visibility.Hidden;
            _viewModel.CurrentView = null;
            _viewModel.Initialization();
        }
    }
}
