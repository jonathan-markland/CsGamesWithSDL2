using System;
using AnyGameClassLibrary;
using SDLCoverLibrary;

// TODO: Sound
// TODO: Support all main keys.
// TODO: Reasonable examples.

namespace DemoProgram1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialise Jonathan's SDLCoverLibrary, which will initialise the SDL library itself,
                // and will perform the actions in the anonymous function body below.

                SDLCoverLibrary.Init.WithSDLDo(
                    () =>
                    {
                        // Establish the (initial) size of the main desktop window.
                        var mainWindowSize = new Dimensions { Width = 1200, Height = 800 };

                        // Establish the (initial) size of the "retro screen", which is an off-screen
                        // bitmap onto which you will draw the scene images for the whole display.
                        // It is *also*, and importantly, the coordinate scheme in which your 
                        // game /program will work.
                        // I have chosen dimensions to give chunky retro-game pixels, because retro
                        // is what my SDLCoverLibrary is all about!  This means you never need to
                        // worry about what the main screen's dimensions actually are.
                        var retroScreenSize = new Dimensions { Width = 320, Height = 200 };

                        // Create the main window:
                        var mainWindow = new Window("SDL2 + C# + SDLCoverLibrary - DemoProgram1", mainWindowSize);

                        // Load the .BMP image files that this particular demo program needs.
                        // One of these is actually a font definition (see Images folder).
                        var gameImages = new GameImages(mainWindow.Renderer);

                        // Now start the main event processing loop.  This function call will
                        // NOT return until it is time to terminate the entire program.
                        // We also tell the library the dimensions we desire to use for the "retro screen".
                        // Note that class DemoGameImplementation is the implementation for
                        // this particular demo, so we're passing in the images into its constructor.
                        mainWindow.RunUsing(new DemoGameImplementation(gameImages), retroScreenSize);
                    }
                );
            }
            catch(Exception e)
            {
                // You could do something different here, of course.

                Console.WriteLine("Unhandled exception during program: " + e.ToString());
                Console.Write(e.StackTrace);
            }
        }
    }
}
