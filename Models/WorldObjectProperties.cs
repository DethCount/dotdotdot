using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObjectProperties
    {
        public Int32 size;
        public WorldObjectRef parentId;
        public Int32 childrenCount;
        public List<WorldObjectRef> children;
        public List<WorldObjectProperty> properties;
    }
}