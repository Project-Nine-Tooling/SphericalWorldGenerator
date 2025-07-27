using SphericalWorldGenerator.Generators;

namespace ExampleUsage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WrappingWorldGenerator generator = new();
            generator.Start();
            generator.HeightMapRenderer.Data.Save("Output.png");
        }
    }
}
