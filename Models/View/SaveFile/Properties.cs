using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.View.SaveFile
{
    public class Properties
    {
        public string basepath;
        public string filename;
        [JsonIgnore]
        public SaveFileProperties _properties;
        public Int32 count
        {
            get { return _properties.count; }
            set { _properties.count = value; }
        }
        public List<WorldObjectProperties> properties
        {
            get { return _properties.properties; }
            set { _properties.properties = value; }
        }
    }
}