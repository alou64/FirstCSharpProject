using NLog.Web;
using NLog;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);

  // register nlog as logging provider
  builder.Host.UseNLog();

  // Add services to the container.

  builder.Services.AddControllers()
    .AddNewtonsoftJson();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  var app = builder.Build();

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
}
catch (Exception ex)
{
  // catch setup errors
  logger.Error(ex, "Program stopped due to exception");
  throw;
}
finally
{
  NLog.LogManager.Shutdown();
}
