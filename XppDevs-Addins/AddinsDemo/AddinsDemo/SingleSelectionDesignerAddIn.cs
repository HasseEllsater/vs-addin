using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Text;
using Microsoft.Dynamics.AX.Metadata.Core;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;

namespace XppDevs.AddinsDemo
{
    /// <summary>
    /// Demonstrates how to get information about a single element selected in class designer.
    /// </summary>
    
    // This add-in is available from context menu in designer.
    [Export(typeof(IDesignerMenu))]
    // This add-in will work on classes (IClassItem) and class methods (IMethod).
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IClassItem))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IMethod))]
    public class SingleSelectionDesignerAddIn : DesignerMenuBase
    {
        private const string addinName = "SingleSelectionDemoAXAddIn";
        // Caption is shown to users in the Addins menu.
        public override string Caption => AddinResources.SingleSelectionAddInCaption;
        public override string Name => addinName;

        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinDesignerEventArgs e)
        {
            try
            {
                StringBuilder messages = new StringBuilder();

                // Get the selected element as a base type "NamedElement". Maybe you don't need to know the specific
                // type, because you just want to dump element names, for example. You still can check the type later
                // and run any type-specific logic.
                NamedElement namedElement = e.SelectedElement as NamedElement;
                if (namedElement != null)
                {
                    messages.AppendLine($"You selected {namedElement.GetType().Name} named {namedElement.Name}.");
                }

                // If you're looking for a specific type, try to cast the selected item directly to the type.
                ClassItem c = e.SelectedElement as ClassItem;
                if (c != null)
                {
                    int methodCount = c.Methods.Cast<IMethod>().Count(); // Calculating number of methods
                    messages.AppendLine($"The class has {methodCount} method(s).");
                }

                // Show messages in a dialog box.
                CoreUtility.DisplayInfo(messages.ToString());
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
    }
}
