﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    public sealed class IndexHub : Control
    {
        public static readonly DependencyProperty HubHeaderTextProperty =
            DependencyProperty.Register(nameof(HubHeaderText), typeof(string), typeof(IndexHub), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(IndexHub), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(IndexHub), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty HubContentTemplateProperty =
            DependencyProperty.Register(nameof(HubContentTemplate), typeof(DataTemplate), typeof(IndexHub), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(IndexHub), new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.Register(nameof(DesiredWidth), typeof(double), typeof(IndexHub), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(IndexHub), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(IndexHub), new PropertyMetadata(default(object)));

        public string HubHeaderText
        {
            get => (string) GetValue(HubHeaderTextProperty);
            set => SetValue(HubHeaderTextProperty, value);
        }

        public string HeaderText
        {
            get => (string) GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate HubContentTemplate
        {
            get => (DataTemplate) GetValue(HubContentTemplateProperty);
            set => SetValue(HubContentTemplateProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public double DesiredWidth
        {
            get => (double) GetValue(DesiredWidthProperty);
            set => SetValue(DesiredWidthProperty, value);
        }

        public double ItemHeight
        {
            get => (double) GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public IndexHub()
        {
            DefaultStyleKey = typeof(IndexHub);
        }
    }
}