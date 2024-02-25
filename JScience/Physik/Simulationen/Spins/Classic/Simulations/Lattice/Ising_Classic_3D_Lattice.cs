using JScience.Interfaces;
using JScience.Physik.Enums;
using JScience.Physik.Simulationen.Spins.Classic.Interfaces;
using JScience.Physik.Simulationen.Spins.Classic.VarTypes;
using JScience.Physik.Simulationen.Spins.Enums;
using System;
using System.Data;
using System.IO;
using System.Xml.Linq;

namespace JScience.Physik.Simulationen.Spins.Classic.Simulations.Lattice
{
    public class Ising_Classic_3D_Lattice : ISimulation, ISpinSimulation
    {
        protected virtual string CONST_FNAME => "ISING_3D_CLASSIC";

        public Ising_Classic_3D_Lattice(double j, double b, double t, uint dimX, uint dimY, uint dimZ, uint MaxSteps, EParticleType types, ELatticeBoundary boundary, uint StepsPerSaving)
        {
            this.MaxSteps = MaxSteps;
            J = j;
            this.StepsPerSaving = StepsPerSaving;
            Boundary = boundary;
            ParticleType = types;
            DimX = dimX;
            DimY = dimY;
            DimZ = dimZ;
            B = b;
            if (t <= 0)
                T = double.Epsilon * 2;
            else
                T = t;
            rand = new Random(DateTime.Now.Millisecond);
            Init();
        }

        protected virtual void Init()
        {
            SeedLattice();
            SetNeighbors();
            SetDatatable();
        }

        protected virtual void SeedLattice()
        {
            lattice = new Spin_Ising_Classic[DimX, DimY, DimZ];
            for (uint i = 0; i < DimX; i++)
            {
                for (uint k = 0; k < DimY; k++)
                {
                    for (uint l = 0; l < DimZ; l++)
                    {
                        lattice[i, k, l] = new Spin_Ising_Classic(ParticleType, i, k, l);
                        if (rand.NextDouble() < 0.5)
                            lattice[i, k, l].Flip();
                    }
                }
            }
        }

        private void SetNeighbor_Direkt()
        {
            for (uint i = 0; i < DimX; i++)
            {
                for (uint k = 0; k < DimY; k++)
                {
                    for (uint l = 0; l < DimZ; l++)
                    {
                        if (i < DimX - 1)
                            lattice[i, k, l].AddNeighbor(lattice[i + 1, k, l]);
                        if (i > 0)
                            lattice[i, k, l].AddNeighbor(lattice[i - 1, k, l]);
                        if (k < DimY - 1)
                            lattice[i, k, l].AddNeighbor(lattice[i, k + 1, l]);
                        if (k > 0)
                            lattice[i, k, l].AddNeighbor(lattice[i, k - 1, l]);
                        if (l < DimZ - 1)
                            lattice[i, k, l].AddNeighbor(lattice[i, k, l + 1]);
                        if (l > 0)
                            lattice[i, k, l].AddNeighbor(lattice[i, k, l - 1]);
                    }
                }
            }
        }

        private void SetNeighbor_Periodic()
        {
            for (uint k = 0; k < DimY; k++)
            {
                for (uint l = 0; l < DimZ; l++)
                {
                    lattice[0, k, l].AddNeighbor(lattice[DimX - 1, k, l]);
                    lattice[DimX - 1, k, l].AddNeighbor(lattice[0, k, l]);
                }
            }
            for (uint i = 0; i < DimX; i++)
            {
                for (uint l = 0; l < DimZ; l++)
                {
                    lattice[i, 0, l].AddNeighbor(lattice[i, DimY - 1, l]);
                    lattice[i, DimY - 1, l].AddNeighbor(lattice[i, 0, l]);
                }
            }
            for (uint i = 0; i < DimX; i++)
            {
                for (uint k = 0; k < DimY; k++)
                {
                    lattice[i, k, 0].AddNeighbor(lattice[i, k, DimZ - 1]);
                    lattice[i, k, DimZ - 1].AddNeighbor(lattice[i, k, 0]);
                }
            }
        }

        private void SetNeighbors()
        {
            switch (Boundary)
            {
                case ELatticeBoundary.None:
                    SetNeighbor_Direkt();
                    break;

                case ELatticeBoundary.Periodic:
                    SetNeighbor_Direkt();
                    SetNeighbor_Periodic();
                    break;

                default:
                    throw new Exception("Not Valid Boundary for this Simulation!");
            }
        }

        private void SetDatatable()
        {
            dt = new DataTable(CONST_FNAME);
            dt.Columns.Add("n", typeof(ulong));
            dt.Columns.Add("H", typeof(double));
            for (int i = 0; i < lattice.Length; i++)
                dt.Columns.Add("S" + i, typeof(short));
        }

        public string Bezeichnung => "Classic 3D Ising Model Simulation";
        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public bool Finished { get; private set; }
        public uint StepsPerSaving { get; private set; }

        public ulong n { get; private set; }

        public double J { get; private set; }

        public double B { get; private set; }

        public double T { get; private set; }

        public uint DimX { get; private set; }

        public uint DimY { get; private set; }
        public uint DimZ { get; private set; }

        public uint MaxSteps { get; private set; }

        public uint DimLength => (uint)lattice.Length;

        public EMagnetiseType MagType => J == 0 ? EMagnetiseType.None : J > 0 ? EMagnetiseType.Ferro : EMagnetiseType.Antiferro;
        public EParticleType ParticleType { get; private set; }
        public ELatticeBoundary Boundary { get; private set; }
        protected Spin_Ising_Classic[,,] lattice { get; set; }

        protected Random rand { get; private set; }

        private DataTable dt { get; set; }

        public void Reset()
        {
            Finished = false;
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            SeedLattice();
        }

        public void SaveSimulationData()
        {
            string dirpath = "." + Path.DirectorySeparatorChar + "Simulations" + Path.DirectorySeparatorChar + CONST_FNAME + Path.DirectorySeparatorChar;
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            string fname = CONST_FNAME + StartDate.ToString("yyyyMMdd_hhmmss");
            dt.WriteXmlSchema(dirpath + fname + "_schema.xml");
            dt.WriteXml(dirpath + fname + ".xml");
            XDocument xDocument = new XDocument();
            SaveParameterXML(ref xDocument);
            xDocument.Save(dirpath + fname + "_parameters.xml");
        }

        protected virtual void SaveParameterXML(ref XDocument xDocument)
        {
            xDocument.Add(new XElement("Parameters"));
            xDocument.Root.Add(new XElement("J", J));
            xDocument.Root.Add(new XElement("B", B));
            xDocument.Root.Add(new XElement("T", T));
            xDocument.Root.Add(new XElement("n", n));
            xDocument.Root.Add(new XElement("Dim", DimLength));
            xDocument.Root.Add(new XElement("DimX", DimX));
            xDocument.Root.Add(new XElement("DimY", DimY));
            xDocument.Root.Add(new XElement("DimZ", DimZ));
            xDocument.Root.Add(new XElement("Startdate", StartDate));
            xDocument.Root.Add(new XElement("Enddate", EndDate));
        }

        public void Start()
        {
            n = 0;
            for (int i = 0; i < DimX; i++)
            {
                Console.Write(H(lattice[i, 0, 0]) + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < DimX; i++)
            {
                Console.Write(lattice[i, 0, 0].Spin);
            }
            Console.WriteLine();
            StartDate = DateTime.Now;
            EndDate = DateTime.MinValue;
            while (MaxSteps > n)
            {
                Sim();
            }
            Stop();

            for (int i = 0; i < DimX; i++)
            {
                Console.Write(lattice[i, 0, 0].Spin);
            }
            Console.WriteLine();
            for (int i = 0; i < DimX; i++)
            {
                Console.Write(H(lattice[i, 0, 0]) + " ");
            }
            Console.WriteLine();
        }

        public void Stop()
        {
            EndDate = DateTime.Now;
            SaveSimulationData();
            Finished = true;
        }

        private void Sim()
        {
            int r = rand.Next(0, (int)DimX);
            int k = rand.Next(0, (int)DimY);
            int c = rand.Next(0, (int)DimZ);
            var Hold = H(lattice[r, k, c]);
            lattice[r, k, c].Flip();
            var Hnew = H(lattice[r, k, c]);
            if (Hnew > Hold)
            {
                if (T == 0 || Math.Exp(-(Hnew - Hold) / T) < rand.NextDouble())
                    lattice[r, k, c].Flip();
            }
            n++;
            if (n == 0 || n % StepsPerSaving == 0)
            {
                var row = dt.NewRow();
                row[0] = n;
                row[1] = H();
                for (uint i = 0; i < DimX; i++)
                {
                    for (uint l = 0; l < DimY; l++)
                    {
                        for (uint q = 0; l < DimY; l++)
                        {
                            uint u = i + DimX * l + DimY * q;
                            row[(int)u + 2] = lattice[i, l, q].getComponent(0);
                        }
                    }
                }

                dt.Rows.Add(row);
            }
        }

        protected virtual double H(Spin_Ising_Classic spin)
        {
            double h = 0;
            foreach (Spin_Ising_Classic n in spin.getNeighbors())
            {
                h += n.getComponent(0);
            }
            h *= -J * spin.getComponent(0);
            h -= B * spin.getComponent(0);
            return h;
        }

        public double H()
        {
            double h = 0;
            foreach (var spin in lattice)
                h += H(spin);
            return h;
        }

        public virtual void PlotData()
        {
        }
    }
}