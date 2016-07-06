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
    /// Demonstrates how to get information about one or more elements selected in class designer.
    /// </summary>

    // This add-in is available from context menu in designer.
    [Export(typeof(IDesignerMenu))]
    // This add-in will work on classes (IClassItem) and class methods (IMethod).
    // Notice the "CanSelectMultiple" parameter of the second attribute.
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IClassItem))]
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IMethod), CanSelectMultiple = true)]
    public class MultiSelectionDesignerAddIn : DesignerMenuBase
    {
        private const string addinName = "MultiSelectionDemoAXAddIn";
        // Caption is shown to users in the Addins menu.
        public override string Caption => AddinResources.MultiSelectionAddInCaption;
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

                // SelectedElements is a collection of objects; you can iterate it as such
                foreach (object o in e.SelectedElements)
                {
                    // Here you would do something
                }

                // It's better to use a more specific type, such as NamedElement.
                // This add-in accepts both classes and methods and this block
                // will handle both.
                foreach (NamedElement el in e.SelectedElements.OfType<NamedElement>())
                {
                    messages.AppendLine($"You selected {el.GetType().Name} named {el.Name}.");
                }

                // You can also select only elements of a specific type - class methods in this case.
                var onlyMethods = e.SelectedElements.OfType<IMethod>();

                messages.AppendLine($"You selected {onlyMethods.Count()} method(s).");

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
