using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace POC_Splitter {

    /// <summary>
    /// Use this class to access the Font Awesome fonts (and other fonts, if any) embedded in the project.
    /// </summary>
    public class AppFonts : IDisposable {

        private readonly PrivateFontCollection _privateFonts = new PrivateFontCollection();

        public Font FALight { get; }
        public Font FARegular { get; }
        public Font FASolid { get; }

        public float Size { get; internal set; }

        // Constructor
        public AppFonts(float sizeInPoints) {
            FALight = AddFont("Font Awesome 5 Pro-Light-300.otf", sizeInPoints);
            FARegular = AddFont("Font Awesome 5 Pro-Regular-400.otf", sizeInPoints);
            FASolid = AddFont("Font Awesome 5 Pro-Solid-900.otf", sizeInPoints);
            Size = sizeInPoints;
        }

        // Helper method to add a font to the private font collection.
        private Font AddFont(string fileName, float sizeInPoints) {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);
            _privateFonts.AddFontFile(path);
            int index = _privateFonts.Families.Length - 1;
            return new Font(_privateFonts.Families[index], sizeInPoints, GraphicsUnit.Point);
        }

        // Destructor
        ~AppFonts() {
            Dispose(false);
        }

        // IDisposable implementation
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this); // Hey, GC: don't bother calling finalize later
        }

        // Dispose helper method.
        private void Dispose(bool isSafeToFreeManagedObjects) {
            // Free unmanaged objects.
            // . . .

            if (isSafeToFreeManagedObjects) {
                // Free managed objects.
                FALight.Dispose();
                FARegular.Dispose();
                FASolid.Dispose();
                _privateFonts.Dispose();
            }
        }

    }

}
