using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Processor.Models;
using Processor.XMLProcessor;

namespace FileWatcherService
{
    public class XmlFileWatcherService : BackgroundService
    {
        private readonly ILogger<XmlFileWatcherService> _logger;
        private FileSystemWatcher _watcher;


        private readonly IConfiguration _config;
        private readonly string _watcherPath;
        private readonly string _outputPath;
        private readonly string _referenceFile;
        private readonly IMediator _mediator;
        private static ReferenceData _referenceData;

        public XmlFileWatcherService(ILogger<XmlFileWatcherService> logger,
                                     IConfiguration config,
                                     IMediator mediator)
        {
            _logger = logger;
            _config = config;
            _mediator = mediator;
            _watcherPath = _config.GetSection("Input:Location").Value;
            _outputPath = _config.GetSection("Output:Location").Value;
            var referenceName = _config.GetSection("Reference:Name").Value;
            var referencePath = _config.GetSection("Reference:Location").Value;

            _referenceFile = Directory.Exists(referencePath)
                           ? (Path.Combine(referencePath, referenceName))
                           : throw new SystemException("Reference Directory is invalid.");

            if(!File.Exists(_referenceFile))
            {
                throw new SystemException("Reference File is invalid.");
            }

        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000, cancellationToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            CheckAndCreatePaths(_watcherPath, _outputPath);
            LoadReferenceData();

            _watcher = new FileSystemWatcher
            {
                Path = _watcherPath,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.Attributes
                | NotifyFilters.CreationTime
                | NotifyFilters.FileName,

                Filter = _config.GetSection("FileExtension").Value
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

            var doc = new XmlDocument
            {
                PreserveWhitespace = true
            };

            try
            {
                doc.Load(args.FullPath);
            }
            catch (Exception)
            {
                throw;
            }

            var document = new InputXml
            {
                Reference = _referenceData,
                InputDocument = doc,
                DocumentName = args.Name,
                OutputLocation = _outputPath
            };

            var result = _mediator.Send(document).GetAwaiter().GetResult();
            _logger.LogInformation(result.Message);
        }


        private static void CheckAndCreatePaths(params string[] path)
        {
            foreach (var pathItem in path)
            {
                if (Directory.Exists(pathItem)) continue;
                Directory.CreateDirectory(pathItem);
            }

        }

        private void LoadReferenceData()
        {
            var reader = new XmlSerializer(typeof(ReferenceData));
            var file = new StreamReader(_referenceFile);
            _referenceData = (ReferenceData)reader.Deserialize(file);
            file.Close();
        }

    }
}
