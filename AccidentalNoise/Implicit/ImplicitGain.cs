﻿namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitGain : ImplicitModuleBase
    {
        public ImplicitGain(ImplicitModuleBase source, double gain)
        {
            Source = source;
            Gain = new ImplicitConstant(gain);
        }

        public ImplicitGain(ImplicitModuleBase source, ImplicitModuleBase gain)
        {
            Source = source;
            Gain = gain;
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase Gain { get; set; }

        public override double Get(double x, double y)
        {
			return MathHelper.Gain(Gain.Get(x, y), Source.Get(x, y));
        }

        public override double Get(double x, double y, double z)
        {
			return MathHelper.Gain(Gain.Get(x, y, z), Source.Get(x, y, z));
        }

        public override double Get(double x, double y, double z, double w)
        {
			return MathHelper.Gain(Gain.Get(x, y, z, w), Source.Get(x, y, z, w));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
			return MathHelper.Gain(Gain.Get(x, y, z, w, u, v), Source.Get(x, y, z, w, u, v));
        }
    }
}
