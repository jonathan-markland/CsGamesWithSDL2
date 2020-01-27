using SDLCoverLibrary;

namespace DemoProgram1
{
    /// <summary>
    /// A class to conveniently hold all of the BMP image files needed
    /// after they are loaded into memory, and converted to Texture objects.
    /// </summary>
    public class GameImages
    {
        public Texture Map;
        public Texture AlliedFleetSymbol;
        public Texture EnemyFleetSymbol;
        public Texture YellowFont;

        public GameImages(Renderer renderer)
        {
            // You can't just load image files into Texture objects.
            // You must load then through the Renderer that will be drawing them.
            // This allows the Renderer to change the internal format, if need be, to
            // match what the screen uses, for highest performance.

            // Load the map image.
            // Note that the map does NOT have transparent sections.

            Map =
                renderer.CreateTextureFromBmpFile(@"Images\Map.bmp");

            // Load the font, and symbols.  These DO have transparent sections,
            // which are indicated in the original image by the colour magenta.
            // Magenta has hex colour code 0xFF00FF (hint: similar to HTML #FF00FF).
            // Therefore, magenta is said to be the "transparency colour key".

            var magentaColour = 0xFF00FFu;

            YellowFont = 
                renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\BeachHeadFont-Yellow.bmp", magentaColour);


            AlliedFleetSymbol = 
                renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\AlliedFleetSymbol.bmp", magentaColour);

            EnemyFleetSymbol = 
                renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\EnemyFleetSymbol.bmp", magentaColour);
        }
    }
}
