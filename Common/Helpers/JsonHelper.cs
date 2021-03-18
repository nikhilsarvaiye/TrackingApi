namespace Common
{
    using Newtonsoft.Json;
    
    public static class JsonHelper
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T Deserialize<T>(this object value)
        {
            return value.ToJson().DeserializeJson<T>();
        }
    }
}
