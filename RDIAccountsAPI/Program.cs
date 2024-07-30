using BusinessLogicLayer.Logging;
using DataAccessLayer.Models;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using RDIAccountsAPI;

var builder = WebApplication.CreateBuilder(args)
    ;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories();
builder.Services.AddServices();
   builder.Services.AddDbContext<EasyAccountDbContext>(options =>
{
	options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
	options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Host.ConfigureLogging((context, logging) =>
{
    logging.AddRoundCodeFileLogger(options =>
    {
        context.Configuration.GetSection("RoundTheCodeFile").GetSection("Options").Bind(options);
    });

});



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

 