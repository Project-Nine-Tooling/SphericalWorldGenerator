using System;

namespace AccidentalNoise
{
    public sealed class ImplicitSelect : ImplicitModuleBase
    {
        public ImplicitSelect(ImplicitModuleBase source, ImplicitModuleBase low, ImplicitModuleBase high, Double falloff, Double threshold)
        {
            this.Source = source;
            this.Low = low;
            this.High = high;
            this.Falloff = new ImplicitConstant(falloff);
            this.Threshold = new ImplicitConstant(threshold);
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase Low { get; set; }

        public ImplicitModuleBase High { get; set; }

        public ImplicitModuleBase Threshold { get; set; }

        public ImplicitModuleBase Falloff { get; set; }

        public override Double Get(Double x, Double y)
        {
            double value = this.Source.Get(x, y);
            double falloff = this.Falloff.Get(x, y);
            double threshold = this.Threshold.Get(x, y);

            if (falloff > 0.0)
            {
                if (value < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return this.Low.Get(x, y);
                }
                if (value > (threshold + falloff))
                {
                    // Lies outside of falloff area above threshold, return second source
                    return this.High.Get(x, y);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, this.Low.Get(x, y), this.High.Get(x, y));
            }

            return (value < threshold ? this.Low.Get(x, y) : this.High.Get(x, y));
        }

        public override Double Get(Double x, Double y, Double z)
        {
            double value = this.Source.Get(x, y, z);
            double falloff = this.Falloff.Get(x, y, z);
            double threshold = this.Threshold.Get(x, y, z);

            if (falloff > 0.0)
            {
                if (value < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return this.Low.Get(x, y, z);
                }
                if (value > (threshold + falloff))
                {
                    // Lies outside of falloff area above threshold, return second source
                    return this.High.Get(x, y, z);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, this.Low.Get(x, y, z), this.High.Get(x, y, z));
            }

            return (value < threshold ? this.Low.Get(x, y, z) : this.High.Get(x, y, z));
        }

        public override Double Get(Double x, Double y, Double z, Double w)
        {
            double value = this.Source.Get(x, y, z, w);
            double falloff = this.Falloff.Get(x, y, z, w);
            double threshold = this.Threshold.Get(x, y, z, w);

            if (falloff > 0.0)
            {
                if (value < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return this.Low.Get(x, y, z, w);
                }
                if (value > (threshold + falloff))
                {
                    // Lise outside of falloff area above threshold, return second source
                    return this.High.Get(x, y, z, w);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, this.Low.Get(x, y, z, w), this.High.Get(x, y, z, w));
            }

            return value < threshold ? this.Low.Get(x, y, z, w) : this.High.Get(x, y, z, w);
        }

        public override Double Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double value = this.Source.Get(x, y, z, w, u, v);
            double falloff = this.Falloff.Get(x, y, z, w, u, v);
            double threshold = this.Threshold.Get(x, y, z, w, u, v);

            if (falloff > 0.0)
            {
                if (value < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return this.Low.Get(x, y, z, w, u, v);
                }
                if (value > (threshold + falloff))
                {
                    // Lies outside of falloff area above threshold, return second source
                    return this.High.Get(x, y, z, w, u, v);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, this.Low.Get(x, y, z, w, u, v), this.High.Get(x, y, z, w, u, v));
            }

            return (value < threshold ? this.Low.Get(x, y, z, w, u, v) : this.High.Get(x, y, z, w, u, v));
        }
    }

}
