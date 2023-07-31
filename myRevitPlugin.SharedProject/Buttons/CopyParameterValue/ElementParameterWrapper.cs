using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;

namespace myRevitPlugin.Buttons.CopyParameterValue
{
    public class ElementParameterWrapper
    {
        public string Name { get; set; }
        public BuiltInParameter BuiltInParameterId { get; set; }
        public Guid GUID { get; set; }
        public StorageType StorageType { get; set; }
        private bool isObjectSelected;
        public bool IsObjectSelected
        {
            get { return isObjectSelected; }
            set { isObjectSelected = value; RaisePropertyChanged(nameof(IsObjectSelected)); }
        }

        public ElementParameterWrapper(Parameter parameter)
        {
            Name = parameter.Definition.Name;
            BuiltInParameterId = (parameter.Definition as InternalDefinition).BuiltInParameter;

            if (parameter.IsShared)
            {
                GUID = parameter.GUID;
            }

            StorageType = parameter.StorageType;
        }
        public ElementParameterWrapper(ElementParameterWrapper parameter)
        {
            Name = parameter.Name;
            BuiltInParameterId = parameter.BuiltInParameterId;

            if (parameter.GUID != null)
            {
                GUID = parameter.GUID;
            }

            StorageType = parameter.StorageType;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
