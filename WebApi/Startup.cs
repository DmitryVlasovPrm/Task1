using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

namespace WebApi
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
			// ������������� �������� ������
			services.AddDbContext<ServerContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("ServerDatabase")));
			// ����������� �����������
			services.AddControllers();

			// ��������� Swagger
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });

				// Bearer token authentication
				OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
				{
					Name = "Bearer",
					BearerFormat = "JWT",
					Scheme = "bearer",
					Description = "Specify the authorization token.",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http
				};
				c.AddSecurityDefinition("jwt_auth", securityDefinition);

				// Make sure swagger UI requires a Bearer token specified
				OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
				{
					Reference = new OpenApiReference()
					{
						Id = "jwt_auth",
						Type = ReferenceType.SecurityScheme
					}
				};
				OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
				{
					{ securityScheme, new string[] { } }
				};
				c.AddSecurityRequirement(securityRequirements);
			});

			// ���������� ���������������� JWT-�������
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						// ��������� ��������
						ValidateIssuer = true,
						ValidIssuer = AuthOptions.ISSUER,

						// ��������� �����������
						ValidateAudience = true,
						ValidAudience = AuthOptions.AUDIENCE,

						// ��������� ������� �������������
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero,

						// ��������� ����� ������������
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
					};
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server");
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				// ������������� �� �����������
				endpoints.MapControllers();
			});
		}
	}
}
