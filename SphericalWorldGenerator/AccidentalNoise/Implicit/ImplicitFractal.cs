using System;

namespace AccidentalNoise
{
    public sealed class ImplicitFractal : ImplicitModuleBase
    {
        private readonly ImplicitBasisFunction[] basisFunctions = new ImplicitBasisFunction[Noise.MAX_SOURCES];

        private readonly ImplicitModuleBase[] sources = new ImplicitModuleBase[Noise.MAX_SOURCES];

        private readonly Double[] expArray = new Double[Noise.MAX_SOURCES];

        private readonly Double[,] correct = new Double[Noise.MAX_SOURCES, 2];

        private Int32 seed;

        private FractalType type;

        private Int32 octaves;

        public ImplicitFractal(FractalType fractalType, BasisType basisType, InterpolationType interpolationType, Int32 octaves, double frequency, Int32 seed)
        {
            this.seed = seed;
            this.Octaves = octaves;
            this.Frequency = frequency;
            this.Lacunarity = 2.00;
            this.Type = fractalType;
            this.SetAllSourceTypes(basisType, interpolationType);
            this.ResetAllSources();
        }

        public override Int32 Seed
        {
            get { return this.seed; }
            set
            {
                this.seed = value;
                for (int source = 0; source < Noise.MAX_SOURCES; source += 1)
                    this.sources[source].Seed = ((this.seed + source * 300));
            }
        }

        public FractalType Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                switch (this.type)
                {
                    case FractalType.FRACTIONALBROWNIANMOTION:
                        this.H = 1.00;
                        this.Gain = 0.00;
                        this.Offset = 0.00;
                        this.FractionalBrownianMotion_CalculateWeights();
                        break;
                    case FractalType.RIDGEDMULTI:
                        this.H = 0.90;
                        this.Gain = 2.00;
                        this.Offset = 1.00;
                        this.RidgedMulti_CalculateWeights();
                        break;
                    case FractalType.BILLOW:
                        this.H = 1.00;
                        this.Gain = 0.00;
                        this.Offset = 0.00;
                        this.Billow_CalculateWeights();
                        break;
                    case FractalType.MULTI:
                        this.H = 1.00;
                        this.Gain = 0.00;
                        this.Offset = 0.00;
                        this.Multi_CalculateWeights();
                        break;
                    case FractalType.HYBRIDMULTI:
                        this.H = 0.25;
                        this.Gain = 1.00;
                        this.Offset = 0.70;
                        this.HybridMulti_CalculateWeights();
                        break;
                    default:
                        this.H = 1.00;
                        this.Gain = 0.00;
                        this.Offset = 0.00;
                        this.FractionalBrownianMotion_CalculateWeights();
                        break;
                }
            }
        }

        public Int32 Octaves
        {
            get {return this.octaves; }
            set
            {
                if (value >= Noise.MAX_SOURCES)
                    value = Noise.MAX_SOURCES - 1;
                this.octaves = value;
            }
        }

        public Double Frequency { get; set; }

        public Double Lacunarity { get; set; }

        public Double Gain { get; set; }

        public Double Offset { get; set; }

        public Double H { get; set; }

        public void SetAllSourceTypes(BasisType newBasisType, InterpolationType newInterpolationType)
        {
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                this.basisFunctions[i] = new ImplicitBasisFunction(newBasisType, newInterpolationType, Seed);
            }
        }

        public void SetSourceType(Int32 which, BasisType newBasisType, InterpolationType newInterpolationType)
        {
            if (which >= Noise.MAX_SOURCES || which < 0) return;

            this.basisFunctions[which].BasisType = newBasisType;
            this.basisFunctions[which].InterpolationType = newInterpolationType;
        }

        public void SetSourceOverride(Int32 which, ImplicitModuleBase newSource)
        {
            if (which < 0 || which >= Noise.MAX_SOURCES) return;

            this.sources[which] = newSource;
        }

        public void ResetSource(Int32 which)
        {
            if (which < 0 || which >= Noise.MAX_SOURCES) return;

            this.sources[which] = this.basisFunctions[which];
        }

        public void ResetAllSources()
        {
            for (int c = 0; c < Noise.MAX_SOURCES; ++c)
                this.sources[c] = this.basisFunctions[c];
        }

        public ImplicitBasisFunction GetBasis(Int32 which)
        {
            if (which < 0 || which >= Noise.MAX_SOURCES) return null;

            return this.basisFunctions[which];
        }

        public override Double Get(Double x, Double y)
        {
            Double v;
            switch (type)
            {
                case FractalType.FRACTIONALBROWNIANMOTION:
                    v = FractionalBrownianMotion_Get(x, y);
                    break;
                case FractalType.RIDGEDMULTI:
                    v = RidgedMulti_Get(x, y);
                    break;
                case FractalType.BILLOW:
                    v = Billow_Get(x, y);
                    break;
                case FractalType.MULTI:
                    v = Multi_Get(x, y);
                    break;
                case FractalType.HYBRIDMULTI:
                    v = HybridMulti_Get(x, y);
                    break;
                default:
                    v = FractionalBrownianMotion_Get(x, y);
                    break;
            }
			return MathHelper.Clamp(v, -1.0, 1.0);
        }

        public override Double Get(Double x, Double y, Double z)
        {
            Double val;
            switch (type)
            {
                case FractalType.FRACTIONALBROWNIANMOTION:
                    val = FractionalBrownianMotion_Get(x, y, z);
                    break;
                case FractalType.RIDGEDMULTI:
                    val = RidgedMulti_Get(x, y, z);
                    break;
                case FractalType.BILLOW:
                    val = Billow_Get(x, y, z);
                    break;
                case FractalType.MULTI:
                    val = Multi_Get(x, y, z);
                    break;
                case FractalType.HYBRIDMULTI:
                    val = HybridMulti_Get(x, y, z);
                    break;
                default:
                    val = FractionalBrownianMotion_Get(x, y, z);
                    break;
            }
			return MathHelper.Clamp(val, -1.0, 1.0);
        }

        public override Double Get(Double x, Double y, Double z, Double w)
        {
            Double val;
            switch (type)
            {
                case FractalType.FRACTIONALBROWNIANMOTION:
                    val = FractionalBrownianMotion_Get(x, y, z, w);
                    break;
                case FractalType.RIDGEDMULTI:
                    val = RidgedMulti_Get(x, y, z, w);
                    break;
                case FractalType.BILLOW:
                    val = Billow_Get(x, y, z, w);
                    break;
                case FractalType.MULTI:
                    val = Multi_Get(x, y, z, w);
                    break;
                case FractalType.HYBRIDMULTI:
                    val = HybridMulti_Get(x, y, z, w);
                    break;
                default:
                    val = FractionalBrownianMotion_Get(x, y, z, w);
                    break;
            }
			return MathHelper.Clamp(val, -1.0, 1.0);
        }

        public override Double Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            Double val;
            switch (type)
            {
                case FractalType.FRACTIONALBROWNIANMOTION:
                    val = FractionalBrownianMotion_Get(x, y, z, w, u, v);
                    break;
                case FractalType.RIDGEDMULTI:
                    val = RidgedMulti_Get(x, y, z, w, u, v);
                    break;
                case FractalType.BILLOW:
                    val = Billow_Get(x, y, z, w, u, v);
                    break;
                case FractalType.MULTI:
                    val = Multi_Get(x, y, z, w, u, v);
                    break;
                case FractalType.HYBRIDMULTI:
                    val = HybridMulti_Get(x, y, z, w, u, v);
                    break;
                default:
                    val = FractionalBrownianMotion_Get(x, y, z, w, u, v);
                    break;
            }

			return MathHelper.Clamp(val, -1.0, 1.0);
        }


        private void FractionalBrownianMotion_CalculateWeights()
        {
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                expArray[i] = Math.Pow(Lacunarity, -i * H);
            }

            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            double minvalue = 0.00;
            double maxvalue = 0.00;
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                minvalue += -1.0 * expArray[i];
                maxvalue += 1.0 * expArray[i];

                const Double a = -1.0;
                const Double b = 1.0;
                double scale = (b - a) / (maxvalue - minvalue);
                double bias = a - minvalue * scale;
                correct[i, 0] = scale;
                correct[i, 1] = bias;
            }
        }

        private void RidgedMulti_CalculateWeights()
        {
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                expArray[i] = Math.Pow(Lacunarity, -i * H);
            }

            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            double minvalue = 0.00;
            double maxvalue = 0.00;
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                minvalue += (Offset - 1.0) * (Offset - 1.0) * expArray[i];
                maxvalue += (Offset) * (Offset) * expArray[i];

                const Double a = -1.0;
                const Double b = 1.0;
                double scale = (b - a) / (maxvalue - minvalue);
                double bias = a - minvalue * scale;
                correct[i, 0] = scale;
                correct[i, 1] = bias;
            }

        }

        private void Billow_CalculateWeights()
        {
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                expArray[i] = Math.Pow(Lacunarity, -i * H);
            }

            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            double minvalue = 0.0;
            double maxvalue = 0.0;
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                minvalue += -1.0 * expArray[i];
                maxvalue += 1.0 * expArray[i];

                const Double a = -1.0;
                const Double b = 1.0;
                double scale = (b - a) / (maxvalue - minvalue);
                double bias = a - minvalue * scale;
                correct[i, 0] = scale;
                correct[i, 1] = bias;
            }

        }

        private void Multi_CalculateWeights()
        {
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                expArray[i] = Math.Pow(Lacunarity, -i * H);
            }

            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            double minvalue = 1.0;
            double maxvalue = 1.0;
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                minvalue *= -1.0 * expArray[i] + 1.0;
                maxvalue *= 1.0 * expArray[i] + 1.0;

                const Double a = -1.0;
                const Double b = 1.0;
                double scale = (b - a) / (maxvalue - minvalue);
                double bias = a - minvalue * scale;
                correct[i, 0] = scale;
                correct[i, 1] = bias;
            }

        }

        private void HybridMulti_CalculateWeights()
        {
            for (int i = 0; i < Noise.MAX_SOURCES; ++i)
            {
                expArray[i] = Math.Pow(Lacunarity, -i * H);
            }

            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            const double a = -1.0;
            const double b = 1.0;

            double minvalue = Offset - 1.0;
            double maxvalue = Offset + 1.0;
            double weightmin = Gain * minvalue;
            double weightmax = Gain * maxvalue;

            double scale = (b - a) / (maxvalue - minvalue);
            double bias = a - minvalue * scale;
            correct[0, 0] = scale;
            correct[0, 1] = bias;


            for (int i = 1; i < Noise.MAX_SOURCES; ++i)
            {
                if (weightmin > 1.00) weightmin = 1.00;
                if (weightmax > 1.00) weightmax = 1.00;

                double signal = (Offset - 1.0) * expArray[i];
                minvalue += signal * weightmin;
                weightmin *= Gain * signal;

                signal = (Offset + 1.0) * expArray[i];
                maxvalue += signal * weightmax;
                weightmax *= Gain * signal;


                scale = (b - a) / (maxvalue - minvalue);
                bias = a - minvalue * scale;
                correct[i, 0] = scale;
                correct[i, 1] = bias;
            }

        }


        private Double FractionalBrownianMotion_Get(Double x, Double y)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;


            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y) * expArray[i];
                value += signal;
                x *= Lacunarity;
                y *= Lacunarity;
            }

            return value;
        }

        private Double FractionalBrownianMotion_Get(Double x, Double y, Double z)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z) * expArray[i];
                value += signal;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
            }

            return value;
        }

        private Double FractionalBrownianMotion_Get(Double x, Double y, Double z, Double w)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z, w) * expArray[i];
                value += signal;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double FractionalBrownianMotion_Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;
            u *= Frequency;
            v *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z, w, u, v) * expArray[i];
                value += signal;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
                u *= Lacunarity;
                v *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }


        private Double Multi_Get(Double x, Double y)
        {
            double value = 1.00;
            x *= Frequency;
            y *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                value *= sources[i].Get(x, y) * expArray[i] + 1.0;
                x *= Lacunarity;
                y *= Lacunarity;

            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double Multi_Get(Double x, Double y, Double z, Double w)
        {
            double value = 1.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                value *= sources[i].Get(x, y, z, w) * expArray[i] + 1.0;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double Multi_Get(Double x, Double y, Double z)
        {
            double value = 1.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                value *= sources[i].Get(x, y, z) * expArray[i] + 1.0;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double Multi_Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double value = 1.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;
            u *= Frequency;
            v *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                value *= sources[i].Get(x, y, z, w, u, v) * expArray[i] + 1.00;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
                u *= Lacunarity;
                v *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }


        private Double Billow_Get(Double x, Double y)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y);
                signal = 2.0 * Math.Abs(signal) - 1.0;
                value += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;

            }

            value += 0.5;
            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double Billow_Get(Double x, Double y, Double z, Double w)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z, w);
                signal = 2.0 * Math.Abs(signal) - 1.0;
                value += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
            }

            value += 0.5;
            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double Billow_Get(Double x, Double y, Double z)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z);
                signal = 2.0 * Math.Abs(signal) - 1.0;
                value += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
            }

            value += 0.5;
            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double Billow_Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double value = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;
            u *= Frequency;
            v *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z, w, u, v);
                signal = 2.0 * Math.Abs(signal) - 1.0;
                value += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
                u *= Lacunarity;
                v *= Lacunarity;
            }

            value += 0.5;
            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }


        private Double RidgedMulti_Get(Double x, Double y)
        {
            double result = 0.00;
            x *= Frequency;
            y *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y);
                signal = Offset - Math.Abs(signal);
                signal *= signal;
                result += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;

            }

            return result * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double RidgedMulti_Get(Double x, Double y, Double z, Double w)
        {
            double result = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z, w);
                signal = Offset - Math.Abs(signal);
                signal *= signal;
                result += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
            }

            return result * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double RidgedMulti_Get(Double x, Double y, Double z)
        {
            double result = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z);
                signal = Offset - Math.Abs(signal);
                signal *= signal;
                result += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
            }

            return result * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double RidgedMulti_Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            double result = 0.00;
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;
            u *= Frequency;
            v *= Frequency;

            for (int i = 0; i < octaves; ++i)
            {
                double signal = sources[i].Get(x, y, z, w, u, v);
                signal = Offset - Math.Abs(signal);
                signal *= signal;
                result += signal * expArray[i];

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
                u *= Lacunarity;
                v *= Lacunarity;
            }

            return result * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }


        private Double HybridMulti_Get(Double x, Double y)
        {
            x *= Frequency;
            y *= Frequency;

            double value = sources[0].Get(x, y) + Offset;
            double weight = Gain * value;
            x *= Lacunarity;
            y *= Lacunarity;

            for (int i = 1; i < octaves; ++i)
            {
                if (weight > 1.0) weight = 1.0;
                double signal = (sources[i].Get(x, y) + Offset) * expArray[i];
                value += weight * signal;
                weight *= Gain * signal;
                x *= Lacunarity;
                y *= Lacunarity;

            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double HybridMulti_Get(Double x, Double y, Double z)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            double value = sources[0].Get(x, y, z) + Offset;
            double weight = Gain * value;
            x *= Lacunarity;
            y *= Lacunarity;
            z *= Lacunarity;

            for (int i = 1; i < octaves; ++i)
            {
                if (weight > 1.0) weight = 1.0;
                double signal = (sources[i].Get(x, y, z) + Offset) * expArray[i];
                value += weight * signal;
                weight *= Gain * signal;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double HybridMulti_Get(Double x, Double y, Double z, Double w)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;

            double value = sources[0].Get(x, y, z, w) + Offset;
            double weight = Gain * value;
            x *= Lacunarity;
            y *= Lacunarity;
            z *= Lacunarity;
            w *= Lacunarity;

            for (int i = 1; i < octaves; ++i)
            {
                if (weight > 1.0) weight = 1.0;
                double signal = (sources[i].Get(x, y, z, w) + Offset) * expArray[i];
                value += weight * signal;
                weight *= Gain * signal;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }

        private Double HybridMulti_Get(Double x, Double y, Double z, Double w, Double u, Double v)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;
            w *= Frequency;
            u *= Frequency;
            v *= Frequency;

            double value = sources[0].Get(x, y, z, w, u, v) + Offset;
            double weight = Gain * value;
            x *= Lacunarity;
            y *= Lacunarity;
            z *= Lacunarity;
            w *= Lacunarity;
            u *= Lacunarity;
            v *= Lacunarity;

            for (int i = 1; i < octaves; ++i)
            {
                if (weight > 1.0) weight = 1.0;
                double signal = (sources[i].Get(x, y, z, w, u, v) + Offset) * expArray[i];
                value += weight * signal;
                weight *= Gain * signal;
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                w *= Lacunarity;
                u *= Lacunarity;
                v *= Lacunarity;
            }

            return value * correct[octaves - 1, 0] + correct[octaves - 1, 1];
        }
    }
}
