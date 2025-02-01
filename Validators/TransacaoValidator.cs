using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;
using TesteApiItau.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TesteApiItau.Validators
{
    public class TransacaoValidator : AbstractValidator<Transacao>
    {
        public TransacaoValidator()
        {
            RuleFor(trs => trs.valor)
                .NotNull().WithMessage("O campo 'Valor' é obrigatório!")
                .GreaterThan(-1).WithMessage("O valor não pode ser negativo!");

            RuleFor(trs => trs.dataHora)
                .NotEmpty().WithMessage("O campo 'Data e hora' é obrigatório!.")
                .Must(p => BeGreaterThanNow(p)).WithMessage("A data informada não pode ser maior que a data atual.")
                .Must(p => BeFormattedISO(p)).WithMessage("A data informada deve estar no formato ISO 8601.");
        }

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
