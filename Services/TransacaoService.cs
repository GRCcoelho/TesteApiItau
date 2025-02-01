using Microsoft.AspNetCore.Http.HttpResults;
using TesteApiItau.Models;

namespace TesteApiItau.Services
{
    public class TransacaoService
    {
        public bool AdicionaTransacao(Dictionary<double, string> valoresMemoria, Transacao transacao)
        {
            try
            {
                valoresMemoria.Add(transacao.valor, transacao.dataHora);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}