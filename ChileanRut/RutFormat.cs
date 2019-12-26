namespace MrCoto.ChileanRut
{
    /// <summary>
    /// Formatos de un Rut.
    /// 
    /// - [FULL]: Formato con puntos y guión (12.345.678-9)
    /// - [ONLY_DASH]: Formato solo sin puntos y con guión (12345678-9)
    /// - [ESCAPED]: Formato sin puntos ni guión (123456789)
    /// </summary>
    public enum RutFormat
    {
        
        /// <summary>
        /// [FULL]: Formato con puntos y guión (12.345.678-9)
        /// </summary>
        FULL,
        /// <summary>
        /// [ONLY_DASH]: Formato solo sin puntos y con guión (12345678-9)
        /// </summary>
        ONLY_DASH,
        /// <summary>
        /// [ESCAPED]: Formato sin puntos ni guión (123456789)
        /// </summary>
        ESCAPED
    }
}