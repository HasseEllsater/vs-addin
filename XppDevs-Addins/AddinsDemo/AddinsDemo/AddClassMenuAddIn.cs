using Microsoft.Dynamics.AX.Metadata.MetaModel;
using Microsoft.Dynamics.Framework.Tools.AddIns.Common;
using Microsoft.Dynamics.Framework.Tools.Extensibility;
using Microsoft.Dynamics.Framework.Tools.MetaModel.Core;
using Microsoft.Dynamics.Framework.Tools.ProjectSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XppDevs.AddinsDemo
{
    /// <summary>
    /// Demonstrates how to add a new class to the the active project.
    /// </summary>

    // This add-in is available from Dynamics AX > Addins menu.
    [Export(typeof(IMainMenu))]
    public class AddClassMenuAddIn : MainMenuBase
    {
        private const string addinName = "AddClassDemoAXAddIn";
        // Caption is shown to users in the Addins menu.
        public override string Caption => AddinResources.AddClassAddInCaption;
        public override string Name => addinName;

        #region Callbacks
        /// <summary>
        /// Called when user clicks on the add-in menu
        /// </summary>
        /// <param name="e">The context of the VS tools and metadata</param>
        public override void OnClick(AddinEventArgs e)
        {
            try
            {
                // Find project to work with
                VSProjectNode project = LocalUtils.GetActiveProject();
                // An alternative way is calling ProjectHelper.GetProjectDetails(), but it's not consistent
                // with how projectService.AddElementToActiveProject() determines the active project.
                // ProjectHelper return the startup project, which doens't have to be the active project.

                if (project == null)
                {
                    throw new InvalidOperationException("No project selected.");
                }
                
                // Create a new class
                AxClass axClass = new AxClass()
                {
                    Name = "MyNewClass",
                    IsAbstract = true // Set a property for demonstration
                };

                // Find metamodel service needed below
                

                // Find current model
                //ModelInfo model = metaModelService.GetModel(ProjectHelper.GetProjectDetails().Model);
                ModelInfo model = project.GetProjectsModelInfo();

                // Prepare information needed for saving
                ModelSaveInfo saveInfo = new ModelSaveInfo
                {
                    Id = model.Id,
                    Layer = model.Layer
                };

                // Create class in the right model
                var metaModelProviders = ServiceLocator.GetService(typeof(IMetaModelProviders)) as IMetaModelProviders;
                var metaModelService = metaModelProviders.CurrentMetaModelService;
                metaModelService.CreateClass(axClass, saveInfo);

                // Add the class to the active project
                var projectService = ServiceLocator.GetService(typeof(IDynamicsProjectService)) as IDynamicsProjectService;
                projectService.AddElementToActiveProject(axClass);

            }
            catch (Exception ex)
            {
                CoreUtility.HandleExceptionWithErrorMessage(ex);
            }
        }
        #endregion
    }
}
