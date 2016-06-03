using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualizingWaterfall
{
    public interface IFixedRenderSize
    {
        double FixedRenderWidth { get; set; }
        double FixedRenderHeight { get; set; }
    }
}
