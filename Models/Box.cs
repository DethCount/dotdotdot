using System;

namespace dotdotdot.Models
{
    public class Box
    {
        public Vector min;
        public Vector max;
        public bool isValid;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return min.GetHashCode()
                ^ max.GetHashCode()
                ^ isValid.GetHashCode();
        }
    }
}