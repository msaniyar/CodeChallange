using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CodeChallange
{
    public class XmlFileWatcherService : BackgroundService
    {
        private readonly ILogger<XmlFileWatcherService> _logger;
        private FileSystemWatcher _watcher;


        private readonly IConfiguration _config;
        private readonly string _watcherPath;

        public XmlFileWatcherService(ILogger<XmlFileWatcherService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _watcherPath = _config.GetSection("Input:Location").Value;

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, cancellationToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _watcher = new FileSystemWatcher
            {
                Path = _watcherPath,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.Attributes
                | NotifyFilters.CreationTime
                | NotifyFilters.FileName,

                Filter = "*.xml"
            };


            _watcher.Created += new FileSystemEventHandler(OnCreated);

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Service");
            _watcher.EnableRaisingEvents = false;
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing Service");
            _watcher.Dispose();
            base.Dispose();
        }


        public void OnCreated(object source, FileSystemEventArgs args)
        {
            Console.WriteLine(args.Name + " is " + args.FullPath);
        }
    }
}
