using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aveva.ApplicationFramework.Presentation;

namespace PHS.CustomizingExplorer
{
    class ExplorerContextMenu
    {
        public string Key { get; private set; }
        public bool IsFirstGroup { get; private set; }
        
        public ExplorerContextMenu(ITool tool)
        {
            Key = tool.Key;
            IsFirstGroup = tool.IsFirstInGroup;
            
        }
    }
}
