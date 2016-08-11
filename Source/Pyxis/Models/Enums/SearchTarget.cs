﻿using System;

namespace Pyxis.Models.Enums
{
    public enum SearchTarget
    {
        TagPartial,

        TagTotal,

        TitleCaption,

        Text,

        Keyword
    }

    internal static class SearchTargetExt
    {
        public static string ToParamString(this SearchTarget target)
        {
            switch (target)
            {
                case SearchTarget.TagPartial:
                    return "partial_match_for_tags";

                case SearchTarget.TagTotal:
                    return "exact_match_for_tags";

                case SearchTarget.TitleCaption:
                    return "title_and_caption";

                case SearchTarget.Text:
                    return "text";

                case SearchTarget.Keyword:
                    return "keyword";

                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }
    }
}