using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace myRevitPlugin.Buttons.ShowScheduleAndItsSheet
{
    public class ShowScheduleAndItsSheetModel
    {
        #region Properties
        public UIApplication UiApp { get; }
        public Document Doc { get; }
        //public ShowScheduleAndItsSheetViewModel ViewModel { get; }

        public ShowScheduleAndItsSheetModel(UIApplication uiApp)
        {
            UiApp = uiApp;
            Doc = uiApp.ActiveUIDocument.Document;
        }

        #endregion

        public void ShowScheduleAndSheetMethod()
        {
            var schedulePlacements = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ScheduleSheetInstance))
                    .WhereElementIsNotElementType()
                    .Cast<ScheduleSheetInstance>();

            List<String> viewScheduleNames = new List<String>();
            foreach (ScheduleSheetInstance schedulePlacement in schedulePlacements)
            {
                viewScheduleNames.Add(schedulePlacement.Name);
            }

            List<ElementId> viewScheduleOwnerIds = new List<ElementId>();
            foreach (ScheduleSheetInstance schedulePlacement in schedulePlacements)
            {
                viewScheduleOwnerIds.Add(schedulePlacement.OwnerViewId);
            }
            var scheduleNameAndSheetIdPair = viewScheduleNames.Zip(viewScheduleOwnerIds, (x, y) => new Tuple<string, object>(x, y))
                .ToList();

            String msg = String.Empty;
            foreach (var pair in scheduleNameAndSheetIdPair)
            {
                msg = msg + pair.Item1 + " " + pair.Item2 + "\n";
            }

            TaskDialog.Show("Results", msg);
        }
    }
}
