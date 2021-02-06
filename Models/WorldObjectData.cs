using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObjectData
    {
        public string type;
        public string parentEntityName;
        public bool needTransform;
        public ValueTuple<Single, Single, Single, Single> rotation;
        public ValueTuple<Single, Single, Single> position;
        public ValueTuple<Single, Single, Single> scale;
        public bool wasPlacedInLevel;
    }
}