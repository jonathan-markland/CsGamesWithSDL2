
using AnyGameClassLibrary;


namespace SDLCoverLibrary
{
    /// <summary>
    /// Abstract font, which allows for different formats.
    /// The derived classes dictate the format of the font.
    /// </summary>
    public abstract class AbstractBaseFont
    {
        /// <summary>
        /// Obtains the dimensions, in pixels, of the extents box of the given string when written in this font.
        /// </summary>
        public abstract Dimensions GetStringDimensions(MagnificationFactors magnificationFactors, string messageText);

        /// <summary>
        /// Draws the given string in the font, at the given position, using the given renderer.
        /// </summary>
        public abstract void Draw(Renderer renderer, Point topLeft, MagnificationFactors magnificationFactors, string messageText);
    }

    /// <summary>
    /// Magnification factors for font writing.
    /// This supports integral multiples for the nicest effects with retro-graphics.
    /// </summary>
    public struct MagnificationFactors
    {
        public int HorizontalMagnification;
        public int VerticalMagnification;
    }
}

