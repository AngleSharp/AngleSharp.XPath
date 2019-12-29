namespace AngleSharp
{
    using AngleSharp.Css;
    using AngleSharp.XPath;
    using System.Linq;

    /// <summary>
    /// Additional extensions for integrating XPath.
    /// </summary>
    public static class XPathConfigurationExtensions
    {
        /// <summary>
        /// Adds XPath to standard queries.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <returns>The new configuration.</returns>
        public static IConfiguration WithXPath(this IConfiguration configuration)
        {
            var selectorFactory = configuration.Services.OfType<DefaultAttributeSelectorFactory>().FirstOrDefault();

            if (selectorFactory != null)
            {
                selectorFactory.Unregister(">");
                selectorFactory.Register(">", (name, value, prefix, mode) =>
                {
                    //if ("xpath".Equals(name, System.StringComparison.OrdinalIgnoreCase))
                    //{
                    //}
                    if ("xpathns".Equals(name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return new XPathNSAttrSelector(value);
                    }
                    else
                    {
                        return new XPathAttrSelector(value);
                    }
                });
            }

            return configuration;
        }
    }
}
