using System;
using System.Linq;
using System.Runtime.InteropServices;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

namespace WpfHybridApp.Services
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class SampleService : ISampleService
    {
        // Sample property.
        public string Prop { get; set; } = "BridgeAddRemoteObject.Prop";

        public string GetCurrentDate()
        {
            return DateTime.Now.ToString("F");
        }

        public string DiscoverMessages()
        {
            var generator = new JSchemaGenerator();

            // change contract resolver so property names are camel case
            generator.ContractResolver = new CamelCasePropertyNamesContractResolver();
            generator.SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName;
            //JSchema schema = generator.Generate(typeof(Person));
            
            var baseType = typeof(IBaseRequest);
            var types = GetType().Assembly.GetTypes()
                .Where(p => baseType.IsAssignableFrom(p))
                .Select(type => generator.Generate(type)).ToList();

            return JsonConvert.SerializeObject(types);
        }
    }
}