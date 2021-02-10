using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

using dotdotdot.Services;

namespace dotdotdot.Models.Diff
{
    public class Model<T>
    {
        [JsonConverter(typeof(DiffStatusJsonConverter))]
        public Status status {
            get {
                Type t = this.GetType();

                foreach (FieldInfo field in this.GetType().GetFields()) {
                    if (this.fieldChanged(field)) {
                        return Status.MODIFIED;
                    }
                }

                foreach (PropertyInfo property in this.GetType().GetProperties()) {
                    if (this.propertyChanged(property)) {
                        return Status.MODIFIED;
                    }
                }

                return Status.UNCHANGED;
            }
        }

        protected bool fieldChanged(FieldInfo field)
        {
            PropertyInfo statusProp = field.FieldType.GetProperty("status");
            if (statusProp != null
                && statusProp.PropertyType == typeof(Status)
            ) {
                object val = field.GetValue(this);

                if (val != null) {
                    Status propStatus = (Status) statusProp.GetValue(val);
                    if (propStatus != Status.UNCHANGED) {
                        return true;
                    }
                }
            }

            return false;
        }

        protected bool propertyChanged(PropertyInfo property)
        {
            PropertyInfo statusProp = property.PropertyType.GetProperty("status");
            if (statusProp != null
                && statusProp.PropertyType == typeof(Status)
            ) {
                object val = property.GetValue(this);

                if (val != null) {
                    Status propStatus = (Status) statusProp.GetValue(val);
                    if (propStatus != Status.UNCHANGED) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}