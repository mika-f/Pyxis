﻿using System.Diagnostics;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.EntityFrameworkCore;
using Microsoft.HockeyApp;
using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Alpha;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Cache;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;

using LifetimeManager = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;

namespace Pyxis
{
    /// <summary>
    ///     既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    public sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        ///     単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///     最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            HockeyClient.Current.Configure("096f082c19e54f24aab0d31ff4d9bfb7");
            InitializeComponent();
            UnhandledException += (sender, e) =>
            {
                Debug.WriteLine("");
                Debug.WriteLine(e.Message);
                e.Handled = true;
                Application.Current.Exit();
            }
                ;
        }

        #region Overrides of PrismApplication

        protected override async Task OnSuspendingApplicationAsync()
        {
            var browsingHistory = Container.Resolve<IBrowsingHistoryService>();
            browsingHistory.ForcePush();
            await base.OnSuspendingApplicationAsync();
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs e)
        {
            var args = e as ProtocolActivatedEventArgs;
            if (args == null)
                return Task.CompletedTask;
            PyxisSchemeActivator.Activate(args.Uri, NavigationService);
            return Task.CompletedTask;
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            shell.SetCategoryService(Container.Resolve<ICategoryService>());
            return shell;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            UIDispatcherScheduler.Initialize();
            var pixivClient = new PixivApiClient();
            var accountService = new AccountService(pixivClient);
            Container.RegisterInstance<IPixivClient>(pixivClient, new LifetimeManager());
            Container.RegisterInstance<IAccountService>(accountService, new LifetimeManager());
            Container.RegisterType<IBrowsingHistoryService, BrowsingHistoryService>(new LifetimeManager());
            Container.RegisterType<ICacheService, CacheService>(new LifetimeManager());
            Container.RegisterType<IImageStoreService, ImageStoreService>(new LifetimeManager());
            Container.RegisterType<IDialogService, DialogService>(new LifetimeManager());
            Container.RegisterType<ICategoryService, CategoryService>(new LifetimeManager());
#if DEBUG
            Container.RegisterType<ILicenseService, LocalLicenseService>(new LifetimeManager());
#else
            Container.RegisterType<ILicenseService, LicenseService>(new LifetimeManager());
#endif
            // Container.RegisterInstance<IPixivClient>(new PixivWebClient(), new ContainerControlledLifetimeManager());
#if !OFFLINE
            await accountService.Login();
#endif
            Debug.WriteLine("Process A");
            if (!((LaunchActivatedEventArgs) args).PrelaunchActivated)
            {
                Debug.WriteLine("Process B");
                using (var db = new CacheContext())
                {
                    if (!db.IsCreated)
                        await Container.Resolve<IImageStoreService>().ClearImagesAsync();
                    db.Database.Migrate();
                }
            }

            Debug.WriteLine("Process C");
            await base.OnInitializeAsync(args);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("HomeMain", "{\"ContentType\":0}");
            var param = new BrowsingHistoryParameter {ContentType = ContentType2.IllustAndManga};
            Debug.WriteLine(param.ToJson());
            return Task.CompletedTask;
        }

        #endregion Overrides of PrismApplication
    }
}