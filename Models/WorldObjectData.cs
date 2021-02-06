using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObjectData
    {
        public string type;
        public string parentEntityName;
        public bool needTransform;
        public Quat rotation;
        public Vector position;
        public Vector scale;
        public bool wasPlacedInLevel;
    }
}