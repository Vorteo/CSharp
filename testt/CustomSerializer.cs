using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
    public class CustomSerializer
    {
        public static void Serialize(object value)
        {
            Type type = value.GetType();

            var sb = new StringBuilder();
            foreach (var property in type.GetProperties())
            {
                object o = property.GetValue(value);

                sb.AppendLine($"{property.Name}:{o.ToString()}");
            }
            Random random = new Random();
            File.WriteAllTextAsync($"Exchange{random.Next()}.txt",sb.ToString());
        }
    }
}
