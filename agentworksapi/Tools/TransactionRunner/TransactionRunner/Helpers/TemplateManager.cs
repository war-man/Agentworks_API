using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace TransactionRunner.Helpers
{
    /// <summary>
    /// Class to register View-ViewModel associations basing on Template attribute
    /// </summary>
    public static class TemplateManager
    {
        public static void RegisterTemplates()
        {
            var viewModelTypes = Assembly.GetAssembly(typeof(TemplateManager))
                .GetTypes()
                .Where(x => x.GetCustomAttribute<TemplateAttribute>() != null);

            foreach (Type viewModelType in viewModelTypes)
            {
                TemplateAttribute attribute = viewModelType.GetCustomAttribute<TemplateAttribute>();
                RegisterDataTemplate(viewModelType, attribute.ViewType);
            }
        }

        private static void RegisterDataTemplate(Type viewModelType, Type viewType)
        {
            var template = CreateTemplate(viewModelType, viewType);
            var key = template.DataTemplateKey;
            Application.Current.Resources.Add(key, template);
        }

        private static DataTemplate CreateTemplate(Type viewModelType, Type viewType)
        {
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");

            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            return template;
        }
    }
}