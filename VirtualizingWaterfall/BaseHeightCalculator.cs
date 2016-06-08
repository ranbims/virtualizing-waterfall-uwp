using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualizingWaterfall
{
    public abstract class BaseHeightCalculator
    {
        public abstract double GetHeight(object data, double width);
    }
}
