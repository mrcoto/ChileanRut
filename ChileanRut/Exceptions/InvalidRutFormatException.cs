using System;
namespace MrCoto.ChileanRut.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando un Rut es inválido (dígito verificador difiere del esperado)
    /// </summary>
    public class InvalidRutFormatException : Exception
    {
        /// <summary>
        /// Formato del Rut es inválido.
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <returns></returns>
        public InvalidRutFormatException(string message) : base(message) {}
    }
}