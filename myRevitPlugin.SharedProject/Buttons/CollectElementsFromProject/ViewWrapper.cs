using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace myRevitPlugin.Buttons.CollectElementsFromProject
{
    public class ViewWrapper : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ViewType ViewType { get; set; } 
        public Level Level { get; set; }
        public ElementId Id { get; set; }

        private bool isObjectSelected;

        public bool IsObjectSelected
        {
            get { return isObjectSelected; }
            set { isObjectSelected = value; RaisePropertyChanged(nameof(IsObjectSelected)); }
        }

        public ViewWrapper(View view)
        {
            Name = view.Name;
            Id = view.Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static explicit operator ViewWrapper(List<View> v)
        {
            throw new NotImplementedException();
        }
    }
}
