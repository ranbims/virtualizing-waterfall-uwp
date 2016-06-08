using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualizingWaterfall
{
    public class SampleHeightCalculator : BaseHeightCalculator
    {
        public override double GetHeight(object data, double width)
        {
            SampleData sData = data as SampleData;
            if (sData == null)
                return 0;
            else
                return sData.FixedRenderHeight; 
        }
    }
}
