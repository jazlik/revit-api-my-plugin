using System.Collections.ObjectModel;
using System.Windows;
using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace myRevitPlugin.Buttons.CopyDraftingViews
{
    public class CopyDraftingViewsViewModel : ViewModelBase
    {
        #region Properties
        public CopyDraftingViewsModel Model { get; set; }
        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> ShowViewsFromRevitLinks { get; set; }
        public RelayCommand<Window> CopyViewFromSelectedRevitLink { get; set; }

        private ObservableCollection<RVTLinkWrapper> rvtLinks;
        public ObservableCollection<RVTLinkWrapper> RVTLinks
        {
            get { return rvtLinks; }
            set { rvtLinks = value; RaisePropertyChanged(() => RVTLinks); }
        }

        private ObservableCollection<ViewWrapper> viewWrapperViews;
        public ObservableCollection<ViewWrapper> ViewWrapperViews
        {
            get { return viewWrapperViews; }
            set { viewWrapperViews = value; RaisePropertyChanged(() => ViewWrapperViews); }
        }

        public Document FromDocument { get; set; }

        #endregion

        public CopyDraftingViewsViewModel(CopyDraftingViewsModel model)
        {
            Model = model;
            RVTLinks = Model.CollectRVTLinks();
            Close = new RelayCommand<Window>(OnClose);
            ShowViewsFromRevitLinks = new RelayCommand<Window>(OnShowViewsFromRevitLinks);
            CopyViewFromSelectedRevitLink = new RelayCommand<Window>(OnCopyViewFromSelectedRevitLink);
        }

        #region Commands
        private void OnClose(Window winObject)
        {
            winObject.Close();
        }

        public void OnShowViewsFromRevitLinks(Window winObject)
        {
            FromDocument = Model.GetSelectedRevitLink(RVTLinks);
            ViewWrapperViews = Model.CollectViewWrappersFromDocument(FromDocument);
        }

        public void OnCopyViewFromSelectedRevitLink(Window winObject)
        {
            Model.CopyViews(ViewWrapperViews, FromDocument);
        }
        #endregion
    }
}

// sprawdź Revit SDK Samples: https://github.com/jeremytammik/RevitSdkSamples/tree/master/SDK/Samples/DuplicateViews/CS

