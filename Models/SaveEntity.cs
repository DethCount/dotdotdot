using System;

namespace dotdotdot.Models
{
    public class SaveEntity : SaveObject
    {
        public bool needTransform;
        public ValueTuple<Single, Single, Single, Single> rotation;
        public ValueTuple<Single, Single, Single> position;
        public ValueTuple<Single, Single, Single> scale;
        public bool wasPlacedInLevel;
        public string parentObjectRoot;
        public string parentObjectName;
    }
}