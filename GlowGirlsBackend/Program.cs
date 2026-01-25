using GlowGirlsBackend.Configuration;
using GlowGirlsBackend.Extensions;
using GlowGirlsBackend.Swagger;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(o =>
{
    o.AddOperationTransformer(new ClientSecretTransformer());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();
app.UseClientSecretValidation();

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
