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


        Task<OutputResult> IRequestHandler<InputXml, OutputResult>.Handle(InputXml request, CancellationToken cancellationToken)
        {

            var serializer = new XmlSerializer(typeof(GenerationReport));
            
            using (XmlReader reader = new XmlNodeReader(request.InputDocument))
            {
                _generationReport = (GenerationReport)serializer.Deserialize(reader);
            }

            return Task<OutputResult>.FromResult(new OutputResult());

        }
    }
}
