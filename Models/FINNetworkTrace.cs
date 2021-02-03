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
    }
}