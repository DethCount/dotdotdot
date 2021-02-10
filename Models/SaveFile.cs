using System;
using System.Text.Json.Serialization;

namespace dotdotdot.Models
{
    public class SaveFile
    {
        public SaveFileHeader header;
        public SaveFileObjects objects;
        public SaveFileProperties properties;
    }
}