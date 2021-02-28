using System.Text.Json;

namespace DataConverter.OutputConverters
{
    public class JsonOutputConverter : IOutputConverter
    {
        public bool Supports(string outputFormat)
        {
            return outputFormat != null && outputFormat.ToLowerInvariant().StartsWith("json");
        }

        public string Convert(string outputFormat, object data)
        {
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = outputFormat != null && outputFormat.ToLowerInvariant() == "json-pretty"
            };

            return JsonSerializer.Serialize(data, serializerOptions);
        }

        public string GetFileExtension()
        {
            return "json";
        }
    }
}
