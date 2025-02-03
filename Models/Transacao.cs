using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace TesteApiItau.Models
{
    public class Transacao/* : IValidatableObject*/
    {
        private long Id { get; set; }
        public double valor { get; set; }
        public string dataHora { get; set; }

        public string ValidaCampos(double valor, string data)
        {
            StringBuilder erros = new StringBuilder();

            string erroValor = ValidaValor(valor);
            if (!string.IsNullOrEmpty(erroValor))
                erros.AppendLine(erroValor);

            string erroData = ValidaDataHora(data);
            if(!string.IsNullOrEmpty(erroData))
                erros.AppendLine(erroData);

            return erros.ToString();
        }

        public long BuscaValorUltimoId(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            return valoresMemoria.Keys.Count;
        }

        private string ValidaValor(double valor)
        {
            if (valor < 0)
                return "O campo 'valor' não pode ser negativo!";

            return string.Empty;
        }

        private string ValidaDataHora(string data)
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

        private bool BeGreaterThanNow(string data)
        {
            if (BeFormattedISO(data))
            {
                if(DateTimeOffset.Now.CompareTo(DateTimeOffset.Parse(data)) == -1) // Se retornar -1 é porque a data recebida pela API é maior que a hora atual
                    return false;

                return true;
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
