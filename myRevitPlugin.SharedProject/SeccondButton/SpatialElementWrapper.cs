using Autodesk.Revit.DB;
using System.ComponentModel;

namespace myRevitPlugin.SeccondButton
{
    public class SpatialElementWrapper : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double Area { get; set; }
        public ElementId Id { get; set; }

        private bool isObjectSelected;

        public bool IsObjectSelected
        {
            get { return isObjectSelected; }
            // RaisePropertyChanged needs propertyName in a string.
            // nameof(IsObjectSelected) returns the string name of the IsObjectSelected property.
            // In this case it returns "IsObjectSelected".
            set { isObjectSelected = value; RaisePropertyChanged(nameof(IsObjectSelected));}
        }

        public SpatialElementWrapper(SpatialElement room)
        {
            Name = room.Name;
            Area = room.Area;
            Id = room.Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
