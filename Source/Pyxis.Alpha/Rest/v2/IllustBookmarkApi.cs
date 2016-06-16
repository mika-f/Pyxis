﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v2;
using Pyxis.Beta.Interfaces.Rest.v2;

namespace Pyxis.Alpha.Rest.v2
{
    public class IllustBookmarkApi : IIllustBookmarkApi
    {
        private readonly PixivApiClient _client;

        public IllustBookmarkApi(PixivApiClient client)
        {
            _client = client;
        }

        #region Implementation of IIllustBookmarkApi

        public async Task<IBookmarkDetail> DetailAsync(params Expression<Func<string, object>>[] parameters)
        {
            var detail = await _client.GetAsync<BookmarkDetailOwner>(Endpoints.IllustBookmarkDetail, true, parameters);
            return detail.BookmarkDetail;
        }

        #endregion
    }
}