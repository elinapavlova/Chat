using AutoMapper;
using Database;
using Infrastructure.Configurations;
using Infrastructure.Contracts;
using Infrastructure.Hashing;
using Infrastructure.Options;
using Infrastructure.Profiles;
using Infrastructure.Repository;
using Infrastructure.SwaggerConfiguration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Services;
using Services.Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;
using TokenOptions = Infrastructure.Options.TokenOptions;

namespace ChatAPI
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
            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.Token));
            services.Configure<PagingOptions>(Configuration.GetSection(PagingOptions.Paging));
            
            var tokenOptions = Configuration.GetSection(TokenOptions.Token).Get<TokenOptions>();
            var pagingOptions = Configuration.GetSection(PagingOptions.Paging).Get<PagingOptions>();
            var signingConfigurations = new SigningConfiguration (tokenOptions.Secret);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton(signingConfigurations);
            services.AddSingleton(pagingOptions);
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            /*
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("chatdb");
            });
            */
            
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connection,  
                x => x.MigrationsAssembly("Database")));
            
            services.AddControllers();
            
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AppProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader("v");
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/api/login");
                    options.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/api/logout");
                    options.Cookie.HttpOnly = true;
                });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    opt => {
                        foreach (var description in provider.ApiVersionDescriptions) {
                            opt.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}