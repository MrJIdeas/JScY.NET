namespace JScy.NET.Interfaces
{
    /// <summary>
    /// Interface zum Speichern von Mess- und Simulationsdaten.
    /// </summary>
    public interface IDataSave
    {
        /// <summary>
        /// Methode zum Speichern der Daten.
        /// </summary>
        void SaveSimulationData();

        /// <summary>
        /// Methode zum Plotten der Daten.
        /// </summary>
        void PlotData();
    }
}