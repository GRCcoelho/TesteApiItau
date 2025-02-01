using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace TesteApiItau.Models
{
    public class Transacao/* : IValidatableObject*/
    {
        public double valor { get; set; }
        public string dataHora { get; set; }





        public string ValidaCampos(double valor, string data)
        {
            StringBuilder erros = new StringBuilder();

            string erroValor = ValidaValor(valor);
            if (!string.IsNullOrEmpty(erroValor))
                erros.AppendLine(ValidaValor(valor));

            string erroData = ValidaDataHora(data);
            if(!string.IsNullOrEmpty(erroData))
                erros.AppendLine(ValidaDataHora(data));

            return erros.ToString();
        }

        public string ValidaValor(double valor)
        {
            if (valor < 0)
                return "O campo 'valor' não pode ser negativo!";

            return string.Empty;
        }

        public string ValidaDataHora(string data)
        {
            StringBuilder erros = new StringBuilder();

            if (string.IsNullOrEmpty(data))
                erros.AppendLine("O campo 'dataHora' é obrigatório");

            if (!BeGreaterThanNow(data))
                erros.AppendLine("O campo 'dataHora' não pode ter valor futuro");

            if (!BeFormattedISO(data))
                erros.AppendLine("O campo 'dataHora' deve estar no formato ISO 8601");

            return erros.ToString();
        }



        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (this.valor < 0)
        //        yield return new ValidationResult("O campo 'valor' não pode ser negativo!", new[] { nameof(valor) });

        //    if (string.IsNullOrEmpty(this.dataHora))
        //        yield return new ValidationResult("O campo 'dataHora' é obrigatório", new[] { nameof(dataHora) });

        //    if (!BeFormattedISO(this.dataHora))
        //        yield return new ValidationResult("O campo 'dataHora' deve estar no formato ISO 8601", new[] { nameof(dataHora) });

        //    if (!BeGreaterThanNow(this.dataHora))
        //        yield return new ValidationResult("O campo 'dataHora' não pode ter valor futuro", new[] { nameof(dataHora) });
        //}

        private bool BeGreaterThanNow(string data)
        {
            if (BeFormattedISO(data))
            {
                if (DateTime.TryParse(data, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime dataConvertida))
                {
                    return dataConvertida <= DateTime.UtcNow;
                }
                return false;
            }
            else
            {
                return true; // Caso não estiver formatado, retorna true para não ativar exceção de BeGreaterThanNow
            }
        }

        private bool BeFormattedISO(string data)
        {
            string regexISO = @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z|[+-]\d{2}:\d{2})";
            if (Regex.IsMatch(data, regexISO))
                return true;
            return false;
        }
    }
}
