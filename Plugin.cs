using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Plugin;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public class Plugin : IPlugin
    {
        public IPluginHost Host { get; set; }

        public bool IsAutoRun => false;

        public string Description => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(),
            typeof(AssemblyTitleAttribute)
        )).Title;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(fieldCount: 3);

        public string Name => ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(),
            typeof(AssemblyProductAttribute)
        )).Product;

        public void AutoRun()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
