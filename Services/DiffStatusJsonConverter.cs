using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using dotdotdot.Models.Diff;

namespace dotdotdot.Services
{
    public class DiffStatusJsonConverter : JsonConverter<Status>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override Status Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) {
            throw new NotSupportedException();
        }

        public override void Write(
            Utf8JsonWriter writer,
            Status field,
            JsonSerializerOptions options
        ) {
            switch (field) {
                case Status.UNCHANGED:
                    writer.WriteStringValue("unchanged");
                    break;
                case Status.ADDED:
                    writer.WriteStringValue("added");
                    break;
                case Status.DELETED:
                    writer.WriteStringValue("deleted");
                    break;
                case Status.MODIFIED:
                    writer.WriteStringValue("modified");
                    break;
            }
        }
    }
}