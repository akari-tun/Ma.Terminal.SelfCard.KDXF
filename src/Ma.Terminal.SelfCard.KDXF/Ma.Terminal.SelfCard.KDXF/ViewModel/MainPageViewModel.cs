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
#if DEBUG
        const double HEIGTH_RATIO = 1080d / 1920d;
        const double WIDTH_RATIO = 607d / 1080d;
#else
        const double HEIGTH_RATIO = 1920d / 1920d;
        const double WIDTH_RATIO = 1080d / 1080d;
#endif

        Thickness _signMargin = new Thickness(110d * WIDTH_RATIO, 503d * HEIGTH_RATIO, 110d * WIDTH_RATIO, 502d * HEIGTH_RATIO);
        Thickness _infoMargin = new Thickness(110d * WIDTH_RATIO, 447d * HEIGTH_RATIO, 110d * WIDTH_RATIO, 446d * HEIGTH_RATIO);
        Thickness _needKnowMargin = new Thickness(110d * WIDTH_RATIO, 339d * HEIGTH_RATIO, 110d * WIDTH_RATIO, 289 * HEIGTH_RATIO);
        Thickness _resultMargin = new Thickness(179d * WIDTH_RATIO, 623d * HEIGTH_RATIO, 179d * WIDTH_RATIO, 622d * HEIGTH_RATIO);

        ItemsConfig _config;

        public Dictionary<FunctionEnum, IPageViewInterface> FunPages { get; set; }
        public IPageViewInterface CurrentView { get; set; }


        Thickness _containerMargin;
        public Thickness ContainerMargin
        {
            get { return _containerMargin; }
            set { SetProperty(ref _containerMargin, value); }
        }

        Visibility _containerVisibility;
        public Visibility ContainerVisibility
        {
            get { return _containerVisibility; }
            set { SetProperty(ref _containerVisibility, value); }
        }

        bool _isClickable = false;
        public bool IsClickable
        {
            get { return _isClickable; }
            set { SetProperty(ref _isClickable, value); }
        }

        public MainPageViewModel(Machine machine, ItemsConfig config)
            : base(machine)
        {
            _config = config;
            _containerMargin = _signMargin;
            _containerVisibility = Visibility.Hidden;
        }

        public override void Initialization()
        {
            IsClickable = _config.Status > 0;
            OnPropertyChanged(nameof(ContainerMargin));
            OnPropertyChanged(nameof(ContainerVisibility));
        }

        public IPageViewInterface ShowSubPage(FunctionEnum func, object data = null)
        {
            CurrentView = FunPages[func];

            if (CurrentView != null)
            {
                switch (func)
                {
                    case FunctionEnum.SIGN:
                        ContainerMargin = _signMargin;
                        break;
                    case FunctionEnum.INFO:
                        ContainerMargin = _infoMargin;
                        break;
                    case FunctionEnum.NEEDKNOW:
                        ContainerMargin = _needKnowMargin;
                        break;
                    case FunctionEnum.LOST:
                        break;
                    case FunctionEnum.UNLOST:
                        break;
                    case FunctionEnum.RESULT:
                        ContainerMargin = _resultMargin;
                        break;
                }

                ContainerVisibility = Visibility.Visible;
                if (data != null) CurrentView.ViewModel.Data = data;
                CurrentView.Init();
            }

            return CurrentView;
        }
    }
}
