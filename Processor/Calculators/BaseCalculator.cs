using System;
using System.Collections.Generic;
using System.Linq;
using Processor.Models;
using Processor.Models.InputModel;
using Day = Processor.Models.OutputModel.Day;

namespace Processor.Calculators
{
    public class BaseCalculator
    {
        private readonly ReferenceData _referenceData;

        protected BaseCalculator(ReferenceData referenceData)
        {
            _referenceData = referenceData;
        }

        protected double CalculateDailyGeneration(Generation generation, GeneratorTypes type)
        {
            double dailyGenerationValue = 0;
            dailyGenerationValue += generation.Day.Sum(day => day.Energy * day.Price * GetValueFactor(type));
            return dailyGenerationValue;
        }

        protected static double CalculateActualHeatRate(double totalHeatInput, double actualNetGeneration) =>
            actualNetGeneration != 0
                ? totalHeatInput / actualNetGeneration
                : 0;

        protected List<Day> CalculateMaxEmissionDays(Generation generation, GeneratorTypes type, double emissionRating, string name)
        {
            var day = new List<Day>();

            day.AddRange(from generatorDay in generation.Day
                         let emission = generatorDay.Energy * emissionRating * GetEmissionFactor(type)
                         select new Day { Name = name, Date = generatorDay.Date, Emission = emission });

            return day;

        }


        private double GetValueFactor(GeneratorTypes type)
        {
            return type switch
            {
                GeneratorTypes.Offshore => _referenceData.Factors.ValueFactor.Low,
                GeneratorTypes.Onshore => _referenceData.Factors.ValueFactor.High,
                GeneratorTypes.Coal => _referenceData.Factors.ValueFactor.Medium,
                GeneratorTypes.Gas => _referenceData.Factors.ValueFactor.Medium,
                _ => 0
            };
        }

        private double GetEmissionFactor(GeneratorTypes type)
        {
            return type switch
            {
                GeneratorTypes.Coal => _referenceData.Factors.EmissionsFactor.High,
                GeneratorTypes.Gas => _referenceData.Factors.EmissionsFactor.Medium,
                _ => 0
            };
        }
    }
}