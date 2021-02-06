using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObject
    {
        public WorldObjectRef id;
        public Int32 type;
        public string typePath;
        public Int32 propertiesLength;
        public WorldObjectData value;
    }
}