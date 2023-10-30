using Ma.Terminal.SelfCard.KDXF.Device;
using Ma.Terminal.SelfCard.KDXF.Model;
using Ma.Terminal.SelfCard.KDXF.WebApi;
using Ma.Terminal.SelfCard.KDXF.WebApi.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ma.Terminal.SelfCard.KDXF.ViewModel
{
    public class InfoPageViewModel : ViewModelBase
    {
        const string SUREISSUE = "pack://SiteOfOrigin:,,,/Resource/Image/SureIssue.png";
        const string ISSUING = "pack://SiteOfOrigin:,,,/Resource/Image/Issuing.png";
        const string REISSE = "pack://SiteOfOrigin:,,,/Resource/Image/ReIssue.png";

        private ItemsConfig _config;
        private Requester _api;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private Device.CardReader.Reader _reader;
        private Queue<object> _finishCards;
        private bool _isLoading = false;

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

        string _buttonTextPath = SUREISSUE;
        public string ButtonTextPath
        {
            get { return _buttonTextPath; }
            set { SetProperty(ref _buttonTextPath, value); }
        }

        bool _isClickable = false;
        public bool IsClickable
        {
            get { return _isClickable; }
            set { SetProperty(ref _isClickable, value); }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        string _account;
        public string Account
        {
            get { return _account; }
            set { SetProperty(ref _account, value); }
        }

        string _className;
        public string ClassName
        {
            get { return _className; }
            set { SetProperty(ref _className, value); }
        }

        string _branchDept;
        public string BranchDept
        {
            get { return _branchDept; }
            set { SetProperty(ref _branchDept, value); }
        }

        string _roomInfo;
        public string RoomInfo
        {
            get { return _roomInfo; }
            set { SetProperty(ref _roomInfo, value); }
        }

        public Action<FunctionEnum, object> NavigationTo { get; set; }

        public InfoPageViewModel(
            Machine machine,
            ItemsConfig config,
            Requester requester,
            Device.CardReader.Reader reader)
            : base(machine)
        {
            _config = config;
            _api = requester;
            _reader = reader;

            _finishCards = new Queue<object>();
        }

        public override void Initialization()
        {
            Error = string.Empty;
            IsClickable = true;
            Opacity = 1;
            _buttonTextPath = SUREISSUE;

            Name = string.Empty;
            Account = string.Empty;
            ClassName = string.Empty;
            BranchDept = string.Empty;
            RoomInfo = string.Empty;

            if (Data != null && Data is UserInfo userInfo)
            {
                Name = userInfo.Username;
                Account = userInfo.Userid;
                ClassName = userInfo.Classname;
                BranchDept = userInfo.Branchinfo;
                RoomInfo = userInfo.Roominfo;
            }
        }

        public void IssueCard()
        {
            IsClickable = true;
            Opacity = 0.5f;
            ButtonTextPath = ISSUING;
            Error = string.Empty;

            IsRunning = true;

            Task.Run(() =>
            {
                try
                {
                    _reader.Read(ReaderCallback);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    _logger.Error(ex.StackTrace);

                    Error = string.IsNullOrEmpty(_reader.LastError) ? "制卡失败" : _reader.LastError;
                    IsClickable = true;
                    Opacity = 1f;
                    ButtonTextPath = REISSE;
                }
            });
        }

        private void ReaderCallback(bool result, IReader reader, string uuid)
        {
            // 在这里根据读卡器或扫码头返回的状态处理返回结果
            if (result && reader is Device.CardReader.Reader cardReader)
            {
                if (cardReader.Issue(uuid))
                {
                    object obj = new
                    {
                        Userid = Account,
                        Uid = uuid
                    };

                    _config.Cards = _config.Cards - 1;
                    _config.Status = _config.Cards > 0 ? 1 : -1;
                    _config.Save();

                    _finishCards.Enqueue(obj);
                    if (!_isLoading) RunUpload();

                    Application.Current.Dispatcher.Invoke(NavigationTo, FunctionEnum.RESULT, obj);
                    return;
                }
            }
            else
            {
                Error = string.IsNullOrEmpty(_reader.LastError) ? "制卡失败" : _reader.LastError;
            }

            IsClickable = true;
            Opacity = 1f;
            ButtonTextPath = REISSE;
        }

        private void RunUpload()
        {
            Task.Run(async () =>
            {
                _isLoading = true;

                while (_isLoading)
                {
                    try
                    {
                        object obj = _finishCards.Count > 0 ? _finishCards.Peek() : null;

                        if (obj != null)
                        {
                            var result = await _api.NotifyCard(obj);
                            if (!string.IsNullOrEmpty(result) && _api.LastCode == 0) _ = _finishCards.Dequeue();
                        }

                        _isLoading = _finishCards.Count > 0;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        _logger.Error(ex.StackTrace);
                    }

                    for (int i = 0; i < 6000; i++)
                    {
                        if (!_isLoading) break;
                        await Task.Delay(10);
                    }
                }
            });
        }
    }
}
