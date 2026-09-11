using DocXReports.BO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
ConfigHelper.Configuration = builder.Configuration;

var app = builder.Build();

// Swagger is always enabled — accessible at /swagger in all environments.
// In a containerised deployment there is no "Development" toggle; the API
// is internal (behind a reverse proxy) so exposing Swagger is safe.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocXReports v1");
    c.RoutePrefix = "swagger"; // http://host:5013/swagger
});

// HTTPS redirection is intentionally removed.
// TLS is terminated at the reverse proxy / load balancer layer.
// Inside the container the app speaks plain HTTP on port 5013.

app.UseAuthorization();

app.MapControllers();

app.Run();
