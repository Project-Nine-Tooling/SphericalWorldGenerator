using System;

namespace AccidentalNoise.Implicit
{
    public sealed class ImplicitAutoCorrect : ImplicitModuleBase
    {
        private ImplicitModuleBase source;

        private double low;

        private double high;

        private double scale2D;

        private double offset2D;

        private double scale3D;

        private double offset3D;

        private double scale4D;

        private double offset4D;

        private double scale6D;

        private double offset6D;

        public ImplicitAutoCorrect(ImplicitModuleBase source, double low, double high)
        {
            this.source = source;
            this.low = low;
            this.high = high;
            Calculate();
        }

        public ImplicitModuleBase Source
        {
            get { return source; }
            set
            {
                source = value;
                Calculate();
            }
        }

        public double Low
        {
            get { return low; }
            set
            {
                low = value;
                Calculate();
            }
        }

        public double High
        {
            get { return high; }
            set
            {
                high = value;
                Calculate();
            }
        }

        private void Calculate()
        {
            Random random = new();

            // Calculate 2D
            double mn = 10000.0;
            double mx = -10000.0;
            for (int c = 0; c < 10000; ++c)
            {
                double nx = random.NextDouble() * 4.0 - 2.0;
                double ny = random.NextDouble() * 4.0 - 2.0;

                double value = Source.Get(nx, ny);
                if (value < mn) mn = value;
                if (value > mx) mx = value;
            }
            scale2D = (high - low) / (mx - mn);
            offset2D = low - mn * scale2D;

            // Calculate 3D
            mn = 10000.0;
            mx = -10000.0;
            for (int c = 0; c < 10000; ++c)
            {
                double nx = random.NextDouble() * 4.0 - 2.0;
                double ny = random.NextDouble() * 4.0 - 2.0;
                double nz = random.NextDouble() * 4.0 - 2.0;

                double value = Source.Get(nx, ny, nz);
                if (value < mn) mn = value;
                if (value > mx) mx = value;
            }
            scale3D = (high - low) / (mx - mn);
            offset3D = low - mn * scale3D;

            // Calculate 4D
            mn = 10000.0;
            mx = -10000.0;
            for (int c = 0; c < 10000; ++c)
            {
                double nx = random.NextDouble() * 4.0 - 2.0;
                double ny = random.NextDouble() * 4.0 - 2.0;
                double nz = random.NextDouble() * 4.0 - 2.0;
                double nw = random.NextDouble() * 4.0 - 2.0;

                double value = Source.Get(nx, ny, nz, nw);
                if (value < mn) mn = value;
                if (value > mx) mx = value;
            }
            scale4D = (high - low) / (mx - mn);
            offset4D = low - mn * scale4D;

            // Calculate 6D
            mn = 10000.0;
            mx = -10000.0;
            for (int c = 0; c < 10000; ++c)
            {
                double nx = random.NextDouble() * 4.0 - 2.0;
                double ny = random.NextDouble() * 4.0 - 2.0;
                double nz = random.NextDouble() * 4.0 - 2.0;
                double nw = random.NextDouble() * 4.0 - 2.0;
                double nu = random.NextDouble() * 4.0 - 2.0;
                double nv = random.NextDouble() * 4.0 - 2.0;

                double value = Source.Get(nx, ny, nz, nw, nu, nv);
                if (value < mn) mn = value;
                if (value > mx) mx = value;
            }
            scale6D = (high - low) / (mx - mn);
            offset6D = low - mn * scale6D;
        }

        public void SetRange(double low, double high)
        {
            this.low = low;
            this.high = high;
            Calculate();
        }

        public override double Get(double x, double y)
        {
            return Math.Max(low, Math.Min(high, Source.Get(x, y) * scale2D + offset2D));
        }

        public override double Get(double x, double y, double z)
        {
            return Math.Max(low, Math.Min(high, Source.Get(x, y, z) * scale3D + offset3D));
        }

        public override double Get(double x, double y, double z, double w)
        {
            return Math.Max(low, Math.Min(high, Source.Get(x, y, z, w) * scale4D + offset4D));
        }

        public override double Get(double x, double y, double z, double w, double u, double v)
        {
            return Math.Max(low, Math.Min(high, Source.Get(x, y, z, w, u, v) * scale6D + offset6D));
        }
    }
}