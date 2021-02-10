using System;

namespace dotdotdot.Models
{
    public class Color
    {
        public Byte r;
        public Byte g;
        public Byte b;
        public Byte a;

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