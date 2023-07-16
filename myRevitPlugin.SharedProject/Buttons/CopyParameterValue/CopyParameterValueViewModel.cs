﻿using System.Collections.Generic;
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
        public List<Category> ListOfCategories { get; set; }
        public List<CategoryWrapper> ListOfCategoryWrappers { get; set; }

        // Lista parameterów przypisanych do danej kategorii:
        public List<Parameter> ListOfCategoryParameters { get; set; }


        #endregion

        public CopyParameterValueViewModel(CopyParameterValueModel model)   
        {
            Model = model;
            Close = new RelayCommand<Window>(OnClose);
            ShowCategoryParameters = new RelayCommand<Window>(OnShowCategoryParameters);
            ListOfCategories = Model.GetAllCategoriesInDocument().OrderBy(x => x.Name).ToList();
            ListOfCategoryWrappers = ListOfCategories.Select(x => new CategoryWrapper(x)).ToList();
        }

        #region Commands
        public void OnShowCategoryParameters(Window winObject)
        {
            var cosTam = Model.GetSelectedCategory(ListOfCategoryWrappers);
            // ListOfCategoryParameters
        }
        private void OnClose(Window winObject)
        {
            winObject.Close();
        }
        #endregion
    }
}