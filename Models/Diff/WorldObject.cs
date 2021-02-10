using System;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.Diff
{
    public class WorldObject : Model<Models.WorldObject>
    {
        public WorldObjectRef id;
        protected Property<Int32> _type;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> type {
            get {
                return _type == null || _type.status == Status.UNCHANGED
                    ? null
                    : _type;
            }
            set { _type = value; }
        }

        protected Property<string> _typePath;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<string> typePath {
            get {
                return _typePath == null || _typePath.status == Status.UNCHANGED
                    ? null
                    : _typePath;
            }
            set { _typePath = value; }
        }

        protected WorldObjectData _value;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WorldObjectData value {
            get {
                return _value == null || _value.status == Status.UNCHANGED
                    ? null
                    : _value;
            }
            set { _value = value; }
        }

        public WorldObject(Models.WorldObject obj2, Models.WorldObject obj1)
        {
            this.id = obj2 != null ? obj2.id : obj1.id;
            bool obj2Null = obj2 == null;
            bool obj1Null = obj1 == null;

            this._type = new Property<Int32>();
            this._typePath = new Property<string>();
            this._value = new WorldObjectData();

            if (obj2 != null) {
                this._type.to = obj2.type;
                this._typePath.to = obj2.typePath;
                this._value.objTo = obj2.value;
            }

            if (obj1 != null) {
                this._type.from = obj1.type;
                this._typePath.from = obj1.typePath;
                this._value.objFrom = obj1.value;
            }
        }
    }
}