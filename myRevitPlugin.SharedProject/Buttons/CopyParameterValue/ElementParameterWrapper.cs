using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace myRevitPlugin.Buttons.CopyParameterValue
{
    public class ElementParameterWrapper
    {
        public string Name { get; set; }
        public ElementId Id { get; set; }
        public Guid GUID { get; set; }
        private bool isObjectSelected;
        public bool IsObjectSelected
        {
            get { return isObjectSelected; }
            set { isObjectSelected = value; }
        }

        public ElementParameterWrapper(Parameter parameter)
        {
            Name = parameter.Definition.Name;
            Id = parameter.Id;
            if (parameter.IsShared)
            {
                GUID = parameter.GUID;
            }
        }
    }
}
