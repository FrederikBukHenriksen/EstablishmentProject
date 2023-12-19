using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApplication1
{

        public class PolymorphicJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                // This converter can handle any type.
                return true;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jsonObject = JObject.Load(reader);

                // Extract the $type field from the JSON.
                JToken typeToken = jsonObject["$type"];
                if (typeToken == null || typeToken.Type != JTokenType.String)
                {
                    throw new JsonSerializationException("'$type' field not found or not a string.");
                }

                // Resolve the type based on the $type field value.
                string typeName = typeToken.Value<string>();
                Type targetType = Type.GetType(typeName);
                if (targetType == null)
                {
                    throw new JsonSerializationException($"Type specified in JSON '{typeName}' was not resolved.");
                }

                // Deserialize the object using the resolved type.
                return jsonObject.ToObject(targetType, serializer);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

    }

