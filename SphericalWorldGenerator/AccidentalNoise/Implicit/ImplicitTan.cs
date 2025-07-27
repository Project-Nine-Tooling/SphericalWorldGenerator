using AccidentalNoise;
using System;

namespace SphericalWorldGenerator.AccidentalNoise.Implicit
{
    public sealed class ImplicitTan : ImplicitModuleBase
    {
        public ImplicitTan(ImplicitModuleBase source)
        {
            Source = source;
        }

        public ImplicitModuleBase Source { get; set; }

        public override double Get(double x, double y)
        {
            return System.Math.Tan(Source.Get(x, y));
        }

        public override double Get(double x, double y, double z)
        {
            return System.Math.Tan(Source.Get(x, y, z));
        }

        public override double Get(double x, double y, double z, double w)
        {
            return System.Math.Tan(Source.Get(x, y, z, w));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            return System.Math.Tan(Source.Get(x, y, z, w, u, v));
        }
    }
}
