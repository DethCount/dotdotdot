using System;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.Diff
{
    public class WorldObjectData : Model<Models.WorldObjectData>
    {
        protected Property<string> _type;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<string> type {
            get {
                return _type == null || _type.status == Status.UNCHANGED
                    ? null
                    : _type;
            }
            set { _type = value; }
        }
        protected Property<string> _parentEntityName;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<string> parentEntityName {
            get {
                return _parentEntityName == null || _parentEntityName.status == Status.UNCHANGED
                    ? null
                    : _parentEntityName;
            }
            set { _parentEntityName = value; }
        }

        protected Property<bool> _needTransform;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<bool> needTransform {
            get {
                return _needTransform == null || _needTransform.status == Status.UNCHANGED
                    ? null
                    : _needTransform;
            }
            set { _needTransform = value; }
        }

        protected Property<Quat> _rotation;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Quat> rotation {
            get {
                return _rotation == null || _rotation.status == Status.UNCHANGED
                    ? null
                    : _rotation;
            }
            set { _rotation = value; }
        }

        protected Property<Vector> _position;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Vector> position {
            get {
                return _position == null || _position.status == Status.UNCHANGED
                    ? null
                    : _position;
            }
            set { _position = value; }
        }

        protected Property<Vector> _scale;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Vector> scale {
            get {
                return _scale == null || _scale.status == Status.UNCHANGED
                    ? null
                    : _scale;
            }
            set { _scale = value; }
        }

        protected Property<bool> _wasPlacedInLevel;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<bool> wasPlacedInLevel {
            get {
                return _wasPlacedInLevel == null || _wasPlacedInLevel.status == Status.UNCHANGED
                    ? null
                    : _wasPlacedInLevel;
            }
            set { _wasPlacedInLevel = value; }
        }

        protected void _initProperties()
        {
            if (this._type == null) {
                this._type = new Property<string>();
                this._parentEntityName = new Property<string>();
                this._needTransform = new Property<bool>();
                this._rotation = new Property<Quat>();
                this._position = new Property<Vector>();
                this._scale = new Property<Vector>();
                this._wasPlacedInLevel = new Property<bool>();
            }
        }

        public Models.WorldObjectData objTo {
            set {
                _initProperties();

                if (value == null) {
                    return;
                }

                this._type.to = value.type;
                this._parentEntityName.to = value.parentEntityName;
                this._needTransform.to = value.needTransform;
                this._rotation.to = value.rotation;
                this._position.to = value.position;
                this._scale.to = value.scale;
                this._wasPlacedInLevel.to = value.wasPlacedInLevel;
            }
        }

        public Models.WorldObjectData objFrom {
            set {
                _initProperties();

                if (value == null) {
                    return;
                }

                this._type.from = value.type;
                this._parentEntityName.from = value.parentEntityName;
                this._needTransform.from = value.needTransform;
                this._rotation.from = value.rotation;
                this._position.from = value.position;
                this._scale.from = value.scale;
                this._wasPlacedInLevel.from = value.wasPlacedInLevel;
            }
        }
    }
}