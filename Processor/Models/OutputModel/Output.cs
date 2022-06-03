using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Processor.Models.OutputModel
{
    [XmlRoot(ElementName = "Generator")]
    public class Generator
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Total")]
        public string Total { get; set; }
    }

    [XmlRoot(ElementName = "Totals")]
    public class Totals
    {
        [XmlElement(ElementName = "Generator")]
        public List<Generator> Generator { get; set; }
    }

    [XmlRoot(ElementName = "Day")]
    public class Day
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Date")]
        public string Date { get; set; }
        [XmlElement(ElementName = "Emission")]
        public string Emission { get; set; }
    }

    [XmlRoot(ElementName = "MaxEmissionGenerators")]
    public class MaxEmissionGenerators
    {
        [XmlElement(ElementName = "Day")]
        public List<Day> Day { get; set; }
    }

    [XmlRoot(ElementName = "ActualHeatRate")]
    public class ActualHeatRate
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "HeatRate")]
        public string HeatRate { get; set; }
    }

    [XmlRoot(ElementName = "ActualHeatRates")]
    public class ActualHeatRates
    {
        [XmlElement(ElementName = "ActualHeatRate")]
        public ActualHeatRate ActualHeatRate { get; set; }
    }

    [XmlRoot(ElementName = "GenerationOutput")]
    public class GenerationOutput
    {
        [XmlElement(ElementName = "Totals")]
        public Totals Totals { get; set; }
        [XmlElement(ElementName = "MaxEmissionGenerators")]
        public MaxEmissionGenerators MaxEmissionGenerators { get; set; }
        [XmlElement(ElementName = "ActualHeatRates")]
        public ActualHeatRates ActualHeatRates { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }
}
