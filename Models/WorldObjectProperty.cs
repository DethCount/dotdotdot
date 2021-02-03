using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObjectProperty
    {
        public string type;
        public string name;
        public Int32 size;
        public Int32 index;
        public object value;
        public bool isMultiple = false;
        public List<object> values;
    }
}