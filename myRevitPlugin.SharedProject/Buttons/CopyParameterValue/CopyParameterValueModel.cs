using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
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
            Element element = new FilteredElementCollector(Doc)
                .WherePasses(elementCategoryFilter)
                .WhereElementIsNotElementType()
                .ToElements()
                .First();
            List<ElementParameterWrapper> elementParameterWrappers = element.GetOrderedParameters().Select(x => new ElementParameterWrapper(x)).ToList();

            return new ObservableCollection<ElementParameterWrapper>(elementParameterWrappers);
        }

        /// <summary>
        /// Copy an observable collection and all its objects by creating a new list and creating a new object for each object in the observable collection.
        /// </summary>
        /// <param name="observableCollection"></param>
        /// <returns></returns>
        public ObservableCollection<ElementParameterWrapper> CopyElementParameterWrapperCollection(ObservableCollection<ElementParameterWrapper> observableCollection)
        {
            List<ElementParameterWrapper> clonedObservableCollection = observableCollection.Select(x => new ElementParameterWrapper(x)
            {
                Name = x.Name,
                BuiltInParameterId = x.BuiltInParameterId,
                GUID = x.GUID
            }).ToList();

            return new ObservableCollection<ElementParameterWrapper>(clonedObservableCollection);
        }

        public void CopyParameterValueFromParameterToParameter(IList<ElementParameterWrapper> elementParameterWrapperList1, IList<ElementParameterWrapper> elementParameterWrapperList2, Category selectedCategory)
        {
            List<Element> elementsList = GetAllElementsOfGivenCategory(selectedCategory).ToElements().ToList();
            ElementParameterWrapper fromParameter = (elementParameterWrapperList1.Where(x => x.IsObjectSelected)).ElementAt(0);
            ElementParameterWrapper toParameter = (elementParameterWrapperList2.Where(x => x.IsObjectSelected)).ElementAt(0);
            StorageType fromParameterStorageType = fromParameter.StorageType;
            StorageType toParameterStorageType = toParameter.StorageType;

            if (fromParameterStorageType == toParameterStorageType)
            {
                using (Transaction t = new Transaction(Doc, "Set Parameters"))
                {
                    t.Start();

                    foreach (Element element in elementsList)
                    {
                        Parameter parameterToSet = default;
                        Parameter parameterToSetFrom = default;
                        var value = (dynamic)null;

                        // Both paramters are shared paramters
                        if (fromParameter.BuiltInParameterId == BuiltInParameter.INVALID && toParameter.BuiltInParameterId == BuiltInParameter.INVALID)
                        {
                            parameterToSet = element.get_Parameter(toParameter.GUID);
                            parameterToSetFrom = element.get_Parameter(fromParameter.GUID);
                        }
                        // Both paramters are BuiltInParameters
                        else if (fromParameter.BuiltInParameterId != BuiltInParameter.INVALID && toParameter.BuiltInParameterId != BuiltInParameter.INVALID)
                        {
                            parameterToSet = element.get_Parameter(toParameter.BuiltInParameterId);
                            parameterToSetFrom = element.get_Parameter(fromParameter.BuiltInParameterId);
                        }
                        // fromParamter is shared while toParamter is BuiltInParameter
                        else if (fromParameter.BuiltInParameterId == BuiltInParameter.INVALID && toParameter.BuiltInParameterId != BuiltInParameter.INVALID)
                        {
                            parameterToSet = element.get_Parameter(toParameter.BuiltInParameterId);
                            parameterToSetFrom = element.get_Parameter(fromParameter.GUID);
                        }
                        // fromParamter is BuiltInParamter while toParameter is shared
                        else if (fromParameter.BuiltInParameterId != BuiltInParameter.INVALID && toParameter.BuiltInParameterId == BuiltInParameter.INVALID)
                        {
                            parameterToSet = element.get_Parameter(toParameter.GUID);
                            parameterToSetFrom = element.get_Parameter(fromParameter.BuiltInParameterId);
                        }

                        switch (fromParameterStorageType)
                        {
                            case StorageType.String:
                                value = parameterToSetFrom.AsString();
                                break;

                            case StorageType.Integer:
                                value = parameterToSetFrom.AsInteger();
                                break;

                            case StorageType.ElementId:
                                value = parameterToSetFrom.AsElementId();
                                break;

                            case StorageType.Double:
                                value = parameterToSetFrom.AsDouble();
                                break;

                            case StorageType.None:
                                value = "_";
                                break;
                            default:
                                value = "_";
                                break;
                        }

                        if (value != null && value != "")
                        {
                            parameterToSet.Set(value);
                        }

                        else
                        {
                            continue;
                        }
                    }
                    t.Commit();
                }
            }

            else
            {
                // Creates a Revit task dialog to communicate error to the user, if StorageType of both parameters are not the same
                TaskDialog popingErrorDialog = new TaskDialog("CopyParameterValue Error");
                popingErrorDialog.MainInstruction = "Error occured while copying parameters value!";
                string message = String.Format(
                    "You cannot copy value from parameter \"{0}\" to parameter \"{1}\"." +
                    "\n" +
                    "Parameter \"{0}\" holds value that is a {2} type, while parameter \"{1}\" holds value that is a {3} type." +
                    "\n" +
                    "You can only copy value between parameters with the same value types.", fromParameter.Name, toParameter.Name, fromParameterStorageType, toParameterStorageType);
                popingErrorDialog.MainContent = message;
                popingErrorDialog.Show();
            }
        }
    }
}
