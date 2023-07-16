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
        /// Gets all categories from the document that can have Project Parameters.
        /// </summary>
        /// <returns>List of Category objects from the document.</returns>
        public List<Category> GetAllCategoriesInDocument()
        {
            Categories categories = Doc.Settings.Categories;
            List <Category> listOfCategories = new List<Category>();
            foreach (Category category in categories)
            {
                // Check if the category is valid and can be used in the model
                if (category != null && category.AllowsBoundParameters)
                {
                    listOfCategories.Add(category);
                }
            }
            return listOfCategories;
        }

        public FilteredElementCollector GetAllElementsOfGivenCategory(Category category)
        {
            FilteredElementCollector elements = new FilteredElementCollector(Doc)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .OfCategory((BuiltInCategory)category.Id.IntegerValue);

            return elements;
        }

        public Category GetSelectedCategory(List<CategoryWrapper> listOfCategoryWrappers)
        {
            CategoryWrapper selectedCategory = listOfCategoryWrappers.Where(x => x.IsObjectSelected == true).FirstOrDefault();
            Category category = Category.GetCategory(Doc, selectedCategory.Id);
            return category;
        }

        public List<Parameter> GetAllParametersOfAGivenElement(Element element)
        {
            // var ele = element.Parameters;
            return (List<Parameter>)element.GetOrderedParameters();
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

        ///// <summary>
        ///// Gets all elements from list of category objects.
        ///// </summary>
        ///// <returns>NOTHING.</returns>
        //public FilteredElementCollector GetAllElementsFromCategories(IList<Category> cats)
        //{
        //    IList<ElementFilter> filter = new List<ElementFilter>(cats.Count);

        //    foreach (Category cat in cats)
        //    {
        //        // Get BuiltInCategory from Category.Id integer value
        //        BuiltInCategory enumCategory = (BuiltInCategory)cat.Id.IntegerValue;

        //        // Add BuiltInCategory filter to list of filters
        //        filter.Add(new ElementCategoryFilter(enumCategory));
        //    }

        //    LogicalOrFilter categoryFilter = new LogicalOrFilter(filter);

        //    FilteredElementCollector elements = new FilteredElementCollector(Doc)
        //        .WhereElementIsNotElementType()
        //        .WhereElementIsViewIndependent()
        //        .WherePasses(categoryFilter);

        //    return elements;
        //}

        /// <summary>
        /// Gets all parameters from an element.
        /// </summary>
        /// <returns>Values of element's parameters.</returns>

        //public Dictionary<string, Dictionary<string, List<string>>> GetParametersValuesForCategories(IList<Category> cats)
        //{
        //    // Set up the return value dictionary
        //    Dictionary<string,
        //        Dictionary<string,
        //            List<string>>>
        //                mapCatsToUIdToParamValues
        //                    = new Dictionary<string,
        //                    Dictionary<string,
        //                        List<string>>>();


        //    //string AddSpaceBeforeUppercase(string input)
        //    //{
        //    //    string result = "";

        //    //    for (int i = 0; i < input.Length; i++)
        //    //    {
        //    //        // Check if the current character is uppercase
        //    //        if (char.IsUpper(input[i]))
        //    //        {
        //    //            // Add a space before the uppercase character
        //    //            result += " ";
        //    //        }

        //    //        // Add the current character to the result string
        //    //        result += input[i];
        //    //    }
        //    //    result = result.Substring(1);
        //    //    return result;
        //    //}

        //    // One top level dictionary per category
        //    foreach (Category cat in cats)
        //    {
        //        mapCatsToUIdToParamValues.Add(cat.Name, new Dictionary<string, List<string>>());
        //    }

        //    // Collect all required element
        //    var elements = GetAllElementsFromCategories(cats);
        //    foreach(Element ele in elements)
        //    {
        //        Category cat = ele.Category;
        //        if(null == cat)
        //        {
        //            Debug.Print("element {0} {1} has null category", ele.Id, ele.Name);
        //            continue;
        //        }

        //        List<string> paramsValues = GetParametersValuesFromElement(ele);

        //        //BuiltInCategory bic = (BuiltInCategory)ele.Category.Id.IntegerValue;

        //        //string catKeyTest = bic.ToString().Substring(4);
        //        //string catKey = AddSpaceBeforeUppercase(catKeyTest);

        //        string catKey = ele.Category.Name.ToString();
        //        string uId = ele.UniqueId;

        //        mapCatsToUIdToParamValues[catKey].Add(uId, paramsValues);
        //    }

        //    return mapCatsToUIdToParamValues;
        //}

        public void DisplayMethod(Dictionary<string,
                Dictionary<string,
                    List<string>>> mapCatsToUIdToParamValues)
        {
            List<string> keys = new List<string>(mapCatsToUIdToParamValues.Keys);
            keys.Sort();

            foreach (string key in keys)
            {
                Dictionary<string, List<string>> els = mapCatsToUIdToParamValues[key];

                int n = els.Count;

                Debug.Print("{0} ({1} elements):", key, n);

                if (0 < n)
                {
                    List<string> uIds = new List<string>(els.Keys);
                    string uId = uIds[0];

                    List<string> paramsValues = els[uId];
                    paramsValues.Sort();

                    n = paramsValues.Count;

                    Debug.Print("   first element {0} has {1} parameters:", uId, n);

                    paramsValues.ForEach(pv => Debug.Print("   " + pv));
                }
            }
        }

        // JAK WRÓCISZ DO TEGO KURSU TO ZOBACZ CO TU JEST NAPISANE. NA TEN MOMENT ZROBIĆ METODĘ, KTÓRA ZACIĄGA CI PARAMETRY I WYPLYWA ICH DANE DO CONSOLI - ODPAL REVITA 2021 NA HOST MODELU I ZOBACZ CO TO JEST!


        // Show these parameters in two separate lists FROM on left and TO on right
        // Select 1 parameter from left column and 1 paramter from right column
        // Click Copy data
        // Window pops up saying, are you sure you want to copy data from Parameter A to Parameter B? Click Yes or No
        // Parameter data is copied
    }
}
