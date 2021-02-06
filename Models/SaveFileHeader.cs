using System;

namespace dotdotdot.Models
{
    public class SaveFileHeader
    {
        public Int32 saveHeaderVersion;
        public Int32 saveVersion;
        public Int32 buildVersion;
        public string worldType;
        public string worldProperties;
        public string sessionName;
        public Int32 playTime; // seconds
        public DateTime saveDate; // number of ticks
        public Byte sessionVisibility;
    }
}