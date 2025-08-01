﻿namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitClamp : ImplicitModuleBase
    {
        public ImplicitClamp(ImplicitModuleBase source, double low, double high)
        {
            Source = source;
            Low = new ImplicitConstant(low);
            High = new ImplicitConstant(high);
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase Low { get; set; }

        public ImplicitModuleBase High { get; set; }

        public override double Get(double x, double y)
        {            
			return MathHelper.Clamp(Source.Get(x, y), Low.Get(x, y), High.Get(x, y));
        }

        public override double Get(double x, double y, double z)
        {
			return MathHelper.Clamp(Source.Get(x, y, z), Low.Get(x, y, z), High.Get(x, y, z));
        }

        public override double Get(double x, double y, double z, double w)
        {
			return MathHelper.Clamp(Source.Get(x, y, z, w), Low.Get(x, y, z, w), High.Get(x, y, z, w));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
			return MathHelper.Clamp(Source.Get(x, y, z, w, u, v), Low.Get(x, y, z, w, u, v), High.Get(x, y, z, w, u, v));
        }
    }
}
