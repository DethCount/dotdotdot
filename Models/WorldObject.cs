using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObject
    {
        public Int32 type;
        public string typePath;
        public WorldObjectRef id;
        public WorldObjectData value;
        public Int32 propertiesLength;
        public List<WorldObjectProperty> properties;
    }
}