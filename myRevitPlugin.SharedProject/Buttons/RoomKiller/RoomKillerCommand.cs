using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using myRevitPlugin.Utilities;

namespace myRevitPlugin.Buttons.RoomKiller
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class RoomKillerCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Creating uiApp, model, viewModel and view inside the Execute method for a button.
                var uiApp = commandData.Application;
                var model = new RoomKillerModel(uiApp);
                var viewModel = new RoomKillerViewModel(model);
                var view = new RoomKillerView { DataContext = viewModel };

                // View is attached to current process, which is Revit as a parent-child relation.
                // When Revit is closed, view window is also closed.
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
            var assembly = Assembly.GetExecutingAssembly(); // Path to assembly needed for button.
            panel.AddItem(new PushButtonData(
                MethodBase.GetCurrentMethod().DeclaringType?.Name,
                "Room" + Environment.NewLine + "Killer",
                assembly.Location,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName
                )
            {
                ToolTip = "RoomKiller command.",
                LargeImage = ImageUtils.LoadImage(assembly, "_32x32.room_kill.png") // _ is added as VS is adding it to folder name.
            }
            );
        }
    }
}
