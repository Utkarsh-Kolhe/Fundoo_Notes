using Microsoft.EntityFrameworkCore;
using Repository_Layer.ContextClass;
using Repository_Layer.InterfaceRL;
using Repository_Layer.ServiceRL;
using Bussiness_Layer.InterfaceBL;
using Bussiness_Layer.ServiceBL;
using Repository_Layer.Hashing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<FundooContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddTransient<IUserInterfaceBL, UserServiceBL>();
builder.Services.AddTransient<IUserInterfaceRL, UserServiceRL>();
builder.Services.AddTransient<HashingPassword>();


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
