using System.IO;
using System.Reflection;

namespace WpfHybridApp.Extensions
{
    public static class AssemblyExtensions
    {
        public static string ReadResourceAsString(this Assembly assembly, string name)
        {
            var fullName = assembly.FindResourceByFragment(name);
            if (string.IsNullOrWhiteSpace(fullName)) return null;

            var stream = assembly.GetManifestResourceStream(fullName);
            if (stream == null) return null;

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static byte[] ReadResourceAsBytes(this Assembly assembly, string name)
        {
            var fullName = assembly.FindResourceByFragment(name);
            if (string.IsNullOrWhiteSpace(fullName)) return null;

            using (var ms = new MemoryStream())
            {
                var stream = assembly.GetManifestResourceStream(fullName);
                if (stream == null) return null;

                stream.CopyTo(ms);
                stream.Close();
                return ms.ToArray();
            }
        }

        public static string FindResourceByFragment(this Assembly assembly, string fragment)
        {
            // The resource path has the following form: [Namespace].[folder].[filename].[fileExtension]
            // This helper will pass the first match. This could lead to unexpected results if there
            // are multiple files of the same type with the same name.
            foreach (var resourceName in assembly.GetManifestResourceNames())
                if (resourceName.EndsWith(fragment))
                    return resourceName;

            return null;
        }
    }
}