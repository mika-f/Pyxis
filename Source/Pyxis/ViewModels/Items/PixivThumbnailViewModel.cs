﻿using System;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Items
{
    public class PixivThumbnailViewModel : TappableThumbnailViewModel
    {
        protected INavigationService NavigationService { get; }
        protected INovel Novel { get; }
        protected IIllust Illust { get; }

        public PixivThumbnailViewModel(IIllust illust, IImageStoreService imageStoreService,
                                       INavigationService navigationService)
        {
            Illust = illust;
            NavigationService = navigationService;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivImage(illust, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        public PixivThumbnailViewModel(INovel novel, IImageStoreService imageStoreService,
                                       INavigationService navigationService)
        {
            Novel = novel;
            NavigationService = navigationService;

            ThumbnailPath = PyxisConstants.DummyImage;
            Thumbnailable = new PixivNovel(novel, imageStoreService);
            Thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                         .Where(w => !string.IsNullOrWhiteSpace(w))
                         .ObserveOnUIDispatcher()
                         .Subscribe(w => ThumbnailPath = w)
                         .AddTo(this);
        }

        #region Overrides of TappableThumbnailViewModel

        public override void OnItemTapped()
        {
            if (Illust != null)
            {
                var parameter = new IllustDetailParameter {Illust = Illust};
                NavigationService.Navigate(Illust.PageCount == 1 ? "Detail.IllustDetail" : "Detail.MangaDetail",
                                           parameter.ToJson());
            }
            else
            {
                var parameter = new NovelDetailParameter {Novel = Novel};
                NavigationService.Navigate("Detail.NovelDetail", parameter.ToJson());
            }
        }

        #endregion
    }
}