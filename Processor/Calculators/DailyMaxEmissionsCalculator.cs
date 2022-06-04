using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor.Models;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;
using Day = Processor.Models.OutputModel.Day;

namespace Processor.Calculators
{
    public class DailyMaxEmissionsCalculator
    {

        private readonly MaxEmissionGenerators _maxEmissionGenerators = new()
        {
            Day = new List<Day>()
        };

        private readonly List<Day> _day = new();

        public MaxEmissionGenerators CalculateMaxEmissionGenerators(GenerationReport report, ReferenceData reference)
        {

            double emission = 0;

            foreach (var coalGenerator in report.Coal.CoalGenerator)
            {
                foreach (var day in coalGenerator.Generation.Day)
                {
                    emission = day.Energy * coalGenerator.EmissionsRating * reference.Factors.EmissionsFactor.High;
                    AddNewValues(coalGenerator.Name, day.Date, emission);
                }
            }

            foreach (var gasGenerator in report.Gas.GasGenerator)
            {
                foreach (var day in gasGenerator.Generation.Day)
                {
                    emission = day.Energy * gasGenerator.EmissionsRating * reference.Factors.EmissionsFactor.Medium;
                    AddNewValues(gasGenerator.Name, day.Date, emission);

                }
            }

            var selectMany = _day.GroupBy(d => d.Date)
                .SelectMany(x => x.Where(y => Math.Abs(y.Emission - x.Max(z => z.Emission)) < 0.0000000001)).ToList();

            _maxEmissionGenerators.Day.AddRange(selectMany);

            return _maxEmissionGenerators;
        }

        private void AddNewValues(string name, string date, double value)
        {
            _day.Add(
                new Day
                {
                    Name = name,
                    Date = date,
                    Emission = value
                }
            );
        }

    }
}
