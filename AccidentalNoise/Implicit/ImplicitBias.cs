﻿namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitBias : ImplicitModuleBase
    {
        public ImplicitBias(ImplicitModuleBase source, double bias)
        {
            Source = source;
            Bias = new ImplicitConstant(bias);
        }

        public ImplicitBias(ImplicitModuleBase source, ImplicitModuleBase bias)
        {
            Source = source;
            Bias = bias;
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase Bias { get; set; }

        public override double Get(double x, double y)
        {
			return MathHelper.Bias(Bias.Get(x, y), Source.Get(x, y));
        }

        public override double Get(double x, double y, double z)
        {
			return MathHelper.Bias(Bias.Get(x, y, z), Source.Get(x, y, z));
        }

        public override double Get(double x, double y, double z, double w)
        {
			return MathHelper.Bias(Bias.Get(x, y, z, w), Source.Get(x, y, z, w));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
			return MathHelper.Bias(Bias.Get(x, y, z, w, u, v), Source.Get(x, y, z, w, u, v));
        }
    }
}