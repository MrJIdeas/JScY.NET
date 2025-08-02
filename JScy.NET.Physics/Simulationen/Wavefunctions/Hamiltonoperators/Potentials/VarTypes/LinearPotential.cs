using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;
using System;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.Hamiltonoperators.Potentials.VarTypes
{
    public class LinearPotential : BlockPotential
    {
        public readonly EShift Direction;

        public LinearPotential(string name, int xSTART, int xEND, double potential, EShift direction) : base(name, xSTART, xEND, potential) => Direction = direction;

        public LinearPotential(string name, int xSTART, int xEND, int ySTART, int yEND, double potential, EShift direction) : base(name, xSTART, xEND, ySTART, yEND, potential) => Direction = direction;

        public LinearPotential(string name, int xSTART, int ySTART, int xEND, int yEND, int zSTART, int zEND, double Vmax, EShift direction) : base(name, xSTART, ySTART, xEND, yEND, zSTART, zEND, Vmax) => Direction = direction;

        public override IWavefunction getPsiV(WFInfo wfinfo)
        {
            var erg = -1 * base.getPsiV(wfinfo);
            if (Direction is EShift.Xm or EShift.Xp)
            {
                int dx = Math.Sign((int)Direction) * limit_x.Item2 - limit_x.Item1;
                int start = dx > 0 ? limit_x.Item1 : limit_x.Item2;
                int ende = dx < 0 ? limit_x.Item1 : limit_x.Item2;
                for (int i = 0; i < psiV.field.Length; i++)
                {
                    int x = i % wfinfo.DimInfo.DimX;
                    if ((x >= start && x < ende)
                        || (x < start && x >= ende))
                    {
                        psiV.field[i] *= (double)(x - start) / dx;
                    }
                }
            }
            else if (Direction is EShift.Ym or EShift.Yp)
            {
                int dy = Math.Sign((int)Direction) * limit_y.Item2 - limit_y.Item1;
                int start = dy > 0 ? limit_y.Item1 : limit_y.Item2;
                int ende = dy < 0 ? limit_y.Item1 : limit_y.Item2;
                for (int i = 0; i < psiV.field.Length; i++)
                {
                    int y = (i / wfinfo.DimInfo.DimX) % wfinfo.DimInfo.DimY;
                    if ((y >= start && y < ende)
                        || (y < start && y >= ende))
                    {
                        psiV.field[i] *= (double)(y - start) / dy;
                    }
                }
            }
            else if (Direction is EShift.Zm or EShift.Zp)
            {
                int dz = Math.Sign((int)Direction) * limit_z.Item2 - limit_z.Item1;
                int start = dz > 0 ? limit_z.Item1 : limit_z.Item2;
                int ende = dz < 0 ? limit_z.Item1 : limit_z.Item2;
                for (int i = 0; i < psiV.field.Length; i++)
                {
                    int z = i / (wfinfo.DimInfo.DimX * wfinfo.DimInfo.DimY);
                    if ((z >= start && z < ende)
                       || (z < start && z >= ende))
                    {
                        psiV.field[i] *= (double)(z - start) / dz;
                    }
                }
            }
            return erg;
        }
    }
}