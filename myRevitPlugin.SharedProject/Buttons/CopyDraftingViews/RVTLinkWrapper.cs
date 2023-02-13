using System;
using Autodesk.Revit.DB;

namespace myRevitPlugin.Buttons.CopyDraftingViews
{
    public class RVTLinkWrapper/* : INotifyPropertyChanged*/
    {
        public string Name { get; set; }

        public ElementId Id { get; set; }

        private bool isObjectSelected;

        public bool IsObjectSelected
        {
            get { return isObjectSelected; }
            set { isObjectSelected = value;
                /*RaisePropertyChanged(nameof(IsObjectSelected));*/ }
        }

        public RVTLinkWrapper(RevitLinkInstance link)
        {
            Name = link.Name;
            Id = link.Id;
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void RaisePropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
