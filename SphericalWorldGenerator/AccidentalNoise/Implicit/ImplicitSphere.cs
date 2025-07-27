using System;

namespace AccidentalNoise
{
    public sealed class ImplicitSphere : ImplicitModuleBase
    {
        public ImplicitSphere(
            Double xCenter, Double yCenter, Double zCenter,
            Double wCenter, Double uCenter, Double vCenter,
            Double radius)
        {
            this.XCenter = new ImplicitConstant(xCenter);
            this.YCenter = new ImplicitConstant(yCenter);
            this.ZCenter = new ImplicitConstant(zCenter);
            this.WCenter = new ImplicitConstant(wCenter);
            this.UCenter = new ImplicitConstant(uCenter);
            this.VCenter = new ImplicitConstant(vCenter);
            this.Radius = new ImplicitConstant(radius);
        }

        public ImplicitModuleBase XCenter { get; set; }

        public ImplicitModuleBase YCenter { get; set; }

        public ImplicitModuleBase ZCenter { get; set; }

        public ImplicitModuleBase WCenter { get; set; }

        public ImplicitModuleBase UCenter { get; set; }

        public ImplicitModuleBase VCenter { get; set; }

        public ImplicitModuleBase Radius { get; set; }

        public override Double Get(Double x, Double y)
        {
            double dx = x - this.XCenter.Get(x, y);
            double dy = y - this.YCenter.Get(x, y);
            var len = Math.Sqrt(dx * dx + dy * dy);
            double rad = this.Radius.Get(x, y);
            var i = (rad - len) / rad;
            if (i < 0) i = 0;
            if (i > 1) i = 1;

            return i;
        }

        public override Double Get(Double x, Double y, Double z)
        {
            double dx = x - this.XCenter.Get(x, y, z);
            double dy = y - this.YCenter.Get(x, y, z);
            double dz = z - this.ZCenter.Get(x, y, z);
            var len = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double rad = this.Radius.Get(x, y, z);
            var i = (rad - len) / rad;
            if (i < 0) i = 0;
            if (i > 1) i = 1;

            return i;
        }

        public override Double Get(Double x, Double y, Double z, Double w)
        {
            double dx = x - this.XCenter.Get(x, y, z, w);
            double dy = y - this.YCenter.Get(x, y, z, w);
            double dz = z - this.ZCenter.Get(x, y, z, w);
            double dw = w - this.WCenter.Get(x, y, z, w);
            var len = Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
            double rad = this.Radius.Get(x, y, z, w);
            var i = (rad - len) / rad;
            if (i < 0) i = 0;
            if (i > 1) i = 1;

            return i;
        }

        public override Double Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double dx = x - this.XCenter.Get(x, y, z, w, u, v);
            double dy = y - this.YCenter.Get(x, y, z, w, u, v);
            double dz = z - this.ZCenter.Get(x, y, z, w, u, v);
            double dw = w - this.WCenter.Get(x, y, z, w, u, v);
            double du = u - this.UCenter.Get(x, y, z, w, u, v);
            double dv = v - this.VCenter.Get(x, y, z, w, u, v);
            var len = Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw + du * du + dv * dv);
            double rad = this.Radius.Get(x, y, z, w, u, v);
            var i = (rad - len) / rad;
            if (i < 0) i = 0;
            if (i > 1) i = 1;

            return i;
        }
    }
}
