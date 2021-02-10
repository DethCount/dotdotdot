using System;

namespace dotdotdot.Models
{
    public class FluidBox
    {
        public float value;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}