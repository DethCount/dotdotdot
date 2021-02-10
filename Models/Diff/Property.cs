using System;
using System.Text.Json.Serialization;
using dotdotdot.Services;

namespace dotdotdot.Models.Diff
{
    public class Property<T>
    {
        [JsonConverter(typeof(DiffStatusJsonConverter))]
        public Status status {
            get {
                if (from == null && to != null) {
                    return Status.ADDED;
                }

                if (from != null && to == null) {
                    return Status.DELETED;
                }

                if ((from == null && to == null)
                    || from.Equals(to)
                ) {
                    return Status.UNCHANGED;
                }

                return Status.MODIFIED;
            }
        }

        public T from;
        public T to;

        public Property() {}
    }
}