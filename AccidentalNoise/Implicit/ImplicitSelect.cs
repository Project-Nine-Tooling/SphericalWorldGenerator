namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitSelect : ImplicitModuleBase
    {
        public ImplicitSelect(ImplicitModuleBase source, ImplicitModuleBase low, ImplicitModuleBase high, double falloff, double threshold)
        {
            Source = source;
            Low = low;
            High = high;
            Falloff = new ImplicitConstant(falloff);
            Threshold = new ImplicitConstant(threshold);
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase Low { get; set; }

        public ImplicitModuleBase High { get; set; }

        public ImplicitModuleBase Threshold { get; set; }

        public ImplicitModuleBase Falloff { get; set; }

        public override double Get(double x, double y)
        {
            double value = Source.Get(x, y);
            double falloff = Falloff.Get(x, y);
            double threshold = Threshold.Get(x, y);

            if (falloff > 0.0)
            {
                if (value < threshold - falloff)
                {
                    // Lies outside of falloff area below threshold, return first source
                    return Low.Get(x, y);
                }
                if (value > threshold + falloff)
                {
                    // Lies outside of falloff area above threshold, return second source
                    return High.Get(x, y);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, Low.Get(x, y), High.Get(x, y));
            }

            return value < threshold ? Low.Get(x, y) : High.Get(x, y);
        }

        public override double Get(double x, double y, double z)
        {
            double value = Source.Get(x, y, z);
            double falloff = Falloff.Get(x, y, z);
            double threshold = Threshold.Get(x, y, z);

            if (falloff > 0.0)
            {
                if (value < threshold - falloff)
                {
                    // Lies outside of falloff area below threshold, return first source
                    return Low.Get(x, y, z);
                }
                if (value > threshold + falloff)
                {
                    // Lies outside of falloff area above threshold, return second source
                    return High.Get(x, y, z);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, Low.Get(x, y, z), High.Get(x, y, z));
            }

            return value < threshold ? Low.Get(x, y, z) : High.Get(x, y, z);
        }

        public override double Get(double x, double y, double z, double w)
        {
            double value = Source.Get(x, y, z, w);
            double falloff = Falloff.Get(x, y, z, w);
            double threshold = Threshold.Get(x, y, z, w);

            if (falloff > 0.0)
            {
                if (value < threshold - falloff)
                {
                    // Lies outside of falloff area below threshold, return first source
                    return Low.Get(x, y, z, w);
                }
                if (value > threshold + falloff)
                {
                    // Lise outside of falloff area above threshold, return second source
                    return High.Get(x, y, z, w);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, Low.Get(x, y, z, w), High.Get(x, y, z, w));
            }

            return value < threshold ? Low.Get(x, y, z, w) : High.Get(x, y, z, w);
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            double value = Source.Get(x, y, z, w, u, v);
            double falloff = Falloff.Get(x, y, z, w, u, v);
            double threshold = Threshold.Get(x, y, z, w, u, v);

            if (falloff > 0.0)
            {
                if (value < threshold - falloff)
                {
                    // Lies outside of falloff area below threshold, return first source
                    return Low.Get(x, y, z, w, u, v);
                }
                if (value > threshold + falloff)
                {
                    // Lies outside of falloff area above threshold, return second source
                    return High.Get(x, y, z, w, u, v);
                }
                // Lies within falloff area.
                double lower = threshold - falloff;
                double upper = threshold + falloff;
                double blend = MathHelper.QuinticBlend((value - lower) / (upper - lower));
				return MathHelper.Lerp(blend, Low.Get(x, y, z, w, u, v), High.Get(x, y, z, w, u, v));
            }

            return value < threshold ? Low.Get(x, y, z, w, u, v) : High.Get(x, y, z, w, u, v);
        }
    }

}
