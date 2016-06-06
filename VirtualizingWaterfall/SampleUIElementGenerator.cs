using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VirtualizingWaterfall
{
    public class SampleUIElementGenerator : BaseUIElementGenerator
    {
        public SampleUIElementGenerator(Panel parent) : base(parent)
        {
            
        }

        protected override UIElement GenerateNewElement()
        {
            SampleContainer container = new SampleContainer();
            return container;
        }

        protected override void SetElementData(UIElement element, object data)
        {
            SampleContainer container = element as SampleContainer;
            if(container == null)
            {
                return;
            }
            else
            {
                SampleData sample = data as SampleData;
                container.SetData(sample);
            }
        }
    }
}
