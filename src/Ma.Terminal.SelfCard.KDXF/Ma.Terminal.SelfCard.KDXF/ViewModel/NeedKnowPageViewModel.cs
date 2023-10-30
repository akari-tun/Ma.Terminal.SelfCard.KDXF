using Ma.Terminal.SelfCard.KDXF.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public class NeedKnowPageViewModel : ViewModelBase
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private Machine _machine;

        public WebBrowser WebCtr { get; set; }

        public NeedKnowPageViewModel(Machine machine)
            : base(machine)
        {
            _machine = machine;
        }

        public override void Initialization()
        {
            try
            {
                var url = new Uri(_machine.NeedKnowUrl);
                WebCtr.Source = url;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }

        }
    }
}
