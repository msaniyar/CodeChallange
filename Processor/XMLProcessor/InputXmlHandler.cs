using MediatR;
using Processor.Models.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Processor.XMLProcessor
{
    public class InputXmlHandler : IRequestHandler<InputXml, OutputResult>
    {

        private GenerationReport _generationReport;


        Task<OutputResult> IRequestHandler<InputXml, OutputResult>.Handle(InputXml input, CancellationToken cancellationToken)
        {

            try
            {
                var serializer = new XmlSerializer(typeof(GenerationReport));

                using (XmlReader reader = new XmlNodeReader(input.InputDocument))
                {
                    _generationReport = (GenerationReport)serializer.Deserialize(reader);
                }

                var fileCreator = new FileCreator(input, _generationReport);
                return Task.FromResult(fileCreator.CreateOutputFile());
            }
            catch (Exception exp)
            {
                return Task.FromResult(new OutputResult
                {
                    Message = $"File cannot be created. Error Message: {exp.Message}",
                    Success = false
                });
            }

        }
    }
}
