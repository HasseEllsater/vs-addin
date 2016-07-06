using Microsoft.Dynamics.AX.Metadata.Extensions;
using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Automation.Classes;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace XppDevs.AddinsDemo
{
    /// <summary>
    /// Demonstrates how to methods to classes.
    /// </summary>

    // This add-in is available from context menu in designer.
    [Export(typeof(IDesignerMenu))]
    // This add-in will work on classes (IClassItem).
    [DesignerMenuExportMetadata(AutomationNodeType = typeof(IClassItem))]
    class AddMethodDesignerAddIn : DesignerMenuBase
    {
        private const string addinName = "AddMethodDemoAXAddIn";
        // Caption is shown to users in the Addins menu.
        public override string Caption => AddinResources.AddMethodAddInCaption;
        public override string Name => addinName;

        public override void OnClick(AddinDesignerEventArgs e)
        {
            try
            {
                ClassItem c = e.SelectedElement as ClassItem;

                if (c != null)
                {
                    AddMethodToFile(c);
                    //CallAddMethodCommand(c);
                }
            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }

        /// <summary>
        /// Add a new method (with a hard-coded name and empty body) to the selected class.
        /// It add the 
        /// </summary>
        /// <param name="c"></param>
        /// <remarks>
        /// The method is added to the underlying file, but not to the designer.
        /// Visual Studio will detect the change and asks the user if the file should be reloaded.
        /// Another approach is needed to update the designer directly.
        /// </remarks>
        void AddMethodToFile(ClassItem c)
        {
            AxClass axClass = (AxClass)c.GetMetadataType();

            // Add the method to the class
            axClass.AddMethod(BuildMethod());

            // Prepare objects needed for saving
            var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
            var metaModelService = metaModelProviders.CurrentMetaModelService;
            // Getting the model will likely have to be more sophisticated, such as getting the model of the project and checking
            // if the object has the same model.
            // But this shold do for demonstration.
            ModelInfo model = DesignMetaModelService.Instance.CurrentMetadataProvider.Classes.GetModelInfo(axClass.Name).FirstOrDefault<ModelInfo>();

            // Update the file
            metaModelService.UpdateClass(axClass, new ModelSaveInfo(model));
        }

        // Calling "Add" command creates a new method, but that's all. You can't set method
        // name, the body nort anything else.
        void CallAddMethodCommand(ClassItem c)
        {
            if (c.CanExecuteAdd(Method.DomainClassId))
            {
                c.ExecuteAdd(Method.DomainClassId);
            }
        }

        // Create a new method with empty body
        private AxMethod BuildMethod()
        {
            AxMethod axMethod = new AxMethod()
            {
                Name = "myNewMethod",
                ReturnType = new AxMethodReturnType()
            };
            
            axMethod.Source = axMethod.GetDefaultMethodSource(4);

            return axMethod;
        }
    }
}
