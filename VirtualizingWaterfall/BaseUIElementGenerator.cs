using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace VirtualizingWaterfall
{
    public abstract class BaseUIElementGenerator
    {
        protected Panel _parent;

        protected Stack<UIElement> _elements;

        protected int _generatedCount;

        public int GeneratedCount { get { return _generatedCount; } }
        public int CacheCount { get { return _elements.Count; } }

        public BaseUIElementGenerator(Panel parent)
        {
            _parent = parent;
            _generatedCount = 0;
            _parent = parent;
            _elements = new Stack<UIElement>();
        }

        protected abstract UIElement GenerateNewElement();

        protected abstract void SetElementData(UIElement element, object data);

        public UIElement GenerateUIElement(object data)
        {
            UIElement newElement;
            if (_elements.Count != 0)
            {
                newElement = _elements.Pop();
            }
            else
            {
                newElement = GenerateNewElement();
  //              _parent.Children.Add(newElement);
                _generatedCount++;
            }
            SetElementData(newElement, data);
            return newElement;
        }

        public void CollectCache(UIElement cache)
        {
            if(cache != null)
                _elements.Push(cache);
        }
    }
}
