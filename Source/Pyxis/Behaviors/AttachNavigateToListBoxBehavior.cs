﻿using System;
using System.Collections.Generic;
using System.Globalization;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.Xaml.Interactivity;

using Pyxis.Attach;

namespace Pyxis.Behaviors
{
    internal class AttachNavigateToListBoxBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty RootFrameProperty =
            DependencyProperty.Register(nameof(RootFrame), typeof(Frame), typeof(AttachNavigationToPivotBehavior),
                                        new PropertyMetadata(null));

        public static readonly DependencyProperty ParentSplitViewProperty =
            DependencyProperty.Register(nameof(ParentSplitView), typeof(SplitView),
                                        typeof(AttachNavigateToListBoxBehavior), new PropertyMetadata(null));

        private readonly Stack<int> _pageStack;
        private bool _isAttached;
        private int _oldIndex;

        public Frame RootFrame
        {
            get { return (Frame) GetValue(RootFrameProperty); }
            set { SetValue(RootFrameProperty, value); }
        }

        public SplitView ParentSplitView
        {
            get { return (SplitView) GetValue(ParentSplitViewProperty); }
            set { SetValue(ParentSplitViewProperty, value); }
        }

        public AttachNavigateToListBoxBehavior()
        {
            _pageStack = new Stack<int>();
            _isAttached = false;
            _oldIndex = 0;
        }

        // https://github.com/PrismLibrary/Prism/blob/3dded2/Source/Windows10/Prism.Windows/PrismApplication.cs#L148-L171
        private Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = GetType().AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(GetType().FullName,
                                                                         typeof(App).Namespace + ".Views.{0}Page");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
                throw new ArgumentException(string.Format("{0}'{1}' is not found.", nameof(pageToken), pageToken));

            return viewType;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (!_isAttached && RootFrame != null)
            {
                RootFrame.Navigating += RootFrameOnNavigating;
                _isAttached = true;
            }
            SyncState();
        }

        private void RootFrameOnNavigating(object sender, NavigatingCancelEventArgs args)
        {
            if (args.NavigationMode == NavigationMode.Back)
                AssociatedObject.SelectedIndex = _pageStack.Pop();
            else if (args.NavigationMode == NavigationMode.New)
            {
                if (_oldIndex >= 0)
                    _pageStack.Push(_oldIndex);
            }

            _oldIndex = AssociatedObject.SelectedIndex;
            SyncState();
        }

        private void SyncState()
        {
            var item = AssociatedObject.SelectedItem as ListBoxItem;
            var pageToken = NavigateTo.GetPageToken(item);
            if (!string.IsNullOrWhiteSpace(pageToken))
                RootFrame?.Navigate(GetPageType(pageToken));
            if (ParentSplitView != null)
                ParentSplitView.IsPaneOpen = false;
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            base.OnDetaching();
        }

        #endregion
    }
}