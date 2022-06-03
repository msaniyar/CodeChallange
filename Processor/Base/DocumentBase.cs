using Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Processor.Base
{
    public abstract class DocumentBase
    {
        public string DocumentName { get; set; }
        public string OutputLocation { get; set; }
        public ReferenceData Reference { get; set; }

    }

}
