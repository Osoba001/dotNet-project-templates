

using Auth.Application.Models;
using Auth.Persistence;
using Auth.Persistence.Data;
using KO.WebAPI.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var config = builder.Configuration;
// Add services to the container.
builder.Services.AddCors(opt =>
{

    opt.AddDefaultPolicy(x =>
    {
        x.AllowAnyOrigin()
        //.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
        //.WithHeaders("Authorization")
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE");
    });
});

string authSecretKey = config.GetSection("AuthConfigData:SecretKey").Value!;
builder.Services.DataServiceCollection(authSecretKey);
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.Configure<AuthConfigData>(config.GetSection(nameof(AuthConfigData)));
builder.Services.AddSqlServer<AuthDbContext>(config.GetConnectionString("AuthConString"), opt =>
{
    opt.EnableRetryOnFailure(1,TimeSpan.FromSeconds(1),null);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

////Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KOTemplate API V1");
    if (!app.Environment.IsDevelopment())
        c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.MapControllers();

app.Run();
