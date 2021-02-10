using System;

namespace dotdotdot.Models
{
    public class WorldObjectRef
    {
        public string rootObject;
        public string instanceName;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.rootObject.GetHashCode()
                ^ this.instanceName.GetHashCode();
        }
    }
}