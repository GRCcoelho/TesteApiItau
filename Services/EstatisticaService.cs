using System.Collections.Concurrent;
using System.Text.Json;
using TesteApiItau.Models;

namespace TesteApiItau.Services
{
    public class EstatisticaService
    {
        public string GerarEstatisticas(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            valoresMemoria = RetornaRegistrosUltimoMinuto(valoresMemoria);
            var response = new
            {
                count = Count(valoresMemoria),
                sum = Soma(valoresMemoria),
                avg = Media(valoresMemoria),
                min = MenorValor(valoresMemoria),
                max = MaiorValor(valoresMemoria)
            };

            string json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            return json;
        }

        private ConcurrentDictionary<long, Transacao> RetornaRegistrosUltimoMinuto(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            ConcurrentDictionary<long, Transacao> listaUltimoMinuto = new ConcurrentDictionary<long, Transacao>();
            DateTimeOffset dtAgora = DateTimeOffset.Now;
            DateTimeOffset dataRecebida = new DateTimeOffset();

            foreach (var item in valoresMemoria)
            {
                dataRecebida = DateTimeOffset.Parse(item.Value.dataHora);
                if (dataRecebida < DateTimeOffset.Now.AddMinutes(-10)) // Se a data for menor que o último minuto
                    valoresMemoria.TryRemove(item);
            }

            return valoresMemoria;
        }

        private int Count(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            return valoresMemoria.Count();
        }

        private double Soma(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            double soma = 0;
            foreach (var item in valoresMemoria)
                soma += item.Value.valor;

            return soma;
        }

        private double Media(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            double valores = Soma(valoresMemoria);
            double media = valores / valoresMemoria.Count();
            return media;

        }

        private double MenorValor(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            List<double> valores = new List<double>();

            foreach (var item in valoresMemoria)
                valores.Add(item.Value.valor);

            return valores.Min();
        }

        private double MaiorValor(ConcurrentDictionary<long, Transacao> valoresMemoria)
        {
            List<double> valores = new List<double>();

            foreach (var item in valoresMemoria)
                valores.Add(item.Value.valor);

            return valores.Max();
        }
    }
}
