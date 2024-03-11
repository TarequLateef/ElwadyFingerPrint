

using HrCodeFirstDB;
using IunitWork;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>options.CustomSchemaIds(type => type.ToString()));

//Add Db Context
builder.Services.AddDbContext<Hr_Db_CFContext>(opt =>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("Hr_Db"),
            b => b.MigrationsAssembly(typeof(Hr_Db_CFContext).Assembly.FullName)));

//Maping
builder.Services.AddScoped<IUnitofWork, UnitofWork>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("TarequPolicy", builder =>
    {
        builder.AllowAnyOrigin()  //WithOrigins("https://localhost:7152") // Allow any origin dynamically
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.Configure<FormOptions>(o =>
{
    o.MemoryBufferThreshold=int.MaxValue;
    o.ValueLengthLimit=int.MaxValue;
    o.MultipartBodyLengthLimit=int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("TarequPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
