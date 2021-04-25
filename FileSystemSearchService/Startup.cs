using FileSystemSearchService.Core.Enums;
using FileSystemSearchService.Core.Interfaces.Services;
using FileSystemSearchService.Core.Options;
using FileSystemSearchService.Infrastructure.Repositories;
using FileSystemSearchService.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using System;
using System.IO.Abstractions;

namespace FileSystemSearchService
{
    public class Startup
    {
        readonly IConfiguration _configuration;
        readonly FolderMonitoringConfigurationOptions _folderMonitoringConfigurationOptions = new FolderMonitoringConfigurationOptions();
        readonly ElasticSearchConfigurationOptions _elasticSearchConfigurationOptions = new ElasticSearchConfigurationOptions();
        public IConfigurationRoot Config { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;

            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHostedService<FileSystemWatcherWorkerService>();

            ConfigureElasticSearch(services);

            ConfigureDependancyInjectedServices(services);

            ConfigureIOptions(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IIndexCreationService indexCreationService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //todo: WebRequest check that ElasticSearch is running.

            indexCreationService.CreateAllIndexes();
        }

        void ConfigureElasticSearch(IServiceCollection services)
        {
            var elasticSearchConnString = _configuration.GetConnectionString(ConnectionStringNames.ElasticSearchConnectionString.ToString());

            var settings = new ConnectionSettings(new Uri(elasticSearchConnString))
                .DefaultIndex(DocumentIndexNames.artifacts.ToString())
                .ThrowExceptions(true)
                .PrettyJson(true)
                .DisableDirectStreaming(); //Debug information visible.  Maybe hide behind ENVIRONMENT?

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }

        void ConfigureDependancyInjectedServices(IServiceCollection services)
        {
            services.AddTransient<IFileSystem, FileSystem>();

            services.AddTransient<IFilesAndFoldersService, FilesAndFoldersService>();
            services.AddTransient<IFileSystemEventFactory, FileSystemEventFactory>();
            services.AddTransient<IArtifactIndexingService, ArtifactIndexingService>();

            services.AddSingleton<ArtifactRepository>();

            services.AddTransient<IIndexCreationService, IndexCreationService>();            
        }

        void ConfigureIOptions(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<ElasticSearchConfigurationOptions>(x => Config.GetSection("ElasticSearchConfiguration").Bind(x));
            services.Configure<FolderMonitoringConfigurationOptions>(x => Config.GetSection("FolderMonitoringConfiguration").Bind(x));

            Config.GetSection("FolderMonitoringConfiguration").Bind(_folderMonitoringConfigurationOptions);
            Config.GetSection("ElasticSearchConfiguration").Bind(_elasticSearchConfigurationOptions);
        }
    }
}
