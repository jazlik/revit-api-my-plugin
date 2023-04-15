using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using myRevitPlugin.Utilities;

namespace myRevitPlugin.Buttons.ShowScheduleAndItsSheet
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ShowScheduleAndItsSheetCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                var model = new ShowScheduleAndItsSheetModel(uiApp);
                //var viewModel = new ShowScheduleAndItsSheetViewModel(model);
                //var view = new ShowScheduleAndItsSheetView { DataContext = viewModel };

                //var unused = new WindowInteropHelper(view);
                //unused.Owner = Process.GetCurrentProcess().MainWindowHandle;

                //view.ShowDialog();

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
                "TEMPLATE" + Environment.NewLine + "BUTTON",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName
                )
            {
                ToolTip = "TEMPLATE FOR THE BUTTONS",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.2-symbol.png")
            }
            );
        }
    }
}
