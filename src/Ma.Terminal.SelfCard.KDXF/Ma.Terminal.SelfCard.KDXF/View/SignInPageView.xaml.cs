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
    /// SignNamePageView.xaml 的交互逻辑
    /// </summary>
    public partial class SignInPageView : Page, IPageViewInterface
    {
        SignInPageViewModel _viewModel = null;
        public IViewModel ViewModel => _viewModel;

        public Action<IPageViewInterface> OnClose { get; set; }

        public SignInPageView()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<SignInPageViewModel>();
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

        private void Search_OnClick(ClickEffectGrid sender)
        {
            if (TextInput.Text.Length < 11)
            {
                _viewModel.Error = "请输入正确的数据";
                return;
            }

            _viewModel.IdCard = string.IsNullOrEmpty(_viewModel.IdCard) ? TextInput.Text : _viewModel.IdCard;
            _viewModel.InputText = $"{_viewModel.IdCard.Substring(0, 4)}**********{_viewModel.IdCard.Substring(_viewModel.IdCard.Length - 4, 4)}";
            _viewModel.QueryUserInfo();
        }

        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.Opacity = string.IsNullOrEmpty(TextInput.Text) ? 0.5f : 1f;
            _viewModel.IsClickable = !string.IsNullOrEmpty(TextInput.Text);
        }
    }
}
