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


        public ObservableCollection<SchedulePlacement> CollectSchedulePlacements()
        {
            var schedulePlacements = new FilteredElementCollector(Doc)
                    .OfClass(typeof(ScheduleSheetInstance))
                    .WhereElementIsNotElementType()
                    .Cast<ScheduleSheetInstance>()
                    .Select(x => new SchedulePlacement(x));
            return new ObservableCollection<SchedulePlacement>(schedulePlacements);
        }

        //public void ShowScheduleAndSheetMethod()
        //{
        //    var schedulePlacements = new FilteredElementCollector(Doc)
        //            .OfClass(typeof(ScheduleSheetInstance))
        //            .WhereElementIsNotElementType()
        //            .Cast<ScheduleSheetInstance>();

        //    List<ElementId> viewScheduleIds = new List<ElementId>();
        //    foreach (ScheduleSheetInstance schedulePlacement in schedulePlacements)
        //    {
        //        viewScheduleIds.Add(schedulePlacement.Id);
        //    }

        //    List<string> viewScheduleNames = new List<string>();
        //    foreach (ScheduleSheetInstance schedulePlacement in schedulePlacements)
        //    {
        //        viewScheduleNames.Add(schedulePlacement.Name);
        //    }

        //    List<ElementId> viewScheduleOwnerIds = new List<ElementId>();
        //    foreach (ScheduleSheetInstance schedulePlacement in schedulePlacements)
        //    {
        //        viewScheduleOwnerIds.Add(schedulePlacement.OwnerViewId);
        //    }

        //    List<string> viewScheduleOwnerNames = new List<string>();
        //    foreach (ElementId viewScheduleOwnerId in viewScheduleOwnerIds)
        //    {
        //        Element scheduleOwner = Doc.GetElement(viewScheduleOwnerId);
        //        viewScheduleOwnerNames.Add(scheduleOwner.Name);
        //    }


        //    var scheduleNameAndSheetIdPair = viewScheduleNames.Zip(viewScheduleOwnerIds, (x, y) => new Tuple<string, object>(x, y))
        //        .ToList();

        //    String msg = String.Empty;
        //    foreach (var pair in scheduleNameAndSheetIdPair)
        //    {
        //        msg = msg + pair.Item1 + " " + pair.Item2 + "\n";
        //    }

        //    TaskDialog.Show("Results", msg);
        //}
    }
}
