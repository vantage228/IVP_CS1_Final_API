
using BondConsoleApp.Repository;
using CS_Console.EquityRepo;
using CS_Console.LogRepo;
using CS_Console.UserRepo;
using Serilog;

namespace IVP_CS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) 
                .Enrich.FromLogContext()   
                .CreateLogger();
            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddTransient<IEquity, EquityOperation>();
            builder.Services.AddTransient<IBond, BondOperations>();
            builder.Services.AddTransient<ILog, LogOperation>();    
            builder.Services.AddTransient<IUser, UserOperation>();    

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

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.MapControllers();

            //app.Run();

            try
            {
                Log.Information("App Started");
                app.Run();
            }
            catch(Exception ex) 
            {
                Log.Error(ex, "Unhandled exception occurred.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
