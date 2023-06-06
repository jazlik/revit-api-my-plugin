using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace myRevitPlugin.Buttons.CopyParameterValue
{
    public class CopyParameterValueViewModel : ViewModelBase
    {
        #region Properties
        public CopyParameterValueModel Model { get; set; }
        public RelayCommand<Window> Close { get; set; }
        #endregion

        public CopyParameterValueViewModel(CopyParameterValueModel model)
        {
            Model = model;
            Close = new RelayCommand<Window>(OnClose);
        }

        #region Commands
        private void OnClose(Window winObject)
        {
            IList<Category> cats = Model.GetAllDefinedCategoriesInDocument();
            var test = Model.GetParametersValuesForCategories(cats);
            Model.DisplayMethod(test);
            winObject.Close();
        }
        #endregion
    }
}
