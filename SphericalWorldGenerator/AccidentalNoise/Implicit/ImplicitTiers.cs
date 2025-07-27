using System;

namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitTiers : ImplicitModuleBase
    {
        public ImplicitTiers(ImplicitModuleBase source, int tiers, bool smooth)
        {
            Source = source;
            Tiers = tiers;
            Smooth = smooth;
        }

        public ImplicitModuleBase Source { get; set; }

        public int Tiers { get; set; }

        public bool Smooth { get; set; }

        public override double Get(double x, double y)
        {
            int numsteps = Tiers;
            if (Smooth) --numsteps;
            double val = Source.Get(x, y);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double u = Smooth ? MathHelper.QuinticBlend(t) : 0.0;
            return tb + u * (tt - tb);
        }

        public override double Get(double x, double y, double z)
        {
            int numsteps = Tiers;
            if (Smooth) --numsteps;
            double val = Source.Get(x, y, z);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double u = Smooth ? MathHelper.QuinticBlend(t) : 0.0;
            return tb + u * (tt - tb);
        }

        public override double Get(double x, double y, double z, double w)
        {
            int numsteps = Tiers;
            if (Smooth) --numsteps;
            double val = Source.Get(x, y, z, w);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double u = Smooth ? MathHelper.QuinticBlend(t) : 0.0;
            return tb + u * (tt - tb);
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            int numsteps = Tiers;
            if (Smooth) --numsteps;
            double val = Source.Get(x, y, z, w, u, v);
            var tb = Math.Floor(val * numsteps);
            var tt = tb + 1.0;
            var t = val * numsteps - tb;
            tb /= numsteps;
            tt /= numsteps;
            double s = Smooth ? MathHelper.QuinticBlend(t) : 0.0;
            return tb + s * (tt - tb);
        }
    }
}
