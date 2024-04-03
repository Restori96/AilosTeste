// Em um arquivo de configuração de serviços personalizado (por exemplo, ServicesConfig.cs)

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using System.Data;

public static class ServicesConfig
{
    public static void Configure(IServiceCollection services)
    {
        // Configurar a conexão com o SQLite
        services.AddScoped<IDbConnection>((serviceProvider) =>
        {
            return new SqliteConnection("YourConnectionStringHere");
        });
    }
}