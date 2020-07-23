using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AwsRekognitionProject.Api.Domain;
using Microsoft.Extensions.Configuration;
using AwsRekognitionProject.Api.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;

namespace AwsRekognitionProject.Api
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
            
            // Habilita a API a exibir arquivos em diretórios pelo navegador
            services.AddDirectoryBrowser();

            // Injeção de dependência das classes e interfaces
            services.AddTransient<IServiceUtils, ServiceUtils>();
            services.AddTransient<IServiceDetectFaces, ServiceDetectFaces>();
            services.AddTransient<IServiceCompareFaces, ServiceCompareFaces>();

            // Singleton da classe que utilizamos para criar a URL
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configura o diretório Images para fornecer arquivos estáticos
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/images"
            });

            // Configura o diretório Images para exibir as imagens pelo navegador
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/images"
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
