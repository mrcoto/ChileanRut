namespace MrCoto.ChileanRut
{
    /// <summary>
    /// Operaciones con el Rut
    /// </summary>
    public partial class Rut
    {
        /// <summary>
        /// Definición de 'Menor que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es menor que y.</returns>
        public static bool operator <(Rut x, Rut y) => x.CompareTo(y)  < 0;
        /// <summary>
        /// Definición de 'Menor o igual que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es menor o igual que y.</returns>
        public static bool operator <=(Rut x, Rut y) => x.CompareTo(y)  <= 0;
        /// <summary>
        /// Definición de 'Mayor que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es mayor que y.</returns>
        public static bool operator >(Rut x, Rut y) => x.CompareTo(y)  > 0;
        /// <summary>
        /// Definición de 'Mayor o igual que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es mayor o igual que y.</returns>
        public static bool operator >=(Rut x, Rut y) => x.CompareTo(y)  >= 0;
        /// <summary>
        /// Definición de 'Igual que'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es igual que y.</returns>
        public static bool operator ==(Rut x, Rut y) => x.Equals(y);
        /// <summary>
        /// Definición de 'Distinto de'.
        /// </summary>
        /// <param name="x">Operando Izquierdo.</param>
        /// <param name="y">Operando Derecho.</param>
        /// <returns>Verdadero Si x es distinto de y.</returns>
        public static bool operator !=(Rut x, Rut y) => !x.Equals(y);
    }
}