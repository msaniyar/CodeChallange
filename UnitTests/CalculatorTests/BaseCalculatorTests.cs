using System.Collections.Generic;
using NUnit.Framework;
using Processor.Calculators;
using Processor.Models;
using Processor.Models.InputModel;

namespace UnitTests.CalculatorTests
{
    public class BaseCalculatorTests

    {

        private Generation _generation;

        private ReferenceData _referenceData;

        private BaseCalculatorChild _baseCalculator;

        [SetUp]
        public void TestSetup()
        {
            _generation = new Generation
            {
                Day = new List<Day>
                {
                    new Day
                    {
                        Date = "2022-01-01T00:00:00+00:00",
                        Energy = 100.1,
                        Price = 10.1
                    },
                    new Day
                    {
                        Date = "2022-01-02T00:00:00+00:00",
                        Energy = 110.1,
                        Price = 15.1
                    },
                    new Day
                    {
                        Date = "2022-01-02T00:00:00+00:00",
                        Energy = 120.1,
                        Price = 20.1
                    }

                }
            };

            _referenceData = new ReferenceData
            {
                Factors = new Factors
                {
                    EmissionsFactor = new EmissionsFactor
                    {
                        High = 1.5,
                        Medium = 1.25,
                        Low = 1.1
                    },
                    ValueFactor = new ValueFactor
                    {
                        High = 2.5,
                        Medium = 2.25,
                        Low = 2.1
                    }
                }
            };

            _baseCalculator = new BaseCalculatorChild(_referenceData);
        }

        [Test]
        public void CoalDailyGenerationCalculationTest()
        {
            var result = _baseCalculator.CalculateDailyGeneration(_generation, GeneratorTypes.Coal);
            Assert.That(result, Is.EqualTo(11446.942500000001));
        }

        [Test]
        public void GasDailyGenerationCalculationTest()
        {
            var result = _baseCalculator.CalculateDailyGeneration(_generation, GeneratorTypes.Gas);
            Assert.That(result, Is.EqualTo(11446.942500000001));
        }

        [Test]
        public void OffShoreDailyGenerationCalculationTest()
        {
            var result = _baseCalculator.CalculateDailyGeneration(_generation, GeneratorTypes.Offshore);
            Assert.That(result, Is.EqualTo(10683.813));
        }

        [Test]
        public void OnShoreDailyGenerationCalculationTest()
        {

            var result = _baseCalculator.CalculateDailyGeneration(_generation, GeneratorTypes.Onshore);
            Assert.That(result, Is.EqualTo(12718.825000000001));
        }

        [Test]
        public void ActualHeatRateCalculationTest()
        {
            const double totalHeatInput = 1250.123321;
            const double actualNetGeneration = 1000.3213213;
            var result = BaseCalculatorChild.CalculateActualHeatRat(1250.123321, 1000.3213213);
            Assert.That(result, Is.EqualTo(totalHeatInput/actualNetGeneration));
        }

        [Test]
        public void MaxEmissionCalculationTest()
        {
            const double emissionRating = 0.582;
            const string name = "Test Generator";

            var emissions = new List<double>
            {
                87.387300000,
                96.117300000,
                104.847300000
            };


            var result = _baseCalculator.CalculateMaxEmissionDays(
                _generation,
                GeneratorTypes.Coal,
                emissionRating,
                name);


            for (var i = 0; i < result.Count - 1; i++)
            {
                Assert.That(result[i].Emission, Is.EqualTo(emissions[i].ToString("F9")));
            }

        }


    }
}