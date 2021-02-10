using System;

namespace dotdotdot.Models
{
    public class NamedWorldObjectProperty
    {
        public string name;
        public WorldObjectProperty value;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return name.GetHashCode()
                ^ value.GetHashCode();
        }
    }
}