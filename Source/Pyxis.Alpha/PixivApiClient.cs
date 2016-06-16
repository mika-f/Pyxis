﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Pyxis.Alpha.Internal;
using Pyxis.Alpha.Rest.v1;
using Pyxis.Beta.Exceptions;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Beta.Interfaces.Rest.Pximg;
using Pyxis.Beta.Interfaces.Rest.v1;

using IllustV1 = Pyxis.Beta.Interfaces.Rest.v1.IIllustApi;
using IllustV2 = Pyxis.Beta.Interfaces.Rest.v2.IIllustApi;
using INovelV1 = Pyxis.Beta.Interfaces.Rest.v1.INovelApi;
using INovelV2 = Pyxis.Beta.Interfaces.Rest.v2.INovelApi;
using IllustApiV1 = Pyxis.Alpha.Rest.v1.IllustApi;
using IllustApiV2 = Pyxis.Alpha.Rest.v2.IllustApi;
using NovelApiV1 = Pyxis.Alpha.Rest.v1.NovelApi;
using NovelApiV2 = Pyxis.Alpha.Rest.v2.NovelApi;

namespace Pyxis.Alpha
{
    /// <summary>
    ///     pixiv への API アクセスを行います。
    /// </summary>
    public class PixivApiClient : IPixivClient
    {
        private static Func<Expression<Func<string, object>>, string> F1 => expr => expr.Parameters[0].Name;

        private static Func<Expression<Func<string, object>>, string> F2
            => expr => expr.Compile().Invoke(null).ToString();

        public string AccessToken { get; set; }

        private IList<KeyValuePair<string, string>> GetPrameter(params Expression<Func<string, object>>[] parameters)
            => parameters.Select(w => new KeyValuePair<string, string>(F1(w), F2(w))).ToList();

        #region Implementation of IPixivClient

        public IAuthorizationApi Authorization => new AuthorizationApi(this);
        public IApplicationInfoApi ApplicationInfo => new ApplicationInfoApi(this);
        public IllustV1 IllustV1 => new IllustApiV1(this);
        public IMangaApi Manga => new MangaApi(this);
        public INovelV1 NovelV1 => new NovelApiV1(this);
        public ISearchApi Search => new SearchApi(this);
        public ISpotlightApi Spotlight => new SpotlightApi(this);
        public ITrendingTagsApi TrendingTags => new TrendingTagsApi(this);
        public IUgoiraApi Ugoira => new UgoiraApi(this);
        public IUserApi User => new UserApi(this);
        public IllustV2 IllustV2 => new IllustApiV2(this);
        public INovelV2 NovelV2 => new NovelApiV2(this);
        public IPximgApi Pximg { get; }

        public async Task<T> GetAsync<T>(string url, bool requireAuth,
                                         params Expression<Func<string, object>>[] parameters)
        {
            if (requireAuth && string.IsNullOrWhiteSpace(AccessToken))
                throw new AuthenticateRequiredException();

            var client = new HttpClient(new PixivHttpClientHandler(this));
            var param = string.Join("&", GetPrameter(parameters).Select(w => $"{w.Key}={Uri.EscapeDataString(w.Value)}"));
            url += "?" + param;

            var response = await client.GetAsync(url);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> PostAsync<T>(string url, bool requireAuth,
                                          params Expression<Func<string, object>>[] parameters)
        {
            if (requireAuth && string.IsNullOrWhiteSpace(AccessToken))
                throw new AuthenticateRequiredException();

            var client = new HttpClient(new PixivHttpClientHandler(this));
            var param = GetPrameter(parameters);
            var content = new FormUrlEncodedContent(param);

            var response = await client.PostAsync(url, content);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        #endregion
    }
}