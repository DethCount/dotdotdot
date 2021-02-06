using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.View.SaveFile
{
    public class Objects
    {
        public string basepath;
        public string filename;
        [JsonIgnore]
        public SaveFileObjects _objects;
        public Int32 size
        {
            get { return _objects.size; }
            set { _objects.size = value; }
        }
        public Int32 count
        {
            get { return _objects.count; }
            set { _objects.count = value; }
        }
        public List<WorldObject> objects
        {
            get { return _objects.objects; }
            set { _objects.objects = value; }
        }
    }
}