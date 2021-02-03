using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class SaveEntity : WorldObjectData
    {
        public bool needTransform;
        public ValueTuple<Single, Single, Single, Single> rotation;
        public ValueTuple<Single, Single, Single> position;
        public ValueTuple<Single, Single, Single> scale;
        public bool wasPlacedInLevel;
        public WorldObjectRef parentId;
        public Int32 childrenCount;
        public List<WorldObjectRef> children;
    }
}