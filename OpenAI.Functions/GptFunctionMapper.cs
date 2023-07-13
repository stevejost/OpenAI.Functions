using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenAI.Functions.Attributes;
using OpenAI.Functions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.Functions
{
    public class GptFunctionMapper
    {
        public IEnumerable<MethodInfo> GetAnnotatedMethodsFromType(Type t)
        {
            var methodList = t.GetMethods().Where(m => m.GetCustomAttributes(typeof(Attributes.GptFunctionAttribute), false).Length > 0);

            return methodList;
        }

        public string MapValueTypesToName(Type type)
        {
            // Maps basic value types to their string name
            if (type == typeof(string)) return "string";
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(double)) return "double";
            if (type == typeof(bool)) return "bool";
            if (type == typeof(char)) return "char";
            if (type == typeof(byte)) return "byte";
            if (type == typeof(sbyte)) return "sbyte";
            if (type == typeof(short)) return "short";
            if (type == typeof(ushort)) return "ushort";
            if (type == typeof(uint)) return "uint";
            if (type == typeof(long)) return "long";
            if (type == typeof(ulong)) return "ulong";
            if (type == typeof(decimal)) return "decimal";
            if(type == typeof(DateTime)) return "datetime";

            throw new ArgumentException("Type is not valid for conversion", nameof(type));
        }

        public GptFunction GetFunctionFromMethod(MethodInfo method)
        {
            var function = new GptFunction();
            // get annotated parameters
            var parameters = method.GetParameters().Where(p => p.ParameterType.IsSubclassOf(typeof(GptRequestArguments)));

            function.Name = method.Name;
            function.Description = method.GetCustomAttribute<Attributes.GptFunctionAttribute>().GetDescription();
            function.Required = GetRequiredParametersFromMethod(method);
            foreach(var parameter in parameters)
            {
                // find parameters with GptRequestArguments as the base class
                if (parameter.ParameterType.IsSubclassOf(typeof(GptRequestArguments)))
                {
                    var properties = new Dictionary<string, GptParameterProperties>();
                    var propertiesType = parameter.ParameterType.GetProperties();

                    foreach(var property in propertiesType)
                    {
                        var propertyType = property.PropertyType;
                        var propertyTypeName = MapValueTypesToName(propertyType);
                        var propertyDescription = property.GetCustomAttribute<Attributes.GptParameterAttribute>().GetDescription();

                        properties.Add(property.Name, new GptParameterProperties
                        {
                            Type = propertyTypeName,
                            Description = propertyDescription
                        });
                    }

                    function.Parameters.Properties = properties;
                }
            }

            return function;
        }


        public List<GptFunction> GetFunctionsFromType(Type t)
        {
            var functions = new List<GptFunction>();
            var methodList = GetAnnotatedMethodsFromType(t);

            foreach(var method in methodList)
            {
                functions.Add(GetFunctionFromMethod(method));
            }

            return functions;
        }

        public List<string> GetRequiredParametersFromMethod(MethodInfo method)
        {
            var parameters = method.GetParameters().Where(p => p.ParameterType.IsSubclassOf(typeof(GptRequestArguments)));
            var required = new List<string>();



            foreach(var parameter in parameters)
            {
                //get properties that are GptParameterAttributes
                var properties = parameter.ParameterType.GetProperties().Where(p => p.GetCustomAttribute<Attributes.GptParameterAttribute>() != null);
                foreach(var prop in properties)
                {
                    // determine if prop is nullable
                    var isNullable = Nullable.GetUnderlyingType(prop.PropertyType) != null;
                    if(!isNullable)
                    {
                        required.Add(prop.Name);
                    }
                }
            }

            return required;
        }

        public string GetFunctionJsonFromType(Type t)
        {
            var functions = this.GetFunctionsFromType(t);

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(functions, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });

            return json;
        }

        public string GetChatResponseFromMethod(Type type, string methodName, string jsonArguments)
        {                        
            var method = type.GetMethod(methodName);
            var methodParams = method.GetParameters();
            var paramType = methodParams[0].ParameterType;
            var deserializedData = JsonConvert.DeserializeObject(jsonArguments, paramType);
            var result = (string)method.Invoke(type, new object[] { deserializedData });

            return result;
        }

        public Type FindTypeByMethodName(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    // check if the type has the SearchableClass attribute
                    if (Attribute.IsDefined(type, typeof(GptClassAttribute)))
                    {
                        var methods = type.GetMethods();

                        foreach (var method in methods)
                        {
                            if (method.Name == name)
                            {
                                // Return the type instead of the method
                                return type;
                            }
                        }
                    }
                }
            }

            // Return null if no matching type is found
            return null;
        }

    }
}
