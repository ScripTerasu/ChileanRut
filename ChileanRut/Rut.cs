using System;
using System.Text.RegularExpressions;

namespace MrCoto.ChileanRut
{
    /// <summary>
    /// Rut Chileno.
    /// </summary>
    public partial class Rut : IComparable<Rut>
    {
        private int _number;
        private string _dv;

        /// <summary>
        /// Constructor principal.
        /// </summary>
        /// <param name="number">Parte númerica de un rut (de 123-k, la parte númerica es "123").</param>
        /// <param name="dv">Dígito verificador de un rut.</param>
        public Rut(string number, string dv)
        {   
            if (!Regex.Match(number, RGX_NUMBER).Success) throw new ArgumentException("Formato Inválido");
            if (!Regex.Match(dv, RGX_DV).Success) throw new ArgumentException("Digito Verificador Inválido");
            _number = int.Parse(number.Replace(".", ""));
            _dv = dv.ToLower();
        }

        /// <summary>
        /// Valida un Rut.
        /// </summary>
        /// <returns>Verdadero si el RUT es válido.</returns>
        public bool IsValid() => CalcDv(_number) == _dv;
        
        /// <summary>
        /// Formatea el RUT.
        /// 
        /// - [RUTFORMAT.FULL] -> RUT con puntos y guión (12.345.678-k)
        /// - [RUTFORMAT.ONLY_DASH] -> RUT sin puntos y con guión (12345678-k)
        /// - [RUTFORMAT.ESCAPED] -> RUT sin puntos y guión (12345678k)
        /// </summary>
        /// <param name="format">Formato del RUT</param>
        /// <returns>String del Rut formateado.</returns>
        public string Format(RutFormat format = RutFormat.FULL) => format switch
        {
            RutFormat.FULL => $"{_number:n0}-{_dv}".Replace(",", "."),
            RutFormat.ONLY_DASH => $"{_number}-{_dv}",
            RutFormat.ESCAPED => $"{_number}{_dv}",
            _ => this.Format() 
        };

        /// <summary>
        /// Compara esta instancia de Rut con otra,
        /// </summary>
        /// <param name="other">La otra instancia de Rut.</param>
        /// <returns>Valor de comparación.</returns>
        public int CompareTo(Rut other) => _number.CompareTo(other._number);

        /// <summary>
        /// Igualdad estructural.
        /// </summary>
        /// <param name="obj">Otra Instancia (o null).</param>
        /// <returns>Verdadero si son un Rut es igual a otro (estructuralmente).</returns>
        #nullable enable
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Rut))
            {
                return false;
            }
            var other = (Rut) obj;
            return other._number == _number && other._dv == _dv;
        }
        #nullable disable

        /// <summary>
        /// Hash Code.
        /// </summary>
        /// <returns>Valor hash.</returns>
        public override int GetHashCode() => 31 * _number + _dv.GetHashCode();

        /// <summary>
        /// Representación de un Rut.
        /// </summary>
        /// <returns>Representación en string de un Rut.</returns>
        public override string ToString() => $"Rut({_number}, {_dv})";

        /// <summary>
        /// Destructuración de un Rut.
        /// </summary>
        /// <param name="number">Parte numérica del Rut.</param>
        /// <param name="dv">Dígito verificador del Rut.</param>
        public void Deconstruct(out int number, out string dv)
        {
            number = _number;
            dv = _dv;
        }

    }
}