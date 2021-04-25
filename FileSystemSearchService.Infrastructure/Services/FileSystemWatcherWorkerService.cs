using FileSystemSearchService.Core.Interfaces.Services;
using FileSystemSearchService.Core.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileSystemSearchService.Infrastructure.Services
{
    public class FileSystemWatcherWorkerService : BackgroundService
    {
        readonly ILogger<FileSystemWatcherWorkerService> _logger;
        FileSystemWatcher _folderWatcher;

        readonly IFileSystemEventFactory _fileSystemEventFactory;
        readonly FolderMonitoringConfigurationOptions _folderMonitoringConfigurationOptions;
        readonly IArtifactIndexingService _artifactIndexingService;

        public FileSystemWatcherWorkerService(ILogger<FileSystemWatcherWorkerService> logger,
            IFileSystemEventFactory fileSystemEventFactory,
            IOptions<FolderMonitoringConfigurationOptions> folderMonitoringConfigurationOptions,
            IArtifactIndexingService artifactIndexingService)
        {
            _logger = logger;
            _fileSystemEventFactory = fileSystemEventFactory;
            _folderMonitoringConfigurationOptions = folderMonitoringConfigurationOptions.Value;
            _artifactIndexingService = artifactIndexingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("FileSystemWatcher is starting");

            if (!Directory.Exists(_folderMonitoringConfigurationOptions.FolderToMonitor))
            {
                _logger.LogError($"The folder [{_folderMonitoringConfigurationOptions.FolderToMonitor}] does not exist.");
                return Task.CompletedTask;
            }

            //All files and all sub-folders.
            _folderWatcher = new FileSystemWatcher(_folderMonitoringConfigurationOptions.FolderToMonitor)
            {
                NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName
                    | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite
                    | NotifyFilters.Security | NotifyFilters.Size
            };

            //Add events
            _folderWatcher.Changed += _folderWatcher_Changed;
            _folderWatcher.Created += _folderWatcher_Created;
            _folderWatcher.Deleted += _folderWatcher_Deleted;
            _folderWatcher.Renamed += _folderWatcher_Renamed;
            _folderWatcher.Error += _folderWatcher_Error;

            _folderWatcher.EnableRaisingEvents = true;

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _folderWatcher.EnableRaisingEvents = false;

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _folderWatcher.Dispose();

            base.Dispose();
        }

        void _folderWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"{e.FullPath} was changed.");

            var eventToBeRaised = _fileSystemEventFactory.GenerateRelevantEvent(e);

            _artifactIndexingService.DeltaUpdate(eventToBeRaised);
        }

        void _folderWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"{e.FullPath} was created.");

            var eventToBeRaised = _fileSystemEventFactory.GenerateRelevantEvent(e);

            _artifactIndexingService.DeltaUpdate(eventToBeRaised);
        }

        void _folderWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"{e.FullPath} was deleted.");

            var eventToBeRaised = _fileSystemEventFactory.GenerateRelevantEvent(e);

            _artifactIndexingService.DeltaUpdate(eventToBeRaised);
        }

        void _folderWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            _logger.LogInformation($"{e.FullPath} was renamed.");

            var eventToBeRaised = _fileSystemEventFactory.GenerateRelevantEvent(e);

            _artifactIndexingService.DeltaUpdate(eventToBeRaised);
        }

        void _folderWatcher_Error(object sender, ErrorEventArgs e)
        {
            _logger.LogError("Some error.", e);
            //todo: What shall we do in this event?
        }
    }
}
