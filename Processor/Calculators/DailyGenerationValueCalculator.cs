using Processor.Models;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processor.Calculators
{
    public class DailyGenerationValueCalculator
    {
        private readonly Totals _totals = new()
        {
            Generator = new List<Generator>()
        };


        public Totals CalculateTotalValue(GenerationReport report, ReferenceData reference)
        {
            double valueFactor = 0;
            double dailyGenerationValue = 0;

            foreach (var windGenerator in report.Wind.WindGenerator)
            {

                valueFactor = windGenerator.Location switch
                {
                    "Offshore" => reference.Factors.ValueFactor.Low,
                    "Onshore" => reference.Factors.ValueFactor.High,
                    _ => valueFactor
                };

                dailyGenerationValue += windGenerator.Generation.Day.Sum(day => day.Energy * day.Price * valueFactor);
                AddNewValues(windGenerator.Name, dailyGenerationValue);
                dailyGenerationValue = 0;
            }


            foreach (var gasGenerator in report.Gas.GasGenerator)
            {
                dailyGenerationValue += gasGenerator.Generation.Day.Sum(day => day.Energy * day.Price * reference.Factors.ValueFactor.Medium);
                AddNewValues(gasGenerator.Name, dailyGenerationValue);
                dailyGenerationValue = 0;
            }


            foreach (var coalGenerator in report.Coal.CoalGenerator)
            {
                dailyGenerationValue += coalGenerator.Generation.Day.Sum(day => day.Energy * day.Price * reference.Factors.ValueFactor.Medium);
                AddNewValues(coalGenerator.Name, dailyGenerationValue);
                dailyGenerationValue = 0;
            }

            return _totals;
        }

        private void AddNewValues(string name, double value)
        {
            _totals.Generator.Add(new Generator
            {
                Name = name,
                Total = value
            });
        }
    }
}
