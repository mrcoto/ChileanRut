using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MrCoto.ChileanRut.Validation
{
    /// <summary>
    /// Valida un Rut dada su parte numérica y dígito verificador, valida que el dv calculado de la 
    /// parte numérica coincida con el dígito verificador del otro campo.
    /// </summary>
    public class ValidRutDvAttribute : ValidationAttribute
    {

        /// <summary>
        /// Largo mínimo que debe poseer la parte numérica del Rut.
        /// </summary>
        /// <value>Largo Mínimo de la Parte Numérica del Rut.</value>
        public int MinLength { get; set; } = 0;

        /// <summary>
        /// Nombre del atributo/columna/campo donde se encuentra la parte numérica dentro del objeto
        /// donde se está validando este atributo.
        /// <para>
        /// Por ejemplo:
        /// public class MiClase
        /// {
        ///     public int UnCampo { get; set; }    
        /// 
        ///     [ValidRutDv(NumberName = "UnCampo")]
        ///     public string Dv { get; set; }    
        /// }
        /// 
        /// Tomará en cuenta que la parte numérica es el valor almacenado en <c>UnCampo</c>.
        /// 
        /// La visibilidad DEBE ser <c>public</c>
        /// </para>
        /// </summary>
        /// <value>Nombre de la columna/atributo/campo donde se encuentra almacenado la parte numérica del Rut.</value>
        public string NumberName { get; set; } = string.Empty;

        /// <summary>
        /// Constructor del validador.
        /// </summary>
        public ValidRutDvAttribute()
        {
            ErrorMessage = "Rut Inválido";
        }
        
        /// <summary>
        /// Verifica si el dv de la parte numérica equivale al dv de este valor.
        /// </summary>
        /// <param name="value">Dígito verificador.</param>
        /// <param name="validationContext">Contexto de validación.</param>
        /// <returns>Resultado de validación entre parte numérica y dígito verificador.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var numberPart = GetNumberPartValue(validationContext);
            if (!(numberPart is int) && !(numberPart is string)) return BadResult(validationContext);
            var validRutAttribute = new ValidRutAttribute() { MinLength = MinLength, ErrorMessage = ErrorMessage };
            if (validRutAttribute.IsValid($"{numberPart}-{value}")) return ValidationResult.Success;
            return BadResult(validationContext);
        }

        private ValidationResult BadResult(ValidationContext validationContext)
        {
            return new ValidationResult(ErrorMessage, GetMemberNames(validationContext));
        }

        private List<string> GetMemberNames(ValidationContext validationContext)
        {
            var memberNames = new List<string>();
            memberNames.Add(validationContext.MemberName);
            return memberNames;
        }

        private object GetNumberPartValue(ValidationContext validationContext)
        {
            return validationContext.ObjectInstance.GetType()
                    .GetProperty(NumberName, BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(validationContext.ObjectInstance);
        }

    }
}