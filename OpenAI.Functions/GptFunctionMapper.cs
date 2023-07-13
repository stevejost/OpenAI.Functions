using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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
            var parameters = method.GetParameters().Where(p => p.GetCustomAttributes(typeof(Attributes.GptFunctionParameterAttribute), false).Length > 0);

            function.Name = method.Name;
            function.Description = method.GetCustomAttribute<Attributes.GptFunctionAttribute>().GetDescription();
            function.Required = GetRequiredParametersFromMethod(method);
            foreach(var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;
                var parameterName = parameter.Name;
                var parameterDescription = parameter.GetCustomAttribute<Attributes.GptFunctionParameterAttribute>().GetDescription();

                function.Parameters.Properties.Add(parameterName, new GptParameterProperties()
                {                    
                    Description = parameterDescription,
                    Type = MapValueTypesToName(parameterType)
                });
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
            var parameters = method.GetParameters();
            var required = new List<string>();

            foreach(var parameter in parameters)
            {
                if (!parameter.IsOptional)
                {
                    required.Add(parameter.Name);
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

        
       

    }
}
