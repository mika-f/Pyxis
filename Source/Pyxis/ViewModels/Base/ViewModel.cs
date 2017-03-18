﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Toolkit.Uwp.UI;

using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Pyxis.ViewModels.Base
{
    public class ViewModel : ViewModelBase, IDisposable
    {
        public CompositeDisposable CompositeDisposable { get; }

        protected ViewModel()
        {
            CompositeDisposable = new CompositeDisposable();
        }

        #region Implementation of IDisposable

        public void Dispose() => CompositeDisposable.Dispose();

        #endregion

        #region Overrides of ViewModelBase

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState,
                                              bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            if (suspending)
                CompositeDisposable.Dispose();
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var frame = ((Window.Current.Content as AppShell).FindDescendantByName("HamburgerMenuControl") as ContentControl)?.Content as Frame;
            if (frame != null && frame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        #endregion
    }
}