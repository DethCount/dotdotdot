using System;
using System.Collections.Generic;

namespace dotdotdot.Models
{
    public class WorldObjectTextProperty
    {
        public Int32 flags;
        public Byte historyType;
        public string ns; // namespace
        public string key;
        public string value;
        public WorldObjectTextProperty sourceFmt;
        public Int32 argsCount;
        public List<WorldObjectTextPropertyArgument> args;
    }
}