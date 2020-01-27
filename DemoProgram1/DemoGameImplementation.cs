using AnyGameClassLibrary;
using SDLCoverLibrary;

namespace DemoProgram1
{
    public class DemoGameImplementation : IPaintAndFrameAdvance
    {
        private GameImages _gameTextures;
        private BasicFont _yellowFont;



        public DemoGameImplementation(GameImages gameTextures)
        {
            _gameTextures = gameTextures;
            _yellowFont = new BasicFont(_gameTextures.YellowFont);
        }



        public FrameAdvanceReturnStatus OnFrameAdvance(double gameTimeSeconds, Input input)
        {
            // This is the FrameAdvance function, and is called by the library every time
            // a new frame of animation is required to be calculated (but not yet drawn!).

            // Here, this is the simplest possible implementation of this function!

            // Since this game has NO ANIMATION and processes NO INPUT, this function
            // does nothing other than return a value that tells the library to keep the
            // game running.

            // If this was a real game, you would have made data-model classes, which would
            // represent the state of your game.  In which case, this function would read
            // the status of those objects, and adjust them, to a new state, possibly
            // according to the elapsed game time (gameTimeSeconds), and the player's input,
            // which is passed in the 'input' parameter.

            // You do NOT do any drawing here.

            // We have the option of returning FrameAdvanceReturnStatus.StopRunning here,
            // and you can return to the main function to terminate the program.

            return FrameAdvanceReturnStatus.ContinueRunning;
        }



        public void OnPaint(double gameTimeSeconds, Window window)
        {
            // The library calls this function every time you need to draw a new frame.
            
            // You would read the state of your model objects, and draw things in the appropriate
            // places on the screen using the methods of the window's renderer:

            var renderer = window.Renderer;

            // In this simple example, there are NO MODEL objects at all, we just keep on
            // showing the same static screen over and over again, which is a bit boring and
            // wasteful, but this is the simplest possible demo!

            // I will use the renderer's methods to draw bitmaps onto the "retro screen".
            // If you recall, that has dimensions 320 x 200 "retro pixels" in this demo.

            // The bitmaps are from the Images folder, and were loaded by the Main function,
            // and passed into the constructor of this class (see _gameTextures).

            // Draw Map
            renderer.DrawTexture1to1(_gameTextures.Map, new Point { x = 0, y = 0 });

            // Draw symbols at fixed positions.
            renderer.DrawTexture1to1(_gameTextures.AlliedFleetSymbol, new Point { x = 200, y = 30 });
            renderer.DrawTexture1to1(_gameTextures.EnemyFleetSymbol, new Point { x = 130, y = 120 });

            // Write some text below the map to indicate the score.
            // This also demonstrates the font magification facility.
            var magLevel = new MagnificationFactors { HorizontalMagnification = 2, VerticalMagnification = 3 };
            renderer.DrawTextMagnified(_yellowFont, 160, 180, TextAlignH.CentreAlign, TextAlignV.MiddleAlign, magLevel, "SCORE 12300");
        }
    }
}
