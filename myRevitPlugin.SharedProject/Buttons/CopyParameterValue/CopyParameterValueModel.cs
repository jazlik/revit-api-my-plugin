using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;

namespace myRevitPlugin.Buttons.CopyParameterValue
{
    public class CopyParameterValueModel
    {
        #region Properties
        public UIApplication UiApp { get; }
        public Document Doc { get; }

        public CopyParameterValueModel(UIApplication uiApp)
        {
            UiApp = uiApp;
            Doc = uiApp.ActiveUIDocument.Document;
        }
        #endregion

        /// <summary>
        /// Gets all categories from the document that are not null, can have project parameters and have at least 1 element in the model.
        /// </summary>
        /// <returns>List of Category objects from the document.</returns>
        public List<Category> GetAllPresentElementCategoriesInDocument()
        {
            Categories categories = Doc.Settings.Categories;
            List <Category> listOfCategories = new List<Category>();
            foreach (Category category in categories)
            {
                if (category != null && category.AllowsBoundParameters)
                {
                    FilteredElementCollector categoryElements = new FilteredElementCollector(Doc)
                    .OfCategory((BuiltInCategory)category.Id.IntegerValue)
                    .WhereElementIsNotElementType();

                    if (categoryElements.Count() > 0)
                    {
                        listOfCategories.Add(category);
                    }

                    else
                    {
                        continue;
                    }
                }

                else
                {
                    continue;
                }
            }

            return listOfCategories;
        }

        /// <summary>
        /// Gets all elements of input category object.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>FilteredElementCollector object.</returns>
        public FilteredElementCollector GetAllElementsOfGivenCategory(Category category)
        {
            FilteredElementCollector elements = new FilteredElementCollector(Doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .OfCategory((BuiltInCategory)category.Id.IntegerValue);

            return elements;
        }

        /// <summary>
        /// Get Category object, using a list of CategoryWrapper objects, that has property IsObjectSelected set to true.
        /// </summary>
        /// <param name="listOfCategoryWrappers"></param>
        /// <returns>Category object.</returns>
        public Category GetSelectedCategory(List<CategoryWrapper> listOfCategoryWrappers)
        {
            CategoryWrapper selectedCategory = listOfCategoryWrappers.Where(x => x.IsObjectSelected == true).FirstOrDefault();
            Category category = Category.GetCategory(Doc, selectedCategory.Id);
            
            return category;
        }

        /// <summary>
        /// Get all parameters of a given category and cast them to ObservableCollection<ElementParameterWrapper> object.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public ObservableCollection<ElementParameterWrapper> GetAllParametersOfGivenCategoryFromFirstElement(Category category)
        {
            ElementFilter elementCategoryFilter = new ElementCategoryFilter((BuiltInCategory)category.Id.IntegerValue);
            Element element = new FilteredElementCollector(Doc).WherePasses(elementCategoryFilter).ToElements().First();
            List<ElementParameterWrapper> elementParameterWrappers = element.GetOrderedParameters().Select(x => new ElementParameterWrapper(x)).ToList();

            return new ObservableCollection<ElementParameterWrapper>(elementParameterWrappers);
        }

        public List<string> GetParametersValuesFromElement(Element element)
        {
            IList<Parameter> ps = element.GetOrderedParameters();

            List<string> psValues = new List<string>(ps.Count);

            foreach (Parameter p in ps)
            {
                psValues.Add(p.AsValueString());
            }

            return psValues;
        }

        // Trzeba zrobić na odwrót: zebrać wszystkie elementy w dokumencie

        // Select 1 parameter from left column and 1 paramter from right column
        // Click Copy data
        // Window pops up saying, are you sure you want to copy data from Parameter A to Parameter B? Click Yes or No
        // Parameter data is copied
    }
}
