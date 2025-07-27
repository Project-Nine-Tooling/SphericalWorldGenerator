namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitScaleDomain : ImplicitModuleBase
    {
        public ImplicitScaleDomain(ImplicitModuleBase source,
            double xScale, double yScale, double zScale,
            double wScale, double uScale, double vScale)
        {
            Source = source;
            XScale = new ImplicitConstant(xScale);
            YScale = new ImplicitConstant(yScale);
            ZScale = new ImplicitConstant(zScale);
            WScale = new ImplicitConstant(wScale);
            UScale = new ImplicitConstant(uScale);
            VScale = new ImplicitConstant(vScale);
        }

        public ImplicitModuleBase Source { get; set; }

        public ImplicitModuleBase XScale { get; set; }

        public ImplicitModuleBase YScale { get; set; }

        public ImplicitModuleBase ZScale { get; set; }

        public ImplicitModuleBase WScale { get; set; }

        public ImplicitModuleBase UScale { get; set; }

        public ImplicitModuleBase VScale { get; set; }

        public void SetScales(
            double xScale, double yScale, double zScale,
            double wScale, double uScale, double vScale)
        {
            XScale = new ImplicitConstant(xScale);
            YScale = new ImplicitConstant(yScale);
            ZScale = new ImplicitConstant(zScale);
            WScale = new ImplicitConstant(wScale);
            UScale = new ImplicitConstant(uScale);
            VScale = new ImplicitConstant(vScale);
        }

        public override double Get(double x, double y)
        {
            return Source.Get(
                x * XScale.Get(x, y), 
                y * YScale.Get(x, y));
        }

        public override double Get(double x, double y, double z)
        {
            return Source.Get(
                x * XScale.Get(x, y, z), 
                y * YScale.Get(x, y, z), 
                z * ZScale.Get(x, y, z));
        }

        public override double Get(double x, double y, double z, double w)
        {
            return Source.Get(
                x * XScale.Get(x, y, z, w), 
                y * YScale.Get(x, y, z, w), 
                z * ZScale.Get(x, y, z, w),
                w * WScale.Get(x, y, z, w));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            return Source.Get(
                x * XScale.Get(x, y, z, w, u, v),
                y * YScale.Get(x, y, z, w, u, v),
                z * ZScale.Get(x, y, z, w, u, v),
                w * WScale.Get(x, y, z, w, u, v),
                u * UScale.Get(x, y, z, w, u, v),
                v * VScale.Get(x, y, z, w, u, v));
        }
    }
}