using System;

namespace TransactionRunner.Helpers
{
    /// <summary>
    /// Attribute to decorate ViewModel classes with corresponding Views (controls)
    /// </summary>
    public class TemplateAttribute : Attribute
    {
        public TemplateAttribute(Type viewType)
        {
            ViewType = viewType;
        }

        public Type ViewType { get; }
    }
}