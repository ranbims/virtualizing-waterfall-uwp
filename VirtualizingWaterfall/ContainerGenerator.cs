using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VirtualizingWaterfall
{
    public class ContainerGenerator<TCacheType>
    {
        private Panel _parent;
        private Stack<TCacheType> _elements;

        private int _generatedCount;

        public int GeneratedCount { get { return _generatedCount; } }
        public int CacheCount { get { return _elements.Count; } }

        public ContainerGenerator(Panel panel)
        {
            _generatedCount = 0;
            _parent = panel;
            _elements = new Stack<TCacheType>();
        }

        public TCacheType GenerateContainer()
        {
            if(_elements.Count != 0)
            {
                return _elements.Pop();
            }
            else
            {
                TCacheType newElement = Activator.CreateInstance<TCacheType>();
                if(newElement is UIElement)
                {
                    _parent.Children.Add(newElement as UIElement);
                    _generatedCount++;
                }
                return newElement;
            }
        }

        public void CollectCache(TCacheType cache)
        {
            _elements.Push(cache);
        }
    }
}
