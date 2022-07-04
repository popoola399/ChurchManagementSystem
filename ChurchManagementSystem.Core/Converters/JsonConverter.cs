using ChurchManagementSystem.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChurchManagementSystem.Core.Converters
{
    public class MaskJsonConverter : JsonConverter
    {
        private readonly string _pattern;
        private readonly string _mask;

        public MaskJsonConverter(string pattern, string mask)
        {
            _pattern = pattern;
            _mask = mask;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stringValue = (string)value;
            var newValue = new Regex(_pattern).Replace(stringValue, _mask);

            var token = JToken.FromObject(newValue);
            token.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override bool CanRead => false;
    }

    public class SsnMaskJsonConverter : MaskJsonConverter
    {
        public SsnMaskJsonConverter()
            : base(Constants.RegExPatterns.Ssn, Constants.RegExPatterns.SsnMask)
        {
        }
    }

    public class PhiMaskJsonConverter : MaskJsonConverter
    {
        private readonly string[] _phiPropertyNames =
        {
            "SSN",
            "SOCIALSECURITYNUMBER"
        };

        public PhiMaskJsonConverter()
            : base(Constants.RegExPatterns.Ssn, Constants.RegExPatterns.SsnMask)
        {
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer.WriteState == WriteState.Property)
            {
                var propertyNames = writer.Path.Split('.');
                var mostDerivedName = propertyNames[propertyNames.Length - 1].ToUpper();

                if (_phiPropertyNames.Contains(mostDerivedName))
                {
                    base.WriteJson(writer, value, serializer);
                    return;
                }
            }

            var token = JToken.FromObject(value);
            token.WriteTo(writer);
        }
    }
}