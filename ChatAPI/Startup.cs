using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Database;
using Infrastructure.Configurations;
using Infrastructure.Contracts;
using Infrastructure.Hashing;
using Infrastructure.Options;
using Infrastructure.Profiles;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;
using Services.Contracts;
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
            services.AddHttpClient<IFileStorageService, FileStorageService>("FileStorage", client =>
            {
                client.BaseAddress = new Uri(Configuration["BaseAddress:FileStorage:Address"]);
            });
            
            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.Token));
            services.Configure<PagingOptions>(Configuration.GetSection(PagingOptions.Paging));

            var tokenOptions = Configuration.GetSection(TokenOptions.Token).Get<TokenOptions>();
            var pagingOptions = Configuration.GetSection(PagingOptions.Paging).Get<PagingOptions>();
            var signingConfigurations = new SigningConfiguration (tokenOptions.Secret);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IUserRoomRepository, UserRoomRepository>();
            services.AddScoped<IUserChatRepository, UserChatRepository>();
            
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton(signingConfigurations);
            services.AddSingleton(pagingOptions);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IUserChatService, UserChatService>();
            services.AddScoped<IUserRoomService, UserRoomService>();

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
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = tokenOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = tokenOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingConfigurations.SecurityKey
                    };
                });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddSwaggerGen(options =>
            {
                // Configure Bearer Authentication
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "Bearer {authToken}",
                    Description = "JSON Web Token to access resources. Example: Bearer {token}",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                
                // Configure xml documentation file
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
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
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}