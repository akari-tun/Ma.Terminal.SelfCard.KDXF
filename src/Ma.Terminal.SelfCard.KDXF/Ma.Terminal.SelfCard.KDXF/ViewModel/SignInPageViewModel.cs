using Ma.Terminal.SelfCard.KDXF.Device;
using Ma.Terminal.SelfCard.KDXF.Model;
using Ma.Terminal.SelfCard.KDXF.WebApi;
using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public class SignInPageViewModel : ViewModelBase
    {
        const string SEARCH = "pack://SiteOfOrigin:,,,/Resource/Image/Search.png";
        const string SEARCHING = "pack://SiteOfOrigin:,,,/Resource/Image/Searching.png";

        private Requester _api;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private Action<string> OnDataReaded;
        private IReader _idReader;
        private IReader _qrReader;

        string _error;
        public string Error
        {
            get { return _error; }
            set { SetProperty(ref _error, value); }
        }

        float _opacity;
        public float Opacity
        {
            get { return _opacity; }
            set { SetProperty(ref _opacity, value); }
        }

        string _buttonTextPath = SEARCH;
        public string ButtonTextPath
        {
            get { return _buttonTextPath; }
            set { SetProperty(ref _buttonTextPath, value); }
        }

        string _inputText;
        public string InputText
        {
            get { return _inputText; }
            set
            {
                SetProperty(ref _inputText, value);
            }
        }

        bool _isClickable = false;
        public bool IsClickable
        {
            get { return _isClickable; }
            set { SetProperty(ref _isClickable, value); }
        }

        public string IdCard { get; set; }
        public Action<FunctionEnum, object> NavigationTo { get; set; }

        public SignInPageViewModel(
            Machine machine,
            Requester requester,
            Device.IdReader.Reader idReader,
            Device.QrReader.Reader qrReader)
            : base(machine)
        {
            _api = requester;
            _idReader = idReader;
            _qrReader = qrReader;

            OnDataReaded = p => InputText = p;
        }

        public override void Initialization()
        {
            IdCard = string.Empty;
            InputText = string.Empty;
            Error = string.Empty;
            Opacity = 0.5f;
            IsClickable = false;
            ButtonTextPath = SEARCH;

            IsRunning = true;

            // 在ReaderCallback函数中处理返回结果
            Task.Run(() => _idReader.Read(ReaderCallback));
            Task.Run(() => _qrReader.Read(ReaderCallback));
        }

        public void QueryUserInfo()
        {
            IsClickable = false;
            Opacity = 0.5f;
            ButtonTextPath = SEARCHING;
            Error = string.Empty;

            Task.Run(async () =>
            {
                try
                {
                    var result = await _api.GetUserInfo(new
                    {
                        Idcard = IdCard,
                        Phone = string.Empty,
                        Deviceid = MachineNo
                    });

                    if (_api.LastCode == 0 && result != null)
                    {
                        await Application.Current.Dispatcher.BeginInvoke(NavigationTo, FunctionEnum.INFO, result);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);
                }

                if (_api.LastCode != 0)
                {
                    Error = string.IsNullOrEmpty(_api.LastMessage) ? "未查询到结果" : _api.LastMessage;
                    IsClickable = true;
                    Opacity = 1f;
                    ButtonTextPath = SEARCH;
                }
            });
        }

        private async void ReaderCallback(bool result, IReader reader, string data)
        {
            await Task.Delay(500);

            // 在这里根据读卡器或扫码头返回的状态处理返回结果
            if (result)
            {
                await Application.Current.Dispatcher.BeginInvoke(OnDataReaded, data);
            }

            if (IsRunning) reader.Read(ReaderCallback);
        }
    }
}
