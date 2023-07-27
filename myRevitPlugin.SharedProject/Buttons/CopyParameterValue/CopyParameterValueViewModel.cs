using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Autodesk.Revit.DB;
using System.Linq;

namespace myRevitPlugin.Buttons.CopyParameterValue
{
    public class CopyParameterValueViewModel : ViewModelBase
    {
        #region Properties
        public CopyParameterValueModel Model { get; set; }
        public RelayCommand<Window> Close { get; set; }
        public RelayCommand<Window> ShowCategoryParameters { get; set; }
        public RelayCommand<Window> CopyParameterValueFromParameterToParameter { get; set; }
        public List<Category> ListOfCategories { get; set; }
        public List<CategoryWrapper> ListOfCategoryWrappers { get; set; }
        public Category SelectedCategory { get; set; }
        public List<Parameter> ListOfCategoryParameters { get; set; }

        private ObservableCollection<ElementParameterWrapper> leftlistOfElementParameterWrappers;
        public ObservableCollection<ElementParameterWrapper> LeftListOfElementParameterWrappers
        {
            get { return leftlistOfElementParameterWrappers; }
            set { leftlistOfElementParameterWrappers = value; RaisePropertyChanged(() => LeftListOfElementParameterWrappers); }
        }

        private ObservableCollection<ElementParameterWrapper> rightlistOfElementParameterWrappers;
        public ObservableCollection<ElementParameterWrapper> RightListOfElementParameterWrappers
        {
            get { return rightlistOfElementParameterWrappers; }
            set { rightlistOfElementParameterWrappers = value; RaisePropertyChanged(() => RightListOfElementParameterWrappers); }
        }

        #endregion

        public CopyParameterValueViewModel(CopyParameterValueModel model)   
        {
            Model = model;
            Close = new RelayCommand<Window>(OnClose);
            ShowCategoryParameters = new RelayCommand<Window>(OnShowCategoryParameters);
            CopyParameterValueFromParameterToParameter = new RelayCommand<Window>(OnCopyParameterValueFromParameterToParameter);
            ListOfCategories = Model.GetAllPresentElementCategoriesInDocument().OrderBy(x => x.Name).ToList();
            ListOfCategoryWrappers = ListOfCategories.Select(x => new CategoryWrapper(x)).ToList();
        }

        #region Commands
        public void OnShowCategoryParameters(Window winObject)
        {
            SelectedCategory = Model.GetSelectedCategory(ListOfCategoryWrappers);
            LeftListOfElementParameterWrappers = Model.GetAllParametersOfGivenCategoryFromFirstElement(SelectedCategory);
            RightListOfElementParameterWrappers = Model.CopyElementParameterWrapperCollection(LeftListOfElementParameterWrappers);
        }

        public void OnCopyParameterValueFromParameterToParameter(Window winObject)
        {
            Model.CopyParameterValueFromParameterToParameter(LeftListOfElementParameterWrappers, RightListOfElementParameterWrappers, SelectedCategory);
        }

        private void OnClose(Window winObject)
        {
            winObject.Close();
        }
        #endregion
    }
}
