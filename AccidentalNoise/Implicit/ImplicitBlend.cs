namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitBlend : ImplicitModuleBase
    {
        public ImplicitBlend(ImplicitModuleBase source, double low, double high)
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
            double v1 = Low.Get(x, y);
            double v2 = High.Get(x, y);
            double blend = (Source.Get(x, y) + 1.0) * 0.5;
            return MathHelper.Lerp(blend, v1, v2);
        }

        public override double Get(double x, double y, double z)
        {
            double v1 = Low.Get(x, y, z);
            double v2 = High.Get(x, y, z);
            double blend = Source.Get(x, y, z);
			return MathHelper.Lerp(blend, v1, v2);
        }

        public override double Get(double x, double y, double z, double w)
        {
            double v1 = Low.Get(x, y, z, w);
            double v2 = High.Get(x, y, z, w);
            double blend = Source.Get(x, y, z, w);
			return MathHelper.Lerp(blend, v1, v2);
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            double v1 = Low.Get(x, y, z, w, u, v);
            double v2 = High.Get(x, y, z, w, u, v);
            double blend = Source.Get(x, y, z, w, u, v);
			return MathHelper.Lerp(blend, v1, v2);
        }
    }
}
