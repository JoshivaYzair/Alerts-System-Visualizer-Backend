using Alerts.Logic.Authorization;
using Alerts.Logic.EventController;
using Alerts.Logic.HubController;
using Alerts.Logic.Interface;
using Alerts.Logic.Repository;
using Alerts.Logic.Security;
using Alerts.Logic.Service;
using Alerts.Persistence.Data;
using Alerts.Persistence.Model;
using Alerts.Persistence.Model.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddSingleton<EventAggregator>();
builder.Services.AddSingleton<EventSubscriber>();

builder.Services.AddScoped<LoginService>();
builder.Services.AddTransient<JwtService>();

builder.Services.AddDbContext<AlertsDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("AlertsDB")));

builder.Services.AddScoped<IGenericRepository<Alert>, GenericRepository<Alert>>();
builder.Services.AddScoped<IGenericRepository<Application>, GenericRepository<Application>>();
builder.Services.AddScoped<AlertService>();
builder.Services.AddScoped<ApplicationService>();

builder.Services.AddSingleton<UserRolePermissionRepository>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>( 
    sp => new PermissionAuthorizationHandler(
            sp.GetRequiredService<UserRolePermissionRepository>()
        )); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference{
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<String>()
        }
    });
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy(Permission.Create.ToString(), policy => policy.RequireRole(Role.Administrator.ToString(), Role.User.ToString()));
//    options.AddPolicy(Permission.Read.ToString(), policy => policy.RequireRole(Role.Administrator.ToString(), Role.User.ToString()));
//    options.AddPolicy(Permission.Update.ToString(), policy => policy.RequireRole(Role.Administrator.ToString()));
//    options.AddPolicy(Permission.Delete.ToString(), policy => policy.RequireRole(Role.Administrator.ToString()));
//});

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Enum.GetValues(typeof(Permission)).Cast<Permission>())
    {
        options.AddPolicy(
            permission.ToString(),
            policy => policy.Requirements.Add(new PermissionRequirement(permission))
            );
    }
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("NuevaPolitica");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<NotificationHub>("/signalR");
});

app.MapControllers();

app.Run();
