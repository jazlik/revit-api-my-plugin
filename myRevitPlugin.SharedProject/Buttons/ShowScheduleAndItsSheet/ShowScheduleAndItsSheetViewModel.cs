using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace myRevitPlugin.Buttons.ShowScheduleAndItsSheet
{
    public class ShowScheduleAndItsSheetViewModel : ViewModelBase
    {
        #region Properties
        public ShowScheduleAndItsSheetModel Model { get; set; }
        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> SelectSelectedElement { get; set; }

        private ObservableCollection<SchedulePlacement> schedulePlacementElements;
        public ObservableCollection<SchedulePlacement> SchedulePlacementElements
        {
            get { return schedulePlacementElements; }
            set { schedulePlacementElements = value; RaisePropertyChanged(() => SchedulePlacementElements); }
        }
        #endregion

        public ShowScheduleAndItsSheetViewModel(ShowScheduleAndItsSheetModel model)
        {
            Model = model;
            Close = new RelayCommand<Window>(OnClose);
            SchedulePlacementElements = Model.CollectSchedulePlacements();
            SelectSelectedElement = new RelayCommand<Window>(OnSelectSelectedElement);
        }

        #region Commands
        private void OnClose(Window winObject)
        {
            winObject.Close();
        }

        public void OnSelectSelectedElement(Window winObject)
        {
            Model.GetSelectedScheduleOrSheet(SchedulePlacementElements);
        }
        #endregion
    }
}
