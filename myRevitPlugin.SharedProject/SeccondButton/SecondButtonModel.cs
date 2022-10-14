using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace myRevitPlugin.SeccondButton
{
    public class SecondButtonModel
    {
        // UiAPp object is rarely used. Usually when messing with UI stuff like switching the view or close current view.
        public UIApplication UiApp { get;  }
        // Document object is used more often, i.e. while deleting a room.
        public Document Doc { get; }
        public SecondButtonModel(UIApplication uiApp)
        {
            // Getting the current uiApplication.
            UiApp = uiApp;
            // Getting the current document from the uiApplication.
            Doc = uiApp.ActiveUIDocument.Document;
        }

        public ObservableCollection<SpatialElementWrapper> CollectSpatialObjects()
        {
            var spatialObjects = new FilteredElementCollector(Doc)
                .OfClass(typeof(SpatialElement))
                .WhereElementIsNotElementType()
                .Cast<SpatialElement>()
                .Where(x => x is Room)
                .Select(x => new SpatialElementWrapper(x));

            return new ObservableCollection<SpatialElementWrapper>(spatialObjects);
        }

        // When deleting something in Revit, element ID is needed to do so.
        public void Delete(List<SpatialElementWrapper> selected)
        {
            var ids = selected.Select(x => x.Id).ToList();
            // To "deal" with Revit, it is needed to open a transaction, as Revit could be treated as a data base.
            using (var trans = new Transaction(Doc, "Name of the Transaction - Delete Rooms"))
            {
                trans.Start();
                Doc.Delete(ids);
                trans.Commit();
            }
        }
    }
}
