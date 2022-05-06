using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bogus.Extensions.UnitedStates;
using SqlServerDEID.Editor.Extensions;

namespace SqlServerDEID.Editor
{
    public partial class Form1 : Form
    {
        private readonly StringComparer _stringComparer = StringComparer.InvariantCultureIgnoreCase;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var assembly = typeof(Bogus.Faker).Assembly;
            var allTypes = assembly.GetTypes();
            var types = allTypes.Where(t => t.IsClass && _stringComparer.Equals(t.Namespace, "Bogus.DataSets")).ToList();
            types.Add(typeof(Bogus.Randomizer));

            var methods = types
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.IsSpecialName)
                .ToList();

            var extenstionTypes = allTypes.Where(t => t.IsClass && _stringComparer.Equals(t.Namespace, "Bogus.Extensions.UnitedStates")).ToList();

            var extensionMethods = extenstionTypes
                .SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static))
                .Where(m => !m.IsSpecialName)
                .ToList();

            var fakerMethods = new SortedDictionary<string, List<string>>
            {
                { "Person", new List<string> { "Faker.Person.Ssn()" } },
                { "Company", new List<string> { "Faker.Company.Ein()" } }
            };

            foreach (var method in methods)
            {
                //these do not match the property names off the faker object
                var typeName = method.DeclaringType.Name
                    .Replace("Randomizer", "Random")
                    .Replace("PhoneNumbers", "Phone");

                if (!fakerMethods.ContainsKey(typeName)) { fakerMethods.Add(typeName, new List<string>()); }

                var parameters = method.GetParameters();
                var paramList = new List<IList<string>>();
                if (parameters.Any())
                {
                    foreach (var p in parameters)
                    {
                        paramList.Add(GetParameterTypeName(p.Name, p.ParameterType));
                    }
                    //produce a cartesian product of all the parameters found
                    var products = paramList.CartesianProduct();
                    // now add the overloads into the faker methods
                    foreach (var product in products)
                    {
                        var paramString = string.Join(", ", product.Where(p => !string.IsNullOrWhiteSpace(p)));
                        fakerMethods[typeName].Add($"Faker.{typeName}.{method.Name}({paramString})");
                    }
                }
                else
                {
                    fakerMethods[typeName].Add($"Faker.{typeName}.{method.Name}()");
                }
            }
        }

        private IList<string> GetParameterTypeName(string name, Type type)
        {
            var ret = new List<string>();
            if (_stringComparer.Equals(type.Name, "Nullable`1"))
            {
                ret.Add(String.Empty);
                ret.AddRange(GetParameterTypeName(name, type.GenericTypeArguments[0]));
            }
            else if (type.IsEnum)
            {
                var typeName = type.FullName.Replace("+", ".");
                var values = Enum.GetNames(type).Select(e => $"{typeName}.{e}");
                ret.AddRange(values);
            }
            else
            {
                ret.Add($"{type.Name} {name}");
            }
            return ret;
        }
    }
}
