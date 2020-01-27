using AnyGameClassLibrary;
using SDLCoverLibrary;

namespace DemoProgram2
{
    public class DemoGameImplementation : IPaintAndFrameAdvance
    {
        private GameImages _gameTextures;

        private const int RetroScreenWidthInPixels = 320;
        private const int RetroScreenHeightInPixels = 256;
        private const float SineWaveHeightFromCentreLine = 50.0F;
        private const float SineMotionPixelsPerSecond = 75.0F;


        public DemoGameImplementation(GameImages gameTextures)
        {
            _gameTextures = gameTextures;
        }



        public FrameAdvanceReturnStatus OnFrameAdvance(double gameTimeSeconds, Input input)
        {
            return FrameAdvanceReturnStatus.ContinueRunning;
        }



        public void OnPaint(double gameTimeSeconds, Window window)
        {
            var renderer = window.Renderer;

            float quarterScreenWidth = ((float)RetroScreenWidthInPixels) / 2.0F;
            float halfScreenHeight = RetroScreenHeightInPixels / 2;
            float scale = (2.0F * System.MathF.PI) / quarterScreenWidth;

            float offset = ((float)gameTimeSeconds) * SineMotionPixelsPerSecond;

            for (int i=0; i < RetroScreenWidthInPixels; i += 10)
            {
                double y = halfScreenHeight + System.MathF.Sin((i + offset) * scale) * SineWaveHeightFromCentreLine;
                int inty = (int)(y + 0.5);
                renderer.DrawTexture1to1Centered(_gameTextures.Bobble, new Point { x = i, y = inty });
            }
        }
    }
}
