using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObjectArrayProperty
    {
        public string type;
        public long length;
        public WorldObjectProperty property;
        public List<object> values;
    }
}