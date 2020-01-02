using System;
using System.ComponentModel.DataAnnotations;

namespace MrCoto.ChileanRut.Validation
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
        /// Formato de entrada del Rut.
        /// 
        /// Indica si debe ser 12.345.678-X, 12345678-X o 12345678X.
        /// </summary>
        /// <value>Formato que debe tener el Rut</value>
        public RutFormat RutFormat { get; set; } = RutFormat.FULL | RutFormat.ONLY_DASH | RutFormat.ESCAPED;

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

        private bool ValidateAsRutString(string rutStr)
        {
            try {
                var rut = Rut.Parse(rutStr);
                if (RutFormat != (RutFormat.FULL | RutFormat.ONLY_DASH | RutFormat.ESCAPED))
                {
                    return ValidateAsRutObject(rut) && rut.Format(RutFormat) == rutStr;
                }
                return ValidateAsRutObject(rut);
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