using System;

namespace dotdotdot.Models
{
    public class FINNetworkTrace
    {
        public bool isValid;
        public WorldObjectRef objectRef;
        public bool hasPrev;
        public FINNetworkTrace prev;
        public bool hasStep;
        public string step;

        public override bool Equals(object obj)
        {
            return null != obj
                && obj.GetType().Equals(obj.GetType())
                && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return isValid.GetHashCode()
                ^ objectRef.GetHashCode()
                ^ hasPrev.GetHashCode()
                ^ prev.GetHashCode()
                ^ hasStep.GetHashCode()
                ^ step.GetHashCode();
        }
    }
}