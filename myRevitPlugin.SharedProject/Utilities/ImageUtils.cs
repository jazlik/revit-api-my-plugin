using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace myRevitPlugin.Utilities
{
    public static class ImageUtils
    {
        // This functions loads images for buttons from the Resources
        public static BitmapImage LoadImage(Assembly a, string name)
        {
            var img = new BitmapImage();
            try
            {
                // Searches for all the Resources in the assembly. Gets the first resource which name that contains the "name" provided for function.
                var resourceName = a.GetManifestResourceNames().FirstOrDefault(x => x.Contains(name));
                var stream = a.GetManifestResourceStream(resourceName);

                img.BeginInit();
                img.StreamSource = stream;
                img.EndInit();
            }
            catch (Exception e)
            {
                // Ignored.
            }

            return img;
        }
    }
}