using System;
using System.ComponentModel.DataAnnotations;
using MrCoto.ChileanRut;

namespace ChileanRut.Validation
{
    /// <summary>
    /// Valida si un atributo de tipo <c>string</c> o <c>MrCoto.ChileanRut.Rut</c> es un Rut Válido.
    /// </summary>
    public class ValidRutAttribute : ValidationAttribute
    {
        /// <summary>
        /// Largo mínimo que debe poseer la parte numérica del Rut.
        /// </summary>
        /// <value>Largo Mínimo de la Parte Numérica del Rut.</value>
        public int MinLength { get; set; } = 0;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        public ValidRutAttribute()
        {
            ErrorMessage = "Rut Inválido";
        }
        
        /// <summary>
        /// Retorna Verdadero si el Rut es válido.
        /// </summary>
        /// <param name="value">Rut como string u el objeto Rut.</param>
        /// <returns>Si el Rut es válido.</returns>
        public override bool IsValid(object value)
        {
            if (value is string) return ValidateAsRutString(value as string);
            if (value is Rut) return ValidateAsRutObject(value as Rut);
            return false;
        }

        private bool ValidateAsRutString(string rut)
        {
            try {
                return ValidateAsRutObject(Rut.Parse(rut));
            } catch (ArgumentException) {
                return false;
            }
        }

        private bool ValidateAsRutObject(Rut rut)
        {
            var (num, _) = rut;
            return rut.IsValid() && num.ToString().Length >= MinLength;
        }
    }
}