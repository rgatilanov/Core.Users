using CORE.Users.Configuration;
using CORE.Users.Interfaces;
using CORE.Users.Models;
using CORE.Users.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
