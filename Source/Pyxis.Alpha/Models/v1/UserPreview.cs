﻿using System.Collections.Generic;

using Newtonsoft.Json;

using Pyxis.Alpha.Converters;
using Pyxis.Beta.Interfaces.Models.v1;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Pyxis.Alpha.Models.v1
{
    public class UserPreview : IUserPreview
    {
        #region Implementation of IUserPreview

        [JsonProperty("user")]
        [JsonConverter(typeof(InterfaceToConcreate<User>))]
        public IUser User { get; set; }

        [JsonProperty("illusts")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<Illust>>))]
        public IList<IIllust> Illusts { get; set; }

        [JsonProperty("novels")]
        [JsonConverter(typeof(InterfaceToConcreate<IList<Novel>>))]
        public IList<INovel> Novels { get; set; }

        #endregion
    }
}