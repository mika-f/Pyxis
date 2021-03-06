﻿using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.EntityFrameworkCore;
using Microsoft.HockeyApp;
using Microsoft.Practices.Unity;

using Prism.Unity.Windows;
using Prism.Windows.AppModel;

using Pyxis.Models;
using Pyxis.Models.Database;
using Pyxis.Services;
using Pyxis.Services.Interfaces;

using Sagitta;

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
            RequestedTheme = ApplicationTheme.Dark;

            InitializeComponent();

            using (var db = new CacheContext())
                db.Database.Migrate();
        }

        #region Overrides of PrismApplication

        protected override async Task OnSuspendingApplicationAsync()
        {
            await base.OnSuspendingApplicationAsync();
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs e)
        {
            return Task.CompletedTask;
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            return shell;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            PyxisConstants.UIDispatcher = Window.Current.CoreWindow.Dispatcher;

            var pixivClient = new PixivClient("bYGKuGVw91e0NMfPGp44euvGt59s", "HP3RmkgAmEGro0gn1x9ioawQE8WMfvLXDz3ZqxpK");
            // New API key 'KzEZED7aC0vird8jWyHM38mXjNTY', 'W9JZoJe00qPvJsiyCGT3CCtC6ZUtdpKpzMbNlUGP'
            var accountService = new AccountService(pixivClient);
            // Prism
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            // Pyxis
            Container.RegisterInstance(pixivClient, new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IAccountService>(accountService, new ContainerControlledLifetimeManager());
            Container.RegisterType<IFileCacheService, FileCacheService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IObjectCacheService, ObjectCacheService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISessionObjectStorageService, SessionObjectStorageService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

            // await accountService.ClearAsync();
            await accountService.LoginAsync();
            await base.OnInitializeAsync(args);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(Container.Resolve<IAccountService>().Account == null ? "Login" : "Home", null);
            return Task.CompletedTask;
        }

        #endregion Overrides of PrismApplication
    }
}