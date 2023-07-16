using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRevitPlugin.Buttons.CopyParameterValue
{
    public class CategoryWrapper
    {
        public string Name { get; set; }
        public ElementId Id { get; set; }
        private bool isObjectSelected;
        public bool IsObjectSelected
        {
            get { return isObjectSelected; }
            set { isObjectSelected = value; }
        }

        public CategoryWrapper(Category category)
        { 
            Name = category.Name;
            Id = category.Id;
        }
    }
}
