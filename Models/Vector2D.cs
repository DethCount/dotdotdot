using System;

namespace dotdotdot.Models
{
    public class Vector2D {
        public float x;
        public float y;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return x.GetHashCode()
                ^ y.GetHashCode();
        }
    }
}