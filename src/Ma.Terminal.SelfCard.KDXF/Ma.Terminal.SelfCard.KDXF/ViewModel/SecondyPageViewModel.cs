using Ma.Terminal.SelfCard.KDXF.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public class SecondyPageViewModel : ViewModelBase
    {
#if DEBUG
        const double HEIGTH_RATIO = 1080 / 1920;
        const double WIDTH_RATIO = 607 / 1080;
#else
        const double HEIGTH_RATIO = 1920 / 1920;
        const double WIDTH_RATIO = 1080 / 1080;
#endif

        Thickness _signMargin = new Thickness(110 * WIDTH_RATIO, 503 * HEIGTH_RATIO, 110 * WIDTH_RATIO, 502 * HEIGTH_RATIO);
        Thickness _infoMargin = new Thickness(110 * WIDTH_RATIO, 447 * HEIGTH_RATIO, 110 * WIDTH_RATIO, 446 * HEIGTH_RATIO);
        Thickness _resultMargin = new Thickness(179 * WIDTH_RATIO, 623 * HEIGTH_RATIO, 179 * WIDTH_RATIO, 622 * HEIGTH_RATIO);

        Thickness _containerMargin;
        public Thickness ContainerMargin
        {
            get { return _containerMargin; }
            set { SetProperty(ref _containerMargin, value); }
        }


        public SecondyPageViewModel(Machine machine)
            : base(machine)
        {
            _containerMargin = _signMargin;
        }

        public override void Initialization()
        {
            OnPropertyChanged(nameof(ContainerMargin));
        }
    }
}
