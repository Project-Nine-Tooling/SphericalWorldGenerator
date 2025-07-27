using SphericalWorldGenerator.Generators;

namespace ExampleUsage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WrappingWorldGenerator generator = new();
            generator.Start();
            generator.GetHeightMap().Data.Save("Output_Height.png");
            generator.BiomeMapRenderer.Data.Save("Output_Biome.png");
        }
    }
}
