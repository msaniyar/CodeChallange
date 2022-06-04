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

        public OutputResult CreateOutputFile()
        {
            var output = new GenerationOutput();
            var outputResult = new OutputResult();
            var dailyGenerationValueCalculator = new DailyGenerationValueCalculator();
            var maxEmissionGenerators = new DailyMaxEmissionsCalculator();
            var actualHeatRates = new ActualHeatRateCalculator();
            try
            {

                output.Totals = dailyGenerationValueCalculator.CalculateTotalValue(_generationReport, _input.Reference);
                output.MaxEmissionGenerators =
                    maxEmissionGenerators.CalculateMaxEmissionGenerators(_generationReport, _input.Reference);
                output.ActualHeatRates =
                    actualHeatRates.CalculateActualHeatRates(_generationReport);
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
