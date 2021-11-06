using CORE.Users.Configuration;
using CORE.Users.Interfaces;
using CORE.Users.Models;
using CORE.Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = Environment.GetEnvironmentVariable("ISSUER_TOKEN"),
                         ValidAudience = Environment.GetEnvironmentVariable("AUDIENCE_TOKEN"),
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")))
                     };
                 });
}

else
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidateAudience = true,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          ValidIssuer = builder.Configuration["JWT:ISSUER_TOKEN"],
                          ValidAudience = builder.Configuration["JWT:AUDIENCE_TOKEN"],
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SECRET_KEY"]))
                      };
                  });

}

//Se agregan estas líneas:
builder.Services.AddTransient((ServiceProvider) => BridgeDBConnection<UserModel>.Create(builder.Configuration.GetConnectionString("LocalServer"), CORE.Connection.Models.DbEnum.Sql));
builder.Services.AddTransient((ServiceProvider) => BridgeDBConnection<LoginModel>.Create(builder.Configuration.GetConnectionString("CloudServer"), CORE.Connection.Models.DbEnum.Sql));

builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<ILogin, LoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
