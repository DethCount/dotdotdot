using System.Collections.Generic;

namespace dotdotdot.Models.Diff
{
    public class SaveFile : Model<Models.SaveFile>
    {
        public SaveFileHeader header;
        public SaveFileObjects objects;
        // public SaveFileProperties properties;
    }
}