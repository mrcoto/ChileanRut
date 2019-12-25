using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ChileanRut
{
    public class Rut
    {
        private static readonly string RGX_NUMBER = @"^([1-9]\d?(\.?\d{3}){0,2}|[1-9]\d{0,2}(\.?\d{3})?)$";
        private static readonly string RGX_DV = @"^([0-9]|k|K)$";

        private int _number;
        private string _dv;

        public Rut(string number, string dv)
        {   
            Contract.Requires(Regex.Match(number, RGX_NUMBER).Success, "Formato Inválido");
            Contract.Requires(Regex.Match(dv, RGX_DV).Success, "Digito Verificador Inválido");
            _number = int.Parse(number.Replace(".", ""));
            _dv = dv.ToLower();
        }
        // TODO: static parse from rut string
        // TODO: static parse from number int
        // TODO: static calcDv
        // TODO: static random
        // TODO: static randoms
        // TODO: static uniques
        // TODO: isvalid (rut valido)
        // TODO: format (to string)
        // TODO: compareTo
        // TODO: Equals
        // TODO: toString
        // TODO: Destructuring
    }
}