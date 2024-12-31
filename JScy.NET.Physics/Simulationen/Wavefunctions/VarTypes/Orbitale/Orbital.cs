using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using JScy.NET.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.Classes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Analyse.VarTypes;
using JScy.NET.Physics.Simulationen.Wavefunctions.Enums;
using JScy.NET.Physics.Simulationen.Wavefunctions.Interfaces;
using JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.StandardWF;

namespace JScy.NET.Physics.Simulationen.Wavefunctions.VarTypes.Orbitale
{
    public class Orbital : IEquatable<Orbital>, IEquatable<IWavefunction>
    {
        public IWavefunction WF { get; internal set; }

        public string Bezeichnung => $"DIM{WF.Dimensions}_{OrbitalBezeichnung}{(Math.Sign(Spin) > 0 ? "-UP-" : "-DOWN-")}{Spin}";
        public readonly float Spin;
        public readonly EOrbitalLabel OrbitalBezeichnung;

        public readonly WFPlotter Plotter;

        #region Connection to other Orbitals

        private readonly List<Orbital> ConnectedOrbitals = new List<Orbital>();

        public void ConnectToOrbital(Orbital remote)
        {
            if (!ConnectedOrbitals.Contains(remote) && !remote.Equals(this))
            {
                ConnectedOrbitals.Add(remote);
                remote.ConnectToOrbital(this);
            }
        }

        public void RemoveConnectionFromOrbital(Orbital remote)
        {
            if (ConnectedOrbitals.Remove(remote))
                remote.RemoveConnectionFromOrbital(this);
        }

        #endregion Connection to other Orbitals

        #region Konstruktoren

        public Orbital(IWavefunction wF, float spin, EOrbitalLabel orbitalBezeichnung)
        {
            WF = wF;
            Spin = spin;
            OrbitalBezeichnung = orbitalBezeichnung;
            Plotter = new WFPlotter(this);
        }

        public Orbital(WFInfo wfinfo, ECalculationMethod method, float spin, EOrbitalLabel orbitalBezeichnung)
        {
            WF = (IWavefunction)Activator.CreateInstance(GetType(), wfinfo, method);
            Spin = spin;
            OrbitalBezeichnung = orbitalBezeichnung;
            Plotter = new WFPlotter(this);
        }

        #endregion Konstruktoren

        #region Cab

        internal readonly List<CabExit> CabExits = new List<CabExit>();

        public void AddExit(string ExitName, IWavefunction exitWF)
        {
            var found = CabExits.FirstOrDefault(x => x.ExitName.Equals(ExitName));
            if (found == null)
                CabExits.Add(new CabExit(ExitName, exitWF));
        }

        public void CreateCabExitAuto()
        {
            CabExits.Clear();
            CabExits.AddRange(WF.CreateCabExitAuto());
        }

        public Dictionary<string, Complex> CalcCab()
        {
            Dictionary<string, Complex> CabCalc = [];
            foreach (var exit in CabExits)
            {
                IWavefunction cal = WF.Clone() * exit.wavefunction.Conj();
                Complex calc = new Complex();
                for (int i = 0; i < cal.field.Length; i++)
                {
                    calc += cal.field[i];
                }

                CabCalc.Add(exit.ExitName, calc);
            }
            return CabCalc;
        }

        #endregion Cab

        #region Equatable

        public bool Equals(Orbital other) => other.Bezeichnung.Equals(Bezeichnung);

        public bool Equals(IWavefunction other) => WF.Dimensions == other.Dimensions;

        #endregion Equatable

        #region Rechenoperationen

        public static Orbital operator +(Orbital A, Orbital B)
        {
            A.WF += B.WF;
            return A;
        }

        public static Orbital operator -(Orbital A, Orbital B)
        {
            A.WF -= B.WF;
            return A;
        }

        public static Orbital operator *(Orbital A, Orbital B)
        {
            A.WF *= B.WF;
            return A;
        }

        #endregion Rechenoperationen
    }
}