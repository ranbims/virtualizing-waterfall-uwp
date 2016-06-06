using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace VirtualizingWaterfall
{
    public class SampleData : IFixedRenderSize
    {
        public String data;
        public Color backgroundColor;

        public static Random rand = new Random();

        public SampleData(String data)
        {
            this.data = data;
            backgroundColor = new Color();
            backgroundColor.A = (byte)rand.Next(256);
            backgroundColor.R = (byte)rand.Next(256);
            backgroundColor.G = (byte)rand.Next(256);
            backgroundColor.B = (byte)rand.Next(256);
        }

        private double _fixedRenderHeight = 0d;

        public double FixedRenderHeight
        {
            get
            {
                if(_fixedRenderHeight == 0d)
                {
                    _fixedRenderHeight = 100 + (rand.NextDouble() * 300 );
                }
                return _fixedRenderHeight;
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
