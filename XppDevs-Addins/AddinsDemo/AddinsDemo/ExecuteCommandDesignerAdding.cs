using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Tables;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace XppDevs.AddinsDemo
{
    /// <summary>
    /// Demonstrates how to call predefined commands.
    /// </summary>

    [Export(typeof(IDesignerMenu))]
    // This add-in will work on tables.
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(ITable))]
    class ExecuteCommandDesignerAdding : DesignerMenuBase
    {
        private const string addinName = "ExecuteCommandDemoAXAddIn";
        // Caption is shown to users in the Addins menu.
        public override string Caption => AddinResources.ExecuteCommandAddInCaption;
        public override string Name => addinName;

        public override void OnClick(AddinDesignerEventArgs e)
        {
            try
            {
                ITable t = e.SelectedElement as ITable;

                if (t != null)
                {
                    if (t.CanExecute(CommandType.TableBrowse)) // Check if the command can be executed
                    {
                        t.Execute(CommandType.TableBrowse);
                    }

                    // For more commands, look at avaiable values of CommandType enum
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
    }
}