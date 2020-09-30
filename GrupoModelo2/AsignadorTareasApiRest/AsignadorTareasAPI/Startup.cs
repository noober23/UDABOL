using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Negocio;

namespace AsignadorTareasAPI
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
            //RUTAS
            string rutaDeAlmacenamientoEnTexto = "BaseDeDatosTxt";
            string cadenaDeConexionSqlServer = "Server = localhost\\MSSQLSERVER01; Database = AsignadorTareas; Trusted_Connection = True;";
            //string cadenaDeConexionSqlServer = "Server = localhost; Database = AsignadorTareas; Trusted_Connection = True;";
            //CONEXION A BASE DE DATOS
            services.AddTransient<IBaseDeDatos>(options => new BaseDeDatosSqlServer(cadenaDeConexionSqlServer));
            //services.AddTransient<IBaseDeDatos>(options => new BaseDeDatosTxt(rutaDeAlmacenamientoEnTexto));

            //DECLARACION DE MANEJADORES (SERVICIOS) PARA REALIZAR INYECCION DE DEPENDENCIAS EN LOS CONTROLADORES
            services.AddTransient<IManejadorTareas, ManejadorTareas>();
            services.AddTransient<IManejadorRoles, ManejadorRoles>();
            services.AddTransient<IManejadorEstados, ManejadorEstados>();
            services.AddTransient<IManejadorUsuarios, ManejadorUsuarios>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
