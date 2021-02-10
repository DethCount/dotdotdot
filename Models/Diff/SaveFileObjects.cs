using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.Diff
{
    public class SaveFileObjects
    {
        protected Property<Int32> _size;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> size {
            get {
                return _size == null || _size.status == Status.UNCHANGED
                    ? null
                    : _size;
            }
            set { _size = value; }
        }

        protected Property<Int32> _count;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> count {
            get {
                return _count == null || _count.status == Status.UNCHANGED
                    ? null
                    : _count;
            }
            set { _count = value; }
        }

        protected DiffList<WorldObject,Models.WorldObject> _objects;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DiffList<WorldObject,Models.WorldObject> objects {
            get {
                return _objects == null || _objects.status == Status.UNCHANGED
                    ? null
                    : _objects;
            }
            set { _objects = value; }
        }

        public void AddObject(WorldObject obj)
        {
            this._objects.Add(obj);
        }
    }
}