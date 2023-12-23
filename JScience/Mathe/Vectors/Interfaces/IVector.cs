using JScience.Mathe.Vectors.Enums;

namespace JScience.Mathe.Vectors.Interfaces
{
    public interface IVector
    {
        string Bezeichnung { get; }
        int Dimensions { get; }

        double Abs2 { get; }
        double Norm { get; }

        EVecType VectorType { get; }
    }
}