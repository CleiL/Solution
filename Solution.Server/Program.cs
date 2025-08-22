using Solution.Infra.Data.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfraData(builder.Configuration);

var app = builder.Build();

{
    var cs = builder.Configuration["Database:ConnectionString"] ?? "Data Source=./data/agenda.db";
    const string pfx = "Data Source=";
    var dbPath = cs.StartsWith(pfx, StringComparison.OrdinalIgnoreCase) ? cs[pfx.Length..].Trim() : cs;
    var dir = Path.GetDirectoryName(Path.GetFullPath(dbPath));
    if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
