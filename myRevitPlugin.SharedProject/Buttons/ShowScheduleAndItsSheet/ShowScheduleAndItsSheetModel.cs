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

        public void GetSelectedScheduleOrSheet(ObservableCollection<SchedulePlacement> schedulePlacements)
        {
            foreach (SchedulePlacement schedulePlacement in schedulePlacements)
            {
                if (schedulePlacement.IsObjectSelected == true)
                {
                    var selected = Doc.GetElement(schedulePlacement.ScheduleId) as ViewSchedule;
                }
                else
                {
                    continue;
                }
            }
        }
        // Co się stanie jak zaznaczymy id schedule a co jak id sheetki?

        // Rezultat - możemy zaznaczyć w tabelce Schedule i Sheet i dać select
    }
}
