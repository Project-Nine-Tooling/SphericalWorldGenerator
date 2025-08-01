﻿using AccidentalNoise.Enums;
using AccidentalNoise.Implicit;
using System;

namespace AccidentalNoise
{
    public class Mapping
    {
        private const double PI2 = Math.PI * 2.0;
        
        public static void Map2D(MappingMode mappingMode, double[,] array, ImplicitModuleBase module, MappingRanges ranges, double z)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double p = x / (double)width;
                    double q = y / (double)height;
                    double r;
                    double nx;
                    double ny;
                    double nz;
                    double nw;
                    double nu;
                    double dx;
                    double dy;
                    double dz;
                    double val = 0.00;
                    switch (mappingMode)
                    {
                        case MappingMode.SeamlessNone:
                            nx = ranges.MapX0 + p*(ranges.MapX1 - ranges.MapX0);
                            ny = ranges.MapY0 + q*(ranges.MapY1 - ranges.MapY0);
                            nz = z;
                            val = module.Get(nx, ny, nz);
                            break;
                        case MappingMode.SeamlessX:
                            dx = ranges.LoopX1 - ranges.LoopX0;
                            dy = ranges.MapY1 - ranges.MapY0;
                            p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                            nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                            ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                            nz = ranges.MapY0 + q*dy;
                            nw = z;
                            val = module.Get(nx, ny, nz, nw);
                            break;
                        case MappingMode.SeamlessY:
                            dx = ranges.MapX1 - ranges.MapX0;
                            dy = ranges.LoopY1 - ranges.LoopY0;
                            q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                            nx = ranges.MapX0 + p*dx;
                            ny = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                            nz = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                            nw = z;
                            val = module.Get(nx, ny, nz, nw);
                            break;
                        case MappingMode.SeamlessZ:
                            dx = ranges.MapX1 - ranges.MapX0;
                            dy = ranges.MapY1 - ranges.MapY0;
                            dz = ranges.LoopZ1 - ranges.LoopZ0;
                            nx = ranges.MapX0 + p*dx;
                            ny = ranges.MapY0 + p*dy;
                            r = (z - ranges.MapZ0)/(ranges.MapZ1 - ranges.MapZ0);
                            double zval = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                            nz = ranges.LoopZ0 + Math.Cos(zval*PI2)*dz/PI2;
                            nw = ranges.LoopZ0 + Math.Sin(zval*PI2)*dz/PI2;
                            val = module.Get(nx, ny, nz, nw);
                            break;
                        case MappingMode.SeamlessXY:
                            dx = ranges.LoopX1 - ranges.LoopX0;
                            dy = ranges.LoopY1 - ranges.LoopY0;
                            p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                            q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                            nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                            ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                            nz = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                            nw = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                            nu = z;
                            val = module.Get(nx, ny, nz, nw, nu, 0);
                            break;
                        case MappingMode.SeamlessXZ:
                            dx = ranges.LoopX1 - ranges.LoopX0;
                            dy = ranges.MapY1 - ranges.MapY0;
                            dz = ranges.LoopZ1 - ranges.LoopZ0;
                            r = (z - ranges.MapZ0)/(ranges.MapZ1 - ranges.MapZ0);
                            double xzval = r*(ranges.MapX1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                            p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                            nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                            ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                            nz = ranges.MapY0 + q*dy;
                            nw = ranges.LoopZ0 + Math.Cos(xzval*PI2)*dz/PI2;
                            nu = ranges.LoopZ0 + Math.Sin(xzval*PI2)*dz/PI2;
                            val = module.Get(nx, ny, nz, nw, nu, 0);
                            break;
                        case MappingMode.SeamlessYZ:
                            dx = ranges.MapX1 - ranges.MapX0;
                            dy = ranges.LoopY1 - ranges.LoopY0;
                            dz = ranges.LoopZ1 - ranges.LoopZ0;
                            r = (z - ranges.MapZ0)/(ranges.MapZ1 - ranges.MapZ0);
                            double yzval = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                            q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                            nx = ranges.MapX0 + p*dx;
                            ny = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                            nz = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                            nw = ranges.LoopZ0 + Math.Cos(yzval*PI2)*dz/PI2;
                            nu = ranges.LoopZ0 + Math.Sin(yzval*PI2)*dz/PI2;
                            val = module.Get(nx, ny, nz, nw, nu, 0);
                            break;
                        case MappingMode.SeamlessXYZ:
                            dx = ranges.LoopX1 - ranges.LoopX0;
                            dy = ranges.LoopY1 - ranges.LoopY0;
                            dz = ranges.LoopZ1 - ranges.LoopZ0;
                            p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                            q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                            r = (z - ranges.MapZ0)/(ranges.MapZ1 - ranges.MapZ0);
                            double xyzval = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                            nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                            ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                            nz = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                            nw = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                            nu = ranges.LoopZ0 + Math.Cos(xyzval*PI2)*dz/PI2;
                            double nv = ranges.LoopZ0 + Math.Sin(xyzval*PI2)*dz/PI2;
                            val = module.Get(nx, ny, nz, nw, nu, nv);
                            break;
                    }
                    array[x, y] = val;
                }
            }
        }

        public static void Map2DNoZ(MappingMode mappingMode, double[,] array, ImplicitModuleBase module, MappingRanges ranges)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double p = x / (double)width;
                    double q = y / (double)height;
                    double nx;
                    double ny;
                    double nz;
                    double dx;
                    double dy;
                    double val = 0.00;
                    switch (mappingMode)
                    {
                        case MappingMode.SeamlessNone:
                            nx = ranges.MapX0 + p*(ranges.MapX1 - ranges.MapX0);
                            ny = ranges.MapY0 + q*(ranges.MapY1 - ranges.MapY0);
                            val = module.Get(nx, ny);
                            break;
                        case MappingMode.SeamlessX:
                            dx = ranges.LoopX1 - ranges.LoopX0;
                            dy = ranges.MapY1 - ranges.MapY0;
                            p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                            nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                            ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                            nz = ranges.MapY0 + q*dy;
                            val = module.Get(nx, ny, nz);
                            break;
                        case MappingMode.SeamlessY:
                            dx = ranges.MapX1 - ranges.MapX0;
                            dy = ranges.LoopY1 - ranges.LoopY0;
                            q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                            nx = ranges.MapX0 + p*dx;
                            ny = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                            nz = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                            val = module.Get(nx, ny, nz);
                            break;

                        case MappingMode.SeamlessXY:
                            dx = ranges.LoopX1 - ranges.LoopX0;
                            dy = ranges.LoopY1 - ranges.LoopY0;
                            p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                            q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                            nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                            ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                            nz = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                            double nw = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                            val = module.Get(nx, ny, nz, nw);
                            break;
                    }
                    array[x, y] = val;
                }
            }
        }

        public static void Map3D(MappingMode mappingMode, double[,,] array, ImplicitModuleBase module, MappingRanges ranges)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            int depth = array.GetLength(2);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int z = 0; z < depth; ++z)
                    {
                        double p = x / (double)width;
                        double q = y / (double)height;
                        double r = z / (double)depth;
                        double nx;
                        double ny;
                        double nz;
                        double nw;
                        double nu;
                        double dx;
                        double dy;
                        double dz;
                        double val = 0.0;

                        switch (mappingMode)
                        {
                            case MappingMode.SeamlessNone:
                                dx = ranges.MapX1 - ranges.MapX0;
                                dy = ranges.MapY1 - ranges.MapY0;
                                dz = ranges.MapZ1 - ranges.MapZ0;
                                nx = ranges.MapX0 + p*dx;
                                ny = ranges.MapY0 + q*dy;
                                nz = ranges.MapZ0 + r*dz;
                                val = module.Get(nx, ny, nz);
                                break;
                            case MappingMode.SeamlessX:
                                dx = ranges.LoopX1 - ranges.LoopX0;
                                dy = ranges.MapY1 - ranges.MapY0;
                                dz = ranges.MapZ1 - ranges.MapZ0;
                                p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                                nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                                ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                                nz = ranges.MapY0 + q*dy;
                                nw = ranges.MapZ0 + depth*dz;
                                val = module.Get(nx, ny, nz, nw);
                                break;
                            case MappingMode.SeamlessY:
                                dx = ranges.MapX1 - ranges.MapX0;
                                dy = ranges.LoopY1 - ranges.LoopY0;
                                dz = ranges.MapZ1 - ranges.MapZ0;
                                q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                                nx = ranges.MapX0 + p*dx;
                                ny = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                                nz = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                                nw = ranges.MapZ0 + r*dz;
                                val = module.Get(nx, ny, nz, nw);
                                break;
                            case MappingMode.SeamlessZ:
                                dx = ranges.MapX1 - ranges.MapX0;
                                dy = ranges.MapY1 - ranges.MapY0;
                                dz = ranges.LoopZ1 - ranges.LoopZ0;
                                r = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                                nx = ranges.MapX0 + p*dx;
                                ny = ranges.MapY0 + q*dy;
                                nz = ranges.LoopZ0 + Math.Cos(r*PI2)*dz/PI2;
                                nw = ranges.LoopZ0 + Math.Sin(r*PI2)*dz/PI2;
                                val = module.Get(nx, ny, nz, nw);
                                break;
                            case MappingMode.SeamlessXY:
                                dx = ranges.LoopX1 - ranges.LoopX0;
                                dy = ranges.LoopY1 - ranges.LoopY0;
                                dz = ranges.MapZ1 - ranges.MapZ0;
                                p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                                q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                                nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                                ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                                nz = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                                nw = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                                nu = ranges.MapZ0 + r*dz;
                                val = module.Get(nx, ny, nz, nw, nu, 0);
                                break;
                            case MappingMode.SeamlessXZ:
                                dx = ranges.LoopX1 - ranges.LoopX0;
                                dy = ranges.MapY1 - ranges.MapY0;
                                dz = ranges.LoopZ1 - ranges.LoopZ0;
                                p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                                r = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                                nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                                ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                                nz = ranges.MapY0 + q*dy;
                                nw = ranges.LoopZ0 + Math.Cos(r*PI2)*dz/PI2;
                                nu = ranges.LoopZ0 + Math.Sin(r*PI2)*dz/PI2;
                                val = module.Get(nx, ny, nz, nw, nu, 0);
                                break;
                            case MappingMode.SeamlessYZ:
                                dx = ranges.MapX1 - ranges.MapX0;
                                dy = ranges.LoopY1 - ranges.LoopY0;
                                dz = ranges.LoopZ1 - ranges.LoopZ0;
                                q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                                r = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                                nx = ranges.MapX0 + p*dx;
                                ny = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                                nz = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                                nw = ranges.LoopZ0 + Math.Cos(r*PI2)*dz/PI2;
                                nu = ranges.LoopZ0 + Math.Sin(r*PI2)*dz/PI2;
                                val = module.Get(nx, ny, nz, nw, nu, 0);
                                break;
                            case MappingMode.SeamlessXYZ:
                                dx = ranges.LoopX1 - ranges.LoopX0;
                                dy = ranges.LoopY1 - ranges.LoopY0;
                                dz = ranges.LoopZ1 - ranges.LoopZ0;
                                p = p*(ranges.MapX1 - ranges.MapX0)/(ranges.LoopX1 - ranges.LoopX0);
                                q = q*(ranges.MapY1 - ranges.MapY0)/(ranges.LoopY1 - ranges.LoopY0);
                                r = r*(ranges.MapZ1 - ranges.MapZ0)/(ranges.LoopZ1 - ranges.LoopZ0);
                                nx = ranges.LoopX0 + Math.Cos(p*PI2)*dx/PI2;
                                ny = ranges.LoopX0 + Math.Sin(p*PI2)*dx/PI2;
                                nz = ranges.LoopY0 + Math.Cos(q*PI2)*dy/PI2;
                                nw = ranges.LoopY0 + Math.Sin(q*PI2)*dy/PI2;
                                nu = ranges.LoopZ0 + Math.Cos(r*PI2)*dz/PI2;
                                double nv = ranges.LoopZ0 + Math.Sin(r*PI2)*dz/PI2;
                                val = module.Get(nx, ny, nz, nw, nu, nv);
                                break;
                        }
                        array[x, y, z] = val;
                    }
                }
            }
        }
    }
}