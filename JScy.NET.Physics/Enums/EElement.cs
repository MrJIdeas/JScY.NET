using JScy.NET.Physics.AttributesCustom;

namespace JScy.NET.Physics.Enums
{
    /// <summary>
    /// Enum für Elemente.
    /// </summary>
    public enum EElement : uint
    {
        [ElementSymbol("n")]
        [ElementKategorie(EElementkategorie.ChemieUnbekannt)]
        Neutron = 0,

        [ElementSymbol("H")]
        [ElementKategorie(EElementkategorie.Nichtmetalle)]
        Wasserstoff = 1,

        [ElementSymbol("He")]
        [ElementKategorie(EElementkategorie.Edelgasse)]
        Helium = 2,

        [ElementKategorie(EElementkategorie.Alkalimetall)]
        [ElementSymbol("Li")]
        Lithium = 3,

        [ElementSymbol("Be")]
        [ElementKategorie(EElementkategorie.Erdalkalimetall)]
        Berillium = 4,

        [ElementSymbol("B")]
        [ElementKategorie(EElementkategorie.Halbmetalle)]
        Bor = 5,

        [ElementSymbol("C")]
        [ElementKategorie(EElementkategorie.Nichtmetalle)]
        Kohlenstoff = 6,

        [ElementSymbol("N")]
        [ElementKategorie(EElementkategorie.Nichtmetalle)]
        Stickstoff = 7,

        [ElementSymbol("O")]
        [ElementKategorie(EElementkategorie.Nichtmetalle)]
        Sauerstoff = 8,

        [ElementSymbol("F")]
        [ElementKategorie(EElementkategorie.Halogene)]
        Fluor = 9,

        [ElementSymbol("Ne")]
        [ElementKategorie(EElementkategorie.Edelgasse)]
        Neon = 10,

        [ElementSymbol("Na")]
        [ElementKategorie(EElementkategorie.Alkalimetall)]
        Natrium = 11,

        [ElementSymbol("Mg")]
        [ElementKategorie(EElementkategorie.Erdalkalimetall)]
        Magnesium = 12,

        [ElementSymbol("Al")]
        [ElementKategorie(EElementkategorie.Metalle)]
        Aluminium = 13,

        [ElementSymbol("Si")]
        [ElementKategorie(EElementkategorie.Halbmetalle)]
        Silicium = 14,

        [ElementSymbol("P")]
        [ElementKategorie(EElementkategorie.Nichtmetalle)]
        Phosphor = 15,

        [ElementSymbol("S")]
        [ElementKategorie(EElementkategorie.Nichtmetalle)]
        Schwefel = 16,

        [ElementSymbol("Cl")]
        [ElementKategorie(EElementkategorie.Halogene)]
        Chlor = 17,

        [ElementSymbol("Ar")]
        [ElementKategorie(EElementkategorie.Edelgasse)]
        Argon = 18,
    }
}