using AnyGameClassLibrary;
using SDLCoverLibrary;

namespace DemoProgram3
{
    /// <summary>
    /// A class to conveniently hold all of the BMP image files needed
    /// after they are loaded into memory, and converted to Texture objects.
    /// </summary>
    public class GameImages
    {
        public Texture Bobble;

        public GameImages(Renderer renderer)
        {
            var magentaColour = 0xFF00FFu;

            Bobble =
                renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\Bobble.bmp", magentaColour);
        }
    }
}
