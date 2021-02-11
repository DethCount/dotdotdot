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
                    Status s = this.fieldChanged(field);
                    if (s != Status.UNCHANGED) {
                        return s;
                    }
                }

                foreach (PropertyInfo property in this.GetType().GetProperties()) {
                    Status s = this.propertyChanged(property);
                    if (s != Status.UNCHANGED) {
                        return s;
                    }
                }

                return Status.UNCHANGED;
            }
        }

        protected Status fieldChanged(FieldInfo field)
        {
            PropertyInfo statusProp = field.FieldType.GetProperty("status");
            if (statusProp != null
                && statusProp.PropertyType == typeof(Status)
            ) {
                object val = field.GetValue(this);

                if (val != null) {
                    Status propStatus = (Status) statusProp.GetValue(val);
                    if (propStatus != Status.UNCHANGED) {
                        return propStatus;
                    }
                }
            }

            return Status.UNCHANGED;
        }

        protected Status propertyChanged(PropertyInfo property)
        {
            PropertyInfo statusProp = property.PropertyType.GetProperty("status");
            if (statusProp != null
                && statusProp.PropertyType == typeof(Status)
            ) {
                object val = property.GetValue(this);

                if (val != null) {
                    Status propStatus = (Status) statusProp.GetValue(val);
                    if (propStatus != Status.UNCHANGED) {
                        return propStatus;
                    }
                }
            }

            return Status.UNCHANGED;
        }
    }
}