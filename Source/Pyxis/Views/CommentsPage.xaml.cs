﻿using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Pyxis.Views
{
    /// <summary>
    ///     それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class CommentsPage : Page
    {
        public CommentsPageViewModel ViewModel => DataContext as CommentsPageViewModel;

        public CommentsPage()
        {
            InitializeComponent();
        }
    }
}