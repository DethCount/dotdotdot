using System;
using System.Text.Json.Serialization;

namespace dotdotdot.Models.Diff
{
    public class SaveFileHeader : Model<Models.SaveFileHeader>
    {
        protected Property<Int32> _saveHeaderVersion;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> saveHeaderVersion {
            get {
                return _saveHeaderVersion == null || _saveHeaderVersion.status == Status.UNCHANGED
                    ? null
                    : _saveHeaderVersion;
            }
            set { _saveHeaderVersion = value; }
        }

        protected Property<Int32> _saveVersion;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> saveVersion {
            get {
                return _saveVersion == null || _saveVersion.status == Status.UNCHANGED
                    ? null
                    : _saveVersion;
            }
            set { _saveVersion = value; }
        }

        protected Property<Int32> _buildVersion;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> buildVersion {
            get {
                return _buildVersion == null || _buildVersion.status == Status.UNCHANGED
                    ? null
                    : _buildVersion;
            }
            set { _buildVersion = value; }
        }

        protected Property<string> _worldType;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<string> worldType {
            get {
                return _worldType == null || _worldType.status == Status.UNCHANGED
                    ? null
                    : _worldType;
            }
            set { _worldType = value; }
        }

        protected Property<string> _worldProperties;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<string> worldProperties {
            get {
                return _worldProperties == null || _worldProperties.status == Status.UNCHANGED
                    ? null
                    : _worldProperties;
            }
            set { _worldProperties = value; }
        }

        protected Property<string> _sessionName;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<string> sessionName {
            get {
                return _sessionName == null || _sessionName.status == Status.UNCHANGED
                    ? null
                    : _sessionName;
            }
            set { _sessionName = value; }
        }

        protected Property<Int32> _playTime; // seconds
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Int32> playTime {
            get {
                return _playTime == null || _playTime.status == Status.UNCHANGED
                    ? null
                    : _playTime;
            }
            set { _playTime = value; }
        }

        protected Property<DateTime> _saveDate;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<DateTime> saveDate {
            get {
                return _saveDate == null || _saveDate.status == Status.UNCHANGED
                    ? null
                    : _saveDate;
            }
            set { _saveDate = value; }
        }

        protected Property<Byte> _sessionVisibility;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Property<Byte> sessionVisibility {
            get {
                return _sessionVisibility == null || _sessionVisibility.status == Status.UNCHANGED
                    ? null
                    : _sessionVisibility;
            }
            set { _sessionVisibility = value; }
        }
    }
}