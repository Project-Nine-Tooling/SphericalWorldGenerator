using System;

namespace AccidentalNoise
{
    public sealed class ImplicitBlend : ImplicitModuleBase
    {
        public ImplicitBlend(ImplicitModuleBase source, Double low, Double high)
        {
            this.Source = source;
            this.Low = new ImplicitConstant(low);
            this.High = new ImplicitConstant(high);
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase Low { get; set; }

        public ImplicitModuleBase High { get; set; }

        public override Double Get(Double x, Double y)
        {
            double v1 = this.Low.Get(x, y);
            double v2 = this.High.Get(x, y);
            double blend = (this.Source.Get(x, y) + 1.0) * 0.5;
            return MathHelper.Lerp(blend, v1, v2);
        }

        public override Double Get(Double x, Double y, Double z)
        {
            double v1 = this.Low.Get(x, y, z);
            double v2 = this.High.Get(x, y, z);
            double blend = this.Source.Get(x, y, z);
			return MathHelper.Lerp(blend, v1, v2);
        }

        public override Double Get(Double x, Double y, Double z, Double w)
        {
            double v1 = this.Low.Get(x, y, z, w);
            double v2 = this.High.Get(x, y, z, w);
            double blend = this.Source.Get(x, y, z, w);
			return MathHelper.Lerp(blend, v1, v2);
        }

        public override Double Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double v1 = this.Low.Get(x, y, z, w, u, v);
            double v2 = this.High.Get(x, y, z, w, u, v);
            double blend = this.Source.Get(x, y, z, w, u, v);
			return MathHelper.Lerp(blend, v1, v2);
        }
    }
}
