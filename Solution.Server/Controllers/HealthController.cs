using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solution.Infra.Data.Context;

namespace Solution.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IDbConnectionFactory _factory;
        public HealthController(IDbConnectionFactory factory) => _factory = factory;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var conn = _factory.Create();              // ← abre SQLite com PRAGMAs
            var one = await conn.ExecuteScalarAsync<long>("select 1");
            return Ok(new { ok = (one == 1) });
        }
    }
}
