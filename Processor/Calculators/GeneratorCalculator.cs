using System;
using System.Collections.Generic;
using System.Linq;
using Processor.Models;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;
using Day = Processor.Models.OutputModel.Day;


namespace Processor.Calculators
{
    public class GeneratorCalculator : BaseCalculator
    {
        public GeneratorCalculator(ReferenceData referenceData) : base(referenceData)
        {
        }

        public GenerationOutput CalculateOutput(GenerationReport report)
        {
            double dailyGenerationValue = 0;
            var totals = new Totals
            {
                Generator = new List<Generator>()
            };

            var maxEmissionGenerators = new MaxEmissionGenerators
            {
                Day = new List<Day>()
            };

            var actualHeatRates = new ActualHeatRates
            {
                ActualHeatRate = new List<ActualHeatRate>()
            };

            var days = new List<Day>();

            var generationOutput = new GenerationOutput();

            foreach (var windGenerator in report.Wind.WindGenerator)
            {
                _ = Enum.TryParse(windGenerator.Location, out GeneratorTypes type);
                dailyGenerationValue = CalculateDailyGeneration(windGenerator.Generation, type);

                totals.Generator.Add(new Generator
                {
                    Name = windGenerator.Name,
                    Total = dailyGenerationValue
                });

            }


            foreach (var coalGenerator in report.Coal.CoalGenerator)
            {
                dailyGenerationValue = CalculateDailyGeneration(coalGenerator.Generation, GeneratorTypes.Coal);
                var coalEmissionDays = CalculateMaxEmissionDays(coalGenerator.Generation, GeneratorTypes.Coal,
                    coalGenerator.EmissionsRating, coalGenerator.Name);
                var actualHeatRate = CalculateActualHeatRate(coalGenerator.TotalHeatInput, coalGenerator.ActualNetGeneration);

                totals.Generator.Add(new Generator
                {
                    Name = coalGenerator.Name,
                    Total = dailyGenerationValue
                });

                days.AddRange(coalEmissionDays);

                actualHeatRates.ActualHeatRate.Add(new ActualHeatRate()
                {
                    Name = coalGenerator.Name,
                    HeatRate = actualHeatRate
                });

            }

            foreach (var gasGenerator in report.Gas.GasGenerator)
            {

                dailyGenerationValue = CalculateDailyGeneration(gasGenerator.Generation, GeneratorTypes.Gas);

                var gasEmissionDays = CalculateMaxEmissionDays(gasGenerator.Generation, GeneratorTypes.Gas,
                    gasGenerator.EmissionsRating, gasGenerator.Name);

                days.AddRange(gasEmissionDays);

                totals.Generator.Add(new Generator
                {
                    Name = gasGenerator.Name,
                    Total = dailyGenerationValue
                });

            }

            var maxEmissionDays = days.GroupBy(d => d.Date)
                .SelectMany(x => x.Where(y => Math.Abs(y.Emission - x.Max(z => z.Emission)) < 0.0000000001)).ToList();

            maxEmissionGenerators.Day.AddRange(maxEmissionDays);

            return new GenerationOutput
            {
                Totals = totals,
                MaxEmissionGenerators = maxEmissionGenerators,
                ActualHeatRates = actualHeatRates
            };

        }
    }
}