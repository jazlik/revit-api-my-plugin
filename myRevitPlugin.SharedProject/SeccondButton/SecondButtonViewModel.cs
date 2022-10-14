using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace myRevitPlugin.SeccondButton
{
    public class SecondButtonViewModel : ViewModelBase
    {
        public SecondButtonModel Model {get; set;}
        // 2A. This method is searched from view. When SecondButtonViewModel is constructed
        // OnClose method is passed into it and triggered, closing the window.
        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> Delete { get; set; }

        private ObservableCollection<SpatialElementWrapper> spatialObjects;
        // This SpatialObjects collection is binded as ItemSource for DataGrid in SecondButtonView.
        public ObservableCollection<SpatialElementWrapper> SpatialObjects
        { 
            get { return spatialObjects; }
            set { spatialObjects = value; RaisePropertyChanged(() => SpatialObjects); }
        }

        public SecondButtonViewModel(SecondButtonModel model)
        {
            Model = model;
            SpatialObjects = Model.CollectSpatialObjects();
            Close = new RelayCommand<Window>(OnClose);
            Delete = new RelayCommand<Window>(OnDelete);
        }

        private void OnClose(Window winObject)
        {
            winObject.Close();
        }

        private void OnDelete(Window winObject)
        {
            var selected = SpatialObjects.Where(x => x.IsObjectSelected == true).ToList();
            Model.Delete(selected);
            winObject.Close();
        }
    }
}
