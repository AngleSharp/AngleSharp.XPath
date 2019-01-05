namespace AngleSharp.XPath
{
    using AngleSharp.Css;
    using AngleSharp.Css.Dom;
    using AngleSharp.Dom;
    using System;
    using System.Collections.Generic;

    sealed class XPathAttrSelector : ISelector
    {
        private readonly String _value;
        private IElement _scope;
        private List<INode> _result;

        public XPathAttrSelector(String value) => _value = value;

        public Priority Specificity => Priority.OneClass;

        public String Text => $"[xpath>'${_value}']";

        public void Accept(ISelectorVisitor visitor)
        {
        }

        public Boolean Match(IElement element, IElement scope)
        {
            if (_scope != scope)
            {
                _scope = scope;
                _result = scope.SelectNodes(_value);
            }

            return _result.Contains(element);
        }
    }
}
