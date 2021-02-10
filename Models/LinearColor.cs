using System;

namespace dotdotdot
{
    public class LinearColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return r.GetHashCode()
                ^ g.GetHashCode()
                ^ b.GetHashCode()
                ^ a.GetHashCode();
        }
    }
}