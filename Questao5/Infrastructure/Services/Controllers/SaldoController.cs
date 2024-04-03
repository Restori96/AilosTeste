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
namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaldoController : Controller
    {
        private readonly IDbConnection _dbConnection;

        public SaldoController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        /// <summary>
        /// Busca dados da conta
        /// </summary>
        ///  <remarks>
        /// Exemplo:
        ///
        ///     GET /{idConta}/saldo
        ///     {
        ///        "idConta": "string",
        ///       
        ///     }
        ///
        /// </remarks>
        ///<returns>Retorna dados da conta</returns>
        /// <response code="200">Consulta realizada com sucesso</response>
        /// <response code="400">dado de parametro invalido</response>
        [HttpGet("{idConta}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(string idConta)
        {
            // Verificar se a conta existe e está ativa
            var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                "SELECT * FROM contacorrente WHERE idcontacorrente = @IdConta AND ativo = 1",
                new { IdConta = idConta });

            if (conta == null)
                return BadRequest(new { Message = "Conta corrente inválida ou inativa.", Type = "INVALID_ACCOUNT" });

            // Calcular saldo
            var saldo = await _dbConnection.QueryFirstOrDefaultAsync<decimal?>(
                "SELECT SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE -valor END) AS Saldo " +
                "FROM movimento WHERE idcontacorrente = @IdConta",
                new { IdConta = idConta });

            var saldoAtual = saldo ?? 0;

            return Ok(new
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataHoraConsulta = DateTime.Now,
                SaldoAtual = saldoAtual
            });
        }
    }
}