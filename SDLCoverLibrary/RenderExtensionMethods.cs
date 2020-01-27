
using AnyGameClassLibrary;

namespace SDLCoverLibrary
{ 
    public enum TextAlignH
    {
        LeftAlign, CentreAlign, RightAlign
    }

    public enum TextAlignV
    {
        TopAlign, MiddleAlign, BottomAlign
    }

    public static class RenderExtensionMethods
    {
        public static void DrawTexture1to1Centered(this Renderer renderer, Texture texture, Point centrePoint)
        {
            renderer.DrawTexture1to1(
                texture, 
                new Point
                {
                    x = centrePoint.x - (texture.Dimensions.Width / 2),
                    y = centrePoint.y - (texture.Dimensions.Height / 2)
                });
        }



        public static void DrawText(
            this Renderer renderer,
            BasicFont font,
            int x, int y,
            TextAlignH alignH, TextAlignV alignV,
            string messageText)
        {
            DrawTextMagnified(
                renderer, font, x, y, alignH, alignV, 
                new MagnificationFactors
                {
                    HorizontalMagnification = 1,
                    VerticalMagnification = 1
                }, 
                messageText);
        }



        public static void DrawTextMagnified(
            this Renderer renderer, 
            BasicFont font, 
            int x, int y, 
            TextAlignH alignH, TextAlignV alignV,
            MagnificationFactors magnificationFactors,
            string messageText)
        {
            var textDimensions = font.GetStringDimensions(magnificationFactors, messageText);

            if (alignH == TextAlignH.CentreAlign)
            {
                x -= textDimensions.Width / 2;
            }
            else if (alignH == TextAlignH.RightAlign)
            {
                x -= textDimensions.Width;
            }

            if (alignV == TextAlignV.MiddleAlign)
            {
                y -= textDimensions.Height / 2;
            }
            else if (alignV == TextAlignV.BottomAlign)
            {
                y -= textDimensions.Height;
            }

            font.Draw(renderer, new Point { x = x, y = y }, magnificationFactors, messageText);
        }
    }
}
