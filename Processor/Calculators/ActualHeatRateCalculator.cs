using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processor.Models;
using Processor.Models.InputModel;
using Processor.Models.OutputModel;

namespace Processor.Calculators
{
    public class ActualHeatRateCalculator
    {
        private readonly ActualHeatRates _actualHeatRates = new()
        {
            ActualHeatRate = new List<ActualHeatRate>()
        };


        public ActualHeatRates CalculateActualHeatRates(GenerationReport report)
        {
            foreach (var coalGenerator in report.Coal.CoalGenerator)
            {
                var actualHeatRate = coalGenerator.ActualNetGeneration != 0
                    ? coalGenerator.TotalHeatInput / coalGenerator.ActualNetGeneration
                    : 0;
                AddNewValues(coalGenerator.Name, actualHeatRate);
            }

            return _actualHeatRates;
        }

        private void AddNewValues(string name, double value)
        {
            _actualHeatRates.ActualHeatRate.Add(new ActualHeatRate()
            {
                Name = name,
                HeatRate = value
            });
        }
    }
}
