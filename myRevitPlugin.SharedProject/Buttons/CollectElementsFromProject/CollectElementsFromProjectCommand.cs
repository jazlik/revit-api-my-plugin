using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using myRevitPlugin.Utilities;

namespace myRevitPlugin.Buttons.CollectElementsFromProject
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class CollectElementsFromProjectCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiApp = commandData.Application;
                var model = new CollectElementsFromProjectModel(uiApp);
                var viewModel = new CollectElementsFromProjectViewModel(model);
                var view = new CollectElementsFromProjectView { DataContext = viewModel };

                var unused = new WindowInteropHelper(view);
                unused.Owner = Process.GetCurrentProcess().MainWindowHandle;

                view.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }

        public static void CreateButton(RibbonPanel panel)
        {
            var assembly = Assembly.GetExecutingAssembly();
            panel.AddItem(new PushButtonData(
                MethodBase.GetCurrentMethod().DeclaringType?.Name,
                "Collect" + Environment.NewLine + "Elements",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName
                )
            {
                ToolTip = "Collect all elements from entire project.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.2-symbol.png")
            }
            );
        }
    }
}
