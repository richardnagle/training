using System;

namespace reviews_service
{
    public static class Program
    {
        public static void Main(params string[] args)
        {
            var header = args[0];
            var body = args[1];

            Console.WriteLine("Header");
            Console.WriteLine(header);
            Console.WriteLine();
            Console.WriteLine("Body");
            Console.WriteLine(body);
        }
    }
}