using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualizingWaterfall
{
    public class SampleData : IFixedRenderSize
    {
        public String data;

        public SampleData(String data)
        {
            this.data = data;
        }

        public double FixedRenderHeight
        {
            get
            {
                return 200;
            }

            set
            {
                
            }
        }

        public double FixedRenderWidth
        {
            get
            {
                return 100;
            }

            set
            {
                
            }
        }
    }
}
