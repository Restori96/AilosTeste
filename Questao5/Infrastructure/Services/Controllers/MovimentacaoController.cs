using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Net;
using System.Net.Http;
using Questao5.Domain.Entities;
using Questao5.Application.Commands.Requests;
namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : Controller
    {
        private readonly IDbConnection _dbConnection;

        public MovimentoController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Cria movimentação de conta corrente
        /// </summary>
        ///<returns>ID movimentação</returns>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Todo
        ///     {
        ///        "idContaCorrente": "string",
        ///        "tipoMovimento": "C",
        ///        "valor": 1234
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Movimentação realizada com sucesso</response>
        /// <response code="400">dados de parametro invalido</response>
        [HttpPost("movimentacao-conta-corrente")]
        public async Task<IActionResult> MovimentarContaCorrente([FromBody] MovimentoRequest request)
        {
            // Validação do tipo de movimento
            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                return BadRequest(new { Message = "Tipo de movimento inválido.", Type = "INVALID_TYPE" });

            // Validação de valor positivo
            if (request.Valor <= 0)
                return BadRequest(new { Message = "Valor inválido.", Type = "INVALID_VALUE" });

            // Verificar se a conta existe e está ativa
            var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdConta AND ativo = 1",
                new { IdConta = request.IdContaCorrente });

            if (conta == null)
                return BadRequest(new { Message = "Conta corrente inválida ou inativa.", Type = "INVALID_ACCOUNT" });

            // Persistir movimento
            var idMovimento = Guid.NewGuid().ToString();
            var dataMovimento = DateTime.Now.ToString("dd/MM/yyyy");
            var insertQuery = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                              "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            await _dbConnection.ExecuteAsync(insertQuery, new
            {
                IdMovimento = idMovimento,
                IdContaCorrente = request.IdContaCorrente,
                DataMovimento = dataMovimento,
                request.TipoMovimento,
                request.Valor
            });

            return Ok(new { IdMovimento = idMovimento });
        }
    }




}