using System;

namespace dotdotdot.Models.View.SaveFile
{
    public class ListItem
    {
        public string filename;
        public string filepath;
        public DateTime lastModified;

        public ListItem(
            string filename, 
            string filepath, 
            DateTime lastModified
        ) {
            this.filename = filename;
            this.filepath = filepath;
            this.lastModified = lastModified;
        }
    }
}