using System;
using System.Net;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pharma.DbContext;
using Pharma.DbContext.Entities;
using Pharma.Helpers;
using Pharma.Models;

namespace Pharma
{
	public class Startup
	{
		private const string SecretKey = "iNivDmHLpUA213sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
		private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<PharmaContext>()
				.AddDefaultTokenProviders();

			services.AddDbContext<PharmaContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
					b => b.MigrationsAssembly("Pharma")));
			services.AddControllersWithViews();
			services.AddSingleton<IJwtFactory, JwtFactory>();
			services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();


			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});


			var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

			// Configure JwtIssuerOptions
			services.Configure<JwtIssuerOptions>(options =>
			{
				options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
				options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
				options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
			});

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

				ValidateAudience = true,
				ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

				ValidateIssuerSigningKey = true,
				IssuerSigningKey = _signingKey,

				RequireExpirationTime = false,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(configureOptions =>
			{
				configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
				configureOptions.TokenValidationParameters = tokenValidationParameters;
				configureOptions.SaveToken = true;
			});

			// api user claim policy
			services.AddAuthorization(options =>
			{
				options.AddPolicy("ApiUser", policy =>
					policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
				options.AddPolicy("ApiDoctor", policy =>
					policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiDoctorAccess));
			});

			var builder = services.AddIdentityCore<AppUser>(o =>
			{
				// configure identity options
				o.Password.RequireDigit = false;
				o.Password.RequireLowercase = false;
				o.Password.RequireUppercase = false;
				o.Password.RequireNonAlphanumeric = false;
				o.Password.RequiredLength = 6;
			});
			builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
			builder.AddEntityFrameworkStores<PharmaContext>().AddDefaultTokenProviders();
			services.AddAutoMapper();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();


			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp";

				if (env.IsDevelopment())
				{
					spa.UseAngularCliServer(npmScript: "start");
				}
			});

			app.UseExceptionHandler(
				builder =>
				{
					builder.Run(
						async context =>
						{
							context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
							context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

							var error = context.Features.Get<IExceptionHandlerFeature>();
							if (error != null)
							{
								context.Response.AddApplicationError(error.Error.Message);
								await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
							}
						});
				});

			app.UseAuthentication();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			if (!env.IsDevelopment())
			{
				app.UseSpaStaticFiles();
			}
		}
	}
}
