using Microsoft.AspNetCore.Mvc;
using TesteApiItau.Models;
using TesteApiItau.Services;

namespace TesteApiItau.Controllers
{
    [ApiController]
    public class TransacaoController : ControllerBase
    {
        Dictionary<double, string> valoresMemoria;
        private TransacaoService _transacaoService;

        [HttpPost("transacao")]
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
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok("Valores armazenados na memória com sucesso");
        }
    }
}
