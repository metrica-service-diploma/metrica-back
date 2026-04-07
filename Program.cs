using metrica_back.src.External;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Регистрация MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Регистрация AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
