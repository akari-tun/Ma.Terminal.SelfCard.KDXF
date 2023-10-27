using CommunityToolkit.Mvvm.DependencyInjection;
using Ma.Terminal.SelfCard.KDXF.Model;
using Ma.Terminal.SelfCard.KDXF.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Ma.Terminal.SelfCard.KDXF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;
        Logger _logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "Terminal.SelfService");
            if (mutex.WaitOne(0, false))
            {
                _logger.Info($"App is OnStartup...");
                base.OnStartup(e);
            }
            else
            {
                _logger.Info($"App is running in anther thread!");
                this.Shutdown();
            }

            ConfigurationBuilder cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.AddJsonFile("machine.json", optional: false, reloadOnChange: false);
            IConfigurationRoot cfgRoot = cfgBuilder.Build();

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton(typeof(MainPageViewModel))
                .AddSingleton(new Machine()
                {
                    MachineNo = cfgRoot.GetSection("MachineNo").Value,
                    ApiUrl = cfgRoot.GetSection("ApiUrl").Value,
                    AppId = cfgRoot.GetSection("AppId").Value,
                })
                .BuildServiceProvider());

            try
            {
                //RSAUtils.Init(cfgRoot.GetSection("PrivateKey").Value);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }

            var path = $"pack://application:,,,/Resource/String.xaml";
            Resources.MergedDictionaries[0].Source = new Uri(path);
            ResourceManager.Instance.StringResourceDictionary = Resources.MergedDictionaries[0];

        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        /// <summary>
        /// 应用程序启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                _logger.Error(e.Exception.Message);
                _logger.Error(e.Exception.StackTrace);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    _logger.Error(exception.Message);
                    _logger.Error(exception.StackTrace);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error(ex.StackTrace);
            }
        }
    }
}
