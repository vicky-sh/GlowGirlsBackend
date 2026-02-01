using GlowGirlsBackend.Configuration;
using GlowGirlsBackend.Extensions;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowGlowGirlsFrontend",
        policy =>
        {
            policy.WithOrigins("https://glowgirlsparlour.com", "http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        }
    );
});
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();
app.UseCors("AllowGlowGirlsFrontend");

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

//}

app.UseHangfireDashboard(
    "/hangfire",
    new DashboardOptions
    {
        Authorization = [new HangfireDashboardAuthorizationFilter(builder.Configuration)],
    }
);

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
