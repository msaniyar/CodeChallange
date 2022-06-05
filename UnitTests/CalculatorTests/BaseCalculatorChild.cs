using System.Collections.Generic;
using System.Linq;
using Processor.Calculators;
using Processor.Models;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;
using Day = Processor.Models.OutputModel.Day;

namespace UnitTests.CalculatorTests
{
    public class BaseCalculatorChild : BaseCalculator
    {
        public BaseCalculatorChild(ReferenceData referenceData) : base(referenceData)
        {
        }

        public override GenerationOutput CalculateOutput(GenerationReport report)
        {
            throw new System.NotImplementedException();
        }

        public new double CalculateDailyGeneration(Generation generation, GeneratorTypes type)
        {
            return base.CalculateDailyGeneration(generation, type);
        }

        public static double CalculateActualHeatRat(double totalHeatInput, double actualNetGeneration)
        {
            return CalculateActualHeatRate(totalHeatInput, actualNetGeneration);
        }

        public new List<Day> CalculateMaxEmissionDays(Generation generation, GeneratorTypes type, double emissionRating, string name)
        {
            return base.CalculateMaxEmissionDays(generation, type, emissionRating, name);

        }
    }
}