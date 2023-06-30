
using PollyClientApp.Policies;

namespace PollyClientApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient("Test").AddPolicyHandler ( request => 
            request.Method == HttpMethod.Get? new ClientPolicy().ImmdiateHttpRetry : new ClientPolicy().ExponentialHttpRetry);


            // Add services to the container.
            builder.Services.AddSingleton<ClientPolicy>(new ClientPolicy());

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}