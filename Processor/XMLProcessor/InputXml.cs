using MediatR;
using Processor.Base;
using Processor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Processor.XMLProcessor
{
    public class InputXml : DocumentBase, IRequest<OutputResult>
    {
        public XmlDocument InputDocument { get; set; }

    }
}
