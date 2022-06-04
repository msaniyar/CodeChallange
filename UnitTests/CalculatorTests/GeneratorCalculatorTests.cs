using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Processor.Calculators;
using Processor.Models;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;
using Day = Processor.Models.InputModel.Day;

namespace UnitTests.CalculatorTests
{
    public class GeneratorCalculatorTests
    {
        private GeneratorCalculator _calculator;

        [SetUp]
        public void TestSetup()
        {
            var referenceData = new ReferenceData
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

            _calculator = new GeneratorCalculator(referenceData);
        }

        [Test]
        public void GenerateOutputTest()
        {
            var coal = new Coal
            {
                CoalGenerator = new List<CoalGenerator>
                {
                    new()
                    {
                        Name = "Coal 1",
                        Generation = new Generation
                        {
                            Day = new List<Day>
                            {
                                new Day
                                {
                                    Date = "2017-01-01T00:00:00+00:00",
                                    Energy = 350.487,
                                    Price = 10.146
                                }
                            }
                        },
                        TotalHeatInput = 11.815,
                        ActualNetGeneration = 11.815,
                        EmissionsRating = 0.482
                    }
                }
            };

            var gas = new Gas
            {
                GasGenerator = new List<GasGenerator>
                {
                    new()
                    {
                        Name = "Gas 1",
                        Generation = new Generation
                        {
                            Day = new List<Day>
                            {
                                new Day
                                {
                                    Date = "2017-01-01T00:00:00+00:00",
                                    Energy = 259.235,
                                    Price = 15.837
                                }
                            }
                        },
                        EmissionsRating = 0.038
                    }
                }
            };

            var wind = new Wind
            {
                WindGenerator = new List<WindGenerator>
                {
                    new WindGenerator
                    {
                        Name = "Wind",
                        Generation = new Generation
                        {
                            Day = new List<Day>
                            {
                                new Day
                                {
                                    Date = "2017-01-01T00:00:00+00:00",
                                    Energy = 100.368,
                                    Price = 20.148
                                }
                            }
                        },
                        Location = "Offshore"
                    }
                }
            };


            var expectedResult = new GenerationOutput
            {
                Totals = new Totals
                {
                    Generator = new List<Generator>
                    {
                        new Generator
                        {
                            Name = "Wind",
                            Total = "4246.650374400"
                        },
                        new Generator
                        {
                            Name = "Gas 1",
                            Total = "9237.385563750"
                        },
                        new Generator
                        {
                            Name = "Coal 1",
                            Total = "8001.092479500"
                        }
                    }
                },
                MaxEmissionGenerators = new MaxEmissionGenerators
                {
                    Day = new List<Processor.Models.OutputModel.Day>
                    {
                        new Processor.Models.OutputModel.Day
                        {
                            Name = "Coal 1",
                            Date = "2017-01-01T00:00:00+00:00",
                            Emission = "253.402101000"
                        }
                    }
                },
                ActualHeatRates = new ActualHeatRates
                {
                    ActualHeatRate = new List<ActualHeatRate>
                    {
                        new ActualHeatRate
                        {
                            Name = "Coal 1",
                            HeatRate = "1"
                        }
                    }
                }
            };

            var report = new GenerationReport
            {
                Wind = wind,
                Gas = gas,
                Coal = coal
            };


            var result = _calculator.CalculateOutput(report);
            result.Should().BeEquivalentTo(expectedResult);

        }
    }
}