namespace MrCoto.ChileanRut
{
    /// <summary>
    /// Constantes de clase
    /// </summary>
    public partial class Rut
    {
        private static readonly string RGX_NUMBER = @"^([1-9]\d?(\.?\d{3}){0,2}|[1-9]\d{0,2}(\.?\d{3})?)$";
        private static readonly string RGX_DV = @"^([0-9]|k|K)$";

        private const int ELEVEN = 11;
        private const int TEN = 10;
        private const string ZERO = "0";
        private const string K = "k";

        private const int MIN_RANGE = 4_000_000;
        private const int MAX_RANGE = 80_000_000;
    }
}