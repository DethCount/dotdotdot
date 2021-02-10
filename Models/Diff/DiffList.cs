using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using dotdotdot.Services;

namespace dotdotdot.Models.Diff
{
    public class DiffList<T,U> : List<T> where T : Model<U>
    {
        [JsonConverter(typeof(DiffStatusJsonConverter))]
        public Status status {
            get {
                foreach (T obj in this) {
                    if (obj.status != Status.UNCHANGED) {
                        return Status.MODIFIED;
                    }
                }

                return Status.UNCHANGED;
            }
        }
    }
}