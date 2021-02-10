using System;

namespace dotdotdot.Models
{
    public class RailroadTrackPosition
    {
        public WorldObjectRef objectRef;
        public float offset;
        public float forward;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return objectRef.GetHashCode()
                ^ offset.GetHashCode()
                ^ forward.GetHashCode();
        }
    }
}