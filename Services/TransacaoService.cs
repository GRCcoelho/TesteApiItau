using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http.HttpResults;
using TesteApiItau.Models;

namespace TesteApiItau.Services
{
    public class TransacaoService
    {
        public bool AdicionaTransacao(ConcurrentDictionary<long, Transacao> valoresMemoria, Transacao transacao)
        {
            try
            {
                valoresMemoria[transacao.BuscaValorUltimoId(valoresMemoria) + 1] = transacao;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}