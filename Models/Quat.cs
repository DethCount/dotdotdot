using System;

namespace dotdotdot.Models
{
    public class Quat
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return x.GetHashCode()
                ^ y.GetHashCode()
                ^ z.GetHashCode()
                ^ w.GetHashCode();
        }
    }
}