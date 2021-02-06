using System.Text.Json.Serialization;

namespace dotdotdot.Models.View.SaveFile
{
    public class Read
    {
        [JsonInclude]
        public string basepath;
        [JsonInclude]
        public string filename;
        [JsonInclude]
        public Models.SaveFile file;
    }
}