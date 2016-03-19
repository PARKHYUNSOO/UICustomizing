using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHS.ExplorerCommands
{
    public class ExplorerCommands : IAddin
    {
        public ExplorerCommands();

        public string Description { get; }
        public string Name { get; }

        public static IEnumerable<ITool> CreateButtons(string uicFile, ITools menuTool, string xName);
        public void Start(ServiceManager serviceManager);
        public void Stop();
    }
}
