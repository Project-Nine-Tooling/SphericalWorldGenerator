using SphericalWorldGenerator;
using System;

namespace AccidentalNoise
{
    public sealed class ImplicitTiers : ImplicitModuleBase
    {
        public ImplicitTiers(ImplicitModuleBase source, Int32 tiers, Boolean smooth)
        {
            this.Source = source;
            this.Tiers = tiers;
            this.Smooth = smooth;
        }

        public ImplicitModuleBase Source { get; set; }

        public Int32 Tiers { get; set; }

        public Boolean Smooth { get; set; }

        public override Double Get(Double x, Double y)
        {
            int numsteps = Tiers;
            if (this.Smooth) --numsteps;
            double val = Source.Get(x, y);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double u = (this.Smooth ? MathHelper.QuinticBlend(t) : 0.0);
            return tb + u * (tt - tb);
        }

        public override Double Get(Double x, Double y, Double z)
        {
            int numsteps = Tiers;
            if (this.Smooth) --numsteps;
            double val = Source.Get(x, y, z);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double u = (this.Smooth ? MathHelper.QuinticBlend(t) : 0.0);
            return tb + u * (tt - tb);
        }

        public override Double Get(Double x, Double y, Double z, Double w)
        {
            int numsteps = Tiers;
            if (this.Smooth) --numsteps;
            double val = Source.Get(x, y, z, w);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double u = (this.Smooth ? MathHelper.QuinticBlend(t) : 0.0);
            return tb + u * (tt - tb);
        }

        public override Double Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            int numsteps = Tiers;
            if (this.Smooth) --numsteps;
            double val = Source.Get(x, y, z, w, u, v);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double s = (this.Smooth ? MathHelper.QuinticBlend(t) : 0.0);
            return tb + s * (tt - tb);
        }
    }
}
