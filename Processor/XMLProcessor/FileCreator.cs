using Processor.Calculators;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Processor.XMLProcessor
{
    public class FileCreator
    {
        private readonly InputXml _input;
        private readonly GenerationReport _generationReport;

        public FileCreator(InputXml input, GenerationReport generationReport)
        {
            _input = input;
            _generationReport = generationReport;
        }

        /// <summary>
        /// Create desired output xml file
        /// </summary>
        /// <returns></returns>
        public OutputResult CreateOutputFile()
        {
            var outputResult = new OutputResult();

            try
            {
                var calculator = new GeneratorCalculator(_input.Reference);
                var output = calculator.CalculateOutput(_generationReport);

                var writer = new XmlSerializer(typeof(GenerationOutput));
                var path = Path.Combine(_input.OutputLocation, $"{Path.GetFileNameWithoutExtension(_input.DocumentName)}-result.xml");
                var file = File.Create(path);
                writer.Serialize(file, output);
                file.Close();

                outputResult.Message = $"Output file created. FileName: {path}";
                outputResult.Success = true;
            }
            catch (Exception exp)
            {
                outputResult.Message = $"File cannot be created. Error Message: {exp.Message}";
                outputResult.Success = false;
            }

            return outputResult;

        }
    }
}
