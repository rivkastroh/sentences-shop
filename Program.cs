using ListSentence.interfases;
using ListSentence.services;
using Middelware;
using Serilog;

Log.Logger = new LoggerConfiguration()
     .WriteTo.Console()
    .WriteTo.DateFormatPath(
        pathFormat: "Logs/log-{date:format=yyyy-MM-dd}.txt"
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISetenceService, SentenceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseFirstMiddelware();
app.UseLoger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
