using Microsoft.EntityFrameworkCore;
using Blog.DAL.Entities;
using Blog.BLL.DTO.AccountDto;
using Blog.BLL.Constants;
using Blog.API.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace Blog
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
            JwtSettingsDto jwtSettings = new JwtSettingsDto()
            {
                AccessTokenLifeTime = new TimeSpan(1, 0, 0, 0),
                RefreshTokenLifeTime = new TimeSpan(30, 0, 0, 0),
                Secret = "jmD99IzdOPoOfpZCaXTm2RBA0pw4j6aN"
            };

            services.AddSingleton(jwtSettings);
           
            services.AddControllers();
            services.AddHttpContextAccessor();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>
               (o => o.UseSqlServer(connection));

            services.RegisterReposAndServices();
            services.RegisterMap();

            services.AddControllers().AddNewtonsoftJson();
           
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Id = "Bearer", //The name of the previously defined security scheme.
                            Type = ReferenceType.SecurityScheme
                        }
                    },new List<string>()
                }
                });
            }).AddSwaggerGenNewtonsoftSupport();

            services.AddIdentity<User, IdentityRole<Guid>>
            (options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); 

            TokenValidationParameters tokenValidationParametres = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParametres);

            services.AddMvc(options => { options.EnableEndpointRouting = false; });
               //.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParametres;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            loggerFactory.AddFile(Configuration["LogPath"]);

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateUserRoles(serviceProvider).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole<Guid>> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            bool roleAdminCheck = await roleManager.RoleExistsAsync(Roles.admin);
            bool roleUserCheck = await roleManager.RoleExistsAsync(Roles.user);

            if (!roleAdminCheck)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.admin));
            }
            if (!roleUserCheck)
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(Roles.user));
            }
        }

    }
}