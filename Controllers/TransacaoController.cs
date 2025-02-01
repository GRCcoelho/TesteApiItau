using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TesteApiItau.Models;
using TesteApiItau.Services;

namespace TesteApiItau.Controllers
{
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        private static ConcurrentDictionary<long, Transacao> valoresMemoria = new ConcurrentDictionary<long, Transacao>();
        private TransacaoService _transacaoService = new TransacaoService();

        #region Método GET
        [HttpGet("get/transacao")]
        public IActionResult GetTransacao()
        {
            // Retorna todos os itens armazenados em memória
            return Ok(valoresMemoria);
        }
        #endregion

        #region Método POST
        [HttpPost("post/transacao")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostTransacao(Transacao transacao)
        {
            try
            {
                string erros = transacao.ValidaCampos(transacao.valor, transacao.dataHora);
                if (!string.IsNullOrEmpty(erros))
                    return UnprocessableEntity(erros);

                if (_transacaoService.AdicionaTransacao(valoresMemoria, transacao))
                    return CreatedAtAction("PostTransacao", transacao);
                else
                    return BadRequest("Ocorreu um problema ao armazenar as informações.");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        [HttpDelete("delete/transacao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteTransacao()
        {
            if (valoresMemoria.Any())
                valoresMemoria = new ConcurrentDictionary<long, Transacao>();
            else
                return BadRequest("Não existem transações para excluir.");
            return Ok("Todas as informações foram apagadas com sucesso");
        }
    }
}
