using System;
using System.IO;

namespace CSharp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: csharp <filename>");
                return 1;
            }

            try
            {
                var content = ReadFile(args[0]);
                Console.WriteLine($"Got: {content}");
                return 0;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File {args[0]} was not found.");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return 1;
            }
        }

        private static string ReadFile(string filename)
        {
            return ReadFile2(filename);
        }

        private static string ReadFile2(string filename)
        {
            return ReadFile3(filename);
        }

        private static string ReadFile3(string filename)
        {
            return File.ReadAllText(filename);
        }
    }
}
