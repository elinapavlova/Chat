using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Database;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Options;
using Infrastructure.Profiles;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using Services.CheckFilesJobs;

namespace FileStorageService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure options
            services.Configure<FileStorageOptions>(Configuration.GetSection(FileStorageOptions.FileStorage));
            var fileStorageOptions = Configuration.GetSection(FileStorageOptions.FileStorage).Get<FileStorageOptions>();
            services.AddSingleton(fileStorageOptions);
            
            // Configure Services
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IFileStorageService, Services.FileStorageService>();
            services.AddScoped<ICheckFilesJob, CheckFilesJob>();
            
            // Configure HttpClient
            services.AddHttpClient();
            
            // Configure DbContext
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection));
            
            // Configure AutoMapper profiles
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Configure Api
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new QueryStringApiVersionReader();
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            
            // Configure swagger
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            // Configure Hangfire
            services.AddHangfire(configuration =>
            {
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                configuration.UseSimpleAssemblyNameTypeSerializer();
                configuration.UseRecommendedSerializerSettings();
                configuration.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection"));
            });
            services.AddHangfireServer();
            
            services.AddControllers();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(               
                    opt => 
                    {
                        foreach (var description in provider.ApiVersionDescriptions) 
                        {
                            opt.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
            }
            
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<ICheckFilesJob>("DeleteImageFromDbIfFileNotExistsJob", 
                x => 
                    x.DeleteImageFromDbIfFileNotExists(), Cron.Daily(0, 35), TimeZoneInfo.Local);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}