using System;
using System.Linq;
using Autodesk.Revit.UI;
using myRevitPlugin.FirstButton;
using myRevitPlugin.Buttons.RoomKiller;
using myRevitPlugin.Buttons.CopyDraftingViews;
using myRevitPlugin.Buttons.ShowScheduleAndItsSheet;
using myRevitPlugin.Buttons.CopyParameterValue;

namespace myRevitPlugin
{
    public class AppCommand : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // Put in  - try catch - in case Tab with similar name already existed.
                application.CreateRibbonTab("Jakub's Plugin");
            }
            catch (Exception e)
            {
                // Ignored.
            }

            var ribbonPanel = application.GetRibbonPanels("Jakub's Plugin")
                .FirstOrDefault(x => x.Name == "Panel 1") ??
                application.CreateRibbonPanel("Jakub's Plugin", "Panel 1");

            // Creating buttons.
            FirstButtonCommand.CreateButton(ribbonPanel);
            RoomKillerCommand.CreateButton(ribbonPanel);
            CopyDraftingViewsCommand.CreateButton(ribbonPanel);
            ShowScheduleAndItsSheetCommand.CreateButton(ribbonPanel);
            CopyParameterValueCommand.CreateButton(ribbonPanel);

            return Result.Succeeded;
        }
    }
}