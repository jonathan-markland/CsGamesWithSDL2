using System;
using AnyGameClassLibrary;

namespace SDLCoverLibrary
{
    /// <summary>
    /// A bitmap containing a monospaced font.
    /// This supports 0-9, capital A-Z, and SPACE only.
    /// All other characters will appear as spaces.
    /// </summary>
    public class BasicFont: AbstractBaseFont
    {
        private readonly Texture FontTexture;
        private readonly int CharWidth;

        public BasicFont(Texture fontTexture)
        {
            if ((fontTexture.Dimensions.Width % 36) != 0)
            {
                throw new Exception(
                    "Cannot use given Texture as a BasicFont because it must have 36 monospaced characters, " +
                    "0-9 then A-Z, and thus a width divisible by 36.");
            }

            CharWidth = fontTexture.Dimensions.Width / 36;
            FontTexture = fontTexture;
        }

        public override void Draw(Renderer renderer, Point topLeft, MagnificationFactors magnificationFactors, string messageText)
        {
            int x = topLeft.x;
            int y = topLeft.y;
            int w = CharWidth;
            int h = FontTexture.Dimensions.Height;

            int magx = magnificationFactors.HorizontalMagnification;
            int magy = magnificationFactors.VerticalMagnification;

            var destw = w * magx;
            var desth = h * magy;

            foreach (char c in messageText)
            {
                int n = -1;
                if (c >= '0' && c <= '9')
                {
                    n = ((int)c) - 48;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    n = ((int)c) - 55;
                }
                
                if (n != -1)
                {
                    var sourceRect = new Rectangle { Left = n * CharWidth, Top = 0, Width = w, Height = h };
                    var destRect = new Rectangle { Left = x, Top = y, Width = destw, Height = desth };
                    renderer.DrawStretchedTexturePortion(FontTexture, sourceRect, destRect);
                }

                x += destw;
            }
        }

        public override Dimensions GetStringDimensions(MagnificationFactors magnificationFactors, string messageText)
        {
            return new Dimensions
            {
                Width = messageText.Length * CharWidth * magnificationFactors.HorizontalMagnification,
                Height = FontTexture.Dimensions.Height * magnificationFactors.VerticalMagnification
            };
        }
    }
}
