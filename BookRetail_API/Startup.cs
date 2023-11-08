using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EasyNetQ;
using BookRetail_API.Data;
using BookRetail_API.GraphQL.Schemas;
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;

namespace BookRetail_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IBookRetailStorage, BookCsvFileStorage>();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add RabbitMQ support
            var bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("AutoRabbitMQ"));
            services.AddSingleton<IBus>(bus);

            // Add GraphQL
            services.AddScoped<ISchema, BookSchema>();
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
            }).AddSystemTextJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();

            app.UseGraphQL<ISchema>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}