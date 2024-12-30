namespace JScience.Enums
{
    /// <summary>
    /// Enum für technische Berechnungsmethoden-
    /// </summary>
    public enum ECalculationMethod
    {
        /// <summary>
        /// Keine spezifische Methode.
        /// </summary>
        None = 0,

        /// <summary>
        /// Single-CPU-Nutzung.
        /// </summary>
        CPU = 1,

        /// <summary>
        /// Nutzung von Multithreading auf CPU.
        /// </summary>
        CPU_Multihreading = 2,

        /// <summary>
        /// Nutzung von OpenCL.
        /// </summary>
        OpenCL = 3,

        /// <summary>
        /// Nutzung von NVIDIA CUDA.
        /// </summary>
        CUDA = 4
    }
}