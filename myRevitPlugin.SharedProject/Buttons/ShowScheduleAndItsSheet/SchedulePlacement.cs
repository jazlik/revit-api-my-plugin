using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRevitPlugin.Buttons.ShowScheduleAndItsSheet
{
    public class SchedulePlacement
    {
        #region Properties
        private Document ElementDoc { get; set; }
        public ElementId ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public ElementId ScheduleOwnerId { get; set; }
        public string ScheduleOwnerName { get; set; }
        #endregion

        public SchedulePlacement(ScheduleSheetInstance schedulePlacement)
        {
            ElementDoc = schedulePlacement.Document;
            ScheduleId = schedulePlacement.Id;
            ScheduleName = schedulePlacement.Name;
            ScheduleOwnerId = schedulePlacement.OwnerViewId;
            ScheduleOwnerName = ElementDoc.GetElement(ScheduleOwnerId).Name;
        }
    }
}
