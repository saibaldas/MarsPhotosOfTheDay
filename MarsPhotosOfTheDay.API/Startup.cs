using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsPhotosOfTheDay.Services.Errors;
using MarsPhotosOfTheDay.Services.Interfaces;
using MarsPhotosOfTheDay.Services.Services.Implementation;
using MarsPhotosOfTheDay.Services.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MarsPhotosOfTheDay.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
            services.AddControllers();
            services.AddSingleton<IHttpRequester>(new HttpRequester(new MarsPhotosOfTheDay.Services.UriBuilder("DEMO_KEY")));
            services.AddTransient<IHttpResponseParser, HttpResponseParser>()
                    .AddTransient<IErrorHandler, ErrorHandler>()
                    .AddTransient<IErrorBuilder, ErrorBuilder>();
            services.AddTransient<IMarsRoverPhotosClient, MarsRoverPhotosClient>();

            services.AddSwaggerGen();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
