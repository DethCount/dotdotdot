using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.View.SaveFile
{
    public class List
    {
        public string basepath;
        public List<ListItem> files;
    }
}