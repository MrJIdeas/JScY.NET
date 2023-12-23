using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.VarTypes;
using JScience.Physik.Simulationen.Spins.Enums;
using System;
using System.Xml.Linq;

namespace JScience.Physik.Simulationen.Spins.Classic.Simulations.Lattice
{
    public class ANNNI_Classic_3D_Lattice : Ising_Classic_3D_Lattice
    {
        public ANNNI_Classic_3D_Lattice(double j, double kappa, double b, double t, uint dimX, uint dimY, uint dimZ, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving) : base(j, b, t, dimX, dimY, dimZ, MaxSteps, types, boundary, StepsPerSaving)
        {
            Kappa = kappa;
        }

        public double Kappa { get; private set; }
        protected override string CONST_FNAME => "ANNNI_3D_CLASSIC";

        private void SetAfterNeighbor_Direkt()
        {
            for (int i = 0; i < DimX; i++)
            {
                for (int k = 0; k < DimY; k++)
                {
                    for (int l = 0; l < DimZ; l++)
                    {
                        if (i + 2 < DimX)
                            (lattice[i, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i + 2, k, l]);
                        if (i > 1)
                            (lattice[i, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i - 2, k, l]);
                        if (k + 2 < DimY)
                            (lattice[i, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, k + 2, l]);
                        if (k > 1)
                            (lattice[i, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, k - 2, l]);
                        if (l + 2 < DimZ)
                            (lattice[i, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, k, l + 2]);
                        if (l > 1)
                            (lattice[i, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, k, l - 2]);
                    }
                }
            }
        }

        private void SetAfterNeighbor_Periodic()
        {
            if (DimX > 1)
                for (int k = 0; k < DimY; k++)
                {
                    for (int l = 0; l < DimZ; l++)
                    {
                        (lattice[0, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[DimX - 2, k, l]);
                        (lattice[DimX - 2, k, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[0, k, l]);
                    }
                }
            if (DimY > 1)
                for (int i = 0; i < DimX; i++)
                {
                    for (int l = 0; l < DimZ; l++)
                    {
                        (lattice[i, 0, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, DimY - 2, l]);
                        (lattice[i, DimY - 2, l] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, 0, l]);
                    }
                }
            if (DimZ > 1)
                for (int i = 0; i < DimX; i++)
                {
                    for (int k = 0; k < DimY; k++)
                    {
                        (lattice[i, k, 0] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, k, DimZ - 2]);
                        (lattice[i, k, DimZ - 2] as Spin_ANNNI_Classic).AddAfterNeighbor(lattice[i, k, 0]);
                    }
                }
        }

        private void SetAfterNeighbors()
        {
            switch (Boundary)
            {
                case ELatticeBoundary.None:
                    SetAfterNeighbor_Direkt();
                    break;

                case ELatticeBoundary.Periodic:
                    SetAfterNeighbor_Direkt();
                    SetAfterNeighbor_Periodic();
                    break;

                default:
                    throw new Exception("Not Valid Boundary for this Simulation!");
            }
        }

        protected override double H(Spin_Ising_Classic spin)
        {
            if (!(spin is Spin_ANNNI_Classic))
                return 0;
            double h = 0;
            foreach (Spin_ANNNI_Classic n in (spin as Spin_ANNNI_Classic).getAfterNeighbors())
            {
                h += spin.getComponent(0) * n.getComponent(0);
            }
            h *= J * Kappa;
            return h + base.H(spin);
        }

        protected override void Init()
        {
            base.Init();
            SetAfterNeighbors();
        }

        protected override void SaveParameterXML(ref XDocument xDocument)
        {
            base.SaveParameterXML(ref xDocument);
            xDocument.Root.Add(new XElement("Kappa", Kappa));
        }

        protected override void SeedLattice()
        {
            lattice = new Spin_ANNNI_Classic[DimX, DimY, DimZ];
            for (int i = 0; i < DimX; i++)
            {
                for (int k = 0; k < DimY; k++)
                {
                    for (int l = 0; l < DimZ; l++)
                    {
                        lattice[i, k, l] = new Spin_ANNNI_Classic(ParticleType, (uint)i, (uint)k, (uint)l);
                        if (rand.NextDouble() < 0.5)
                            lattice[i, k, l].Flip();
                    }
                }
            }
        }
    }
}