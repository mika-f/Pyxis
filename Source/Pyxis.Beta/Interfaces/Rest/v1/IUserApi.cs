﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IUserApi
    {
        IUserBookmarkTagsApi BookmarkTags { get; }

        IUserBookmarksApi Bookmarks { get; }

        IUserBrowsingHistoryApi BrowsingHistory { get; }

        IUserFollowApi Follow { get; }

        Task<IUserDetail> DetailAsync(params Expression<Func<string, object>>[] parameters);

        Task<IUserPreviews> FollowerAsync(params Expression<Func<string, object>>[] parameters);

        Task<IUserPreviews> FollowingAsync(params Expression<Func<string, object>>[] parameters);

        Task<IIllusts> IllustsAsync(params Expression<Func<string, object>>[] parameters);

        Task<IUserPreviews> MypixivAsync(params Expression<Func<string, object>>[] parameters);

        Task<INovels> NovelsAsync(params Expression<Func<string, object>>[] parameters);

        Task<IUserPreviews> RecommendedAsync(params Expression<Func<string, object>>[] parameters);

        Task<IUserPreviews> RelatedAsync(params Expression<Func<string, object>>[] parameters);
    }
}