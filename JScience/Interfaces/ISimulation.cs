using System;

namespace JScience.Interfaces
{
    public interface ISimulation
    {
        string Bezeichnung { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }

        bool Finished { get; }

        void Start();

        void Stop();

        void Reset();
    }
}