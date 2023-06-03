using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using myRevitPlugin.Utilities;

namespace myRevitPlugin.Buttons.CopyDraftingViews
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class CopyDraftingViewsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var uiApp = commandData.Application;
                var model = new CopyDraftingViewsModel(uiApp);
                var viewModel = new CopyDraftingViewsViewModel(model);
                var view = new CopyDraftingViewsView { DataContext = viewModel };

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
                "Copy" + Environment.NewLine + "Drafting Views",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName
                )
            {
                ToolTip = "Collects drafting views from linked models and enables copying them to host model.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.2-symbol.png")
            }
            );
        }
    }
}
