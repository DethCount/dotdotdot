using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{ 
    public class SaveFile
    {
        public string filepath;
        public Int32 saveHeaderVersion;
        public Int32 saveVersion;
        public Int32 buildVersion;
        public string worldType;
        public string worldProperties;
        public string sessionName;
        public Int32 playTime; // seconds
        public DateTime saveDate; // number of ticks
        public Byte sessionVisibility;
        public Int32 worldObjectLength;
        public Int32 worldObjectCount;
        public WorldObject[] worldObjects;
        public Int32 worldObjectPropertyCount;
    }
}