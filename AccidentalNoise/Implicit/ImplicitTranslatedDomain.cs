﻿namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitTranslatedDomain : ImplicitModuleBase
    {        
        public ImplicitTranslatedDomain(
            ImplicitModuleBase source,
            double xAxis, double yAxis, double zAxis,
            double wAxis, double uAxis, double vAxis)
        {
            Source = source;
            XAxis = new ImplicitConstant(xAxis);
            YAxis = new ImplicitConstant(yAxis);
            ZAxis = new ImplicitConstant(zAxis);
            WAxis = new ImplicitConstant(wAxis);
            UAxis = new ImplicitConstant(uAxis);
            VAxis = new ImplicitConstant(vAxis);
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase XAxis { get; set; }

        public ImplicitModuleBase YAxis { get; set; }

        public ImplicitModuleBase ZAxis { get; set; }

        public ImplicitModuleBase WAxis { get; set; }

        public ImplicitModuleBase UAxis { get; set; }

        public ImplicitModuleBase VAxis { get; set; }

        public override double Get(double x, double y)
        {
            return Source.Get(
                x + XAxis.Get(x, y),
                y + YAxis.Get(x, y));
        }

        public override double Get(double x, double y, double z)
        {
            return Source.Get(
                x + XAxis.Get(x, y, z),
                y + YAxis.Get(x, y, z),
                z + ZAxis.Get(x, y, z));
        }

        public override double Get(double x, double y, double z, double w)
        {
            return Source.Get(
                x + XAxis.Get(x, y, z, w),
                y + YAxis.Get(x, y, z, w),
                z + ZAxis.Get(x, y, z, w),
                w + WAxis.Get(x, y, z, w));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            return Source.Get(
                x + XAxis.Get(x, y, z, w, u, v),
                y + YAxis.Get(x, y, z, w, u, v),
                z + ZAxis.Get(x, y, z, w, u, v),
                w + WAxis.Get(x, y, z, w, u, v),
                u + UAxis.Get(x, y, z, w, u, v),
                v + VAxis.Get(x, y, z, w, u, v));
        }
    }
}