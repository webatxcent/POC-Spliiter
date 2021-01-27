using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;

namespace XCENT.JobServer.Manager.App {

    /// <summary>
    /// Use this class to access the Font Awesome fonts (and other fonts, if any) embedded in the project.
    /// </summary>
    public class AppFonts : IDisposable {

        private readonly PrivateFontCollection _privateFonts = new PrivateFontCollection();

        public Font FALight { get; }
        public Font FARegular { get; }
        public Font FASolid { get; }

        public float Size { get; }


        // Constructor
        public AppFonts(float sizeInPoints) {
            FALight = AddFont("Font Awesome 5 Pro-Light-300.otf", sizeInPoints);
            FARegular = AddFont("Font Awesome 5 Pro-Regular-400.otf", sizeInPoints);
            FASolid = AddFont("Font Awesome 5 Pro-Solid-900.otf", sizeInPoints);
            Size = sizeInPoints;
        }

        // Helper method to add a font to the private font collection.
        private Font AddFont(string fileName, float sizeInPoints) {
            bool loadAlternate = false;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);
            try {
                _privateFonts.AddFontFile(path);
            }
            catch(FileNotFoundException ex) {
                //The FileNotFound exception gets thrown for any old reason, including if OTF font format is not supported on older version OS. 
                //So if this happens, we want to try to fall back to the older TTF fonts that the older OSs will support.
                string msg = ex.Message;
                loadAlternate = true;   
            }
            if (loadAlternate) {
                string alternatePath = path.Replace(".otf", ".ttf");
                _privateFonts.AddFontFile(alternatePath);
            }
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
