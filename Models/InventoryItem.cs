using System;

namespace dotdotdot.Models
{
    public class InventoryItem
    {
        public Int32 objectType;
        public string name;
        public WorldObjectRef objectRef;
        public WorldObjectProperty prop;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return objectType.GetHashCode()
                ^ name.GetHashCode()
                ^ objectRef.GetHashCode()
                ^ prop.GetHashCode();
        }
    }
}