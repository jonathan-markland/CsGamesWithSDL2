using System;
using AnyGameClassLibrary;
using SDLCoverLibrary;

namespace DemoProgram2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SDLCoverLibrary.Init.WithSDLDo(
                    () =>
                    {
                        var mainWindowSize = new Dimensions { Width =  1280, Height = 1024 };
                        var retroScreenSize = new Dimensions { Width = 320, Height = 256 };
                        var mainWindow = new Window("Sine wave - DemoProgram2", mainWindowSize);
                        var gameImages = new GameImages(mainWindow.Renderer);
                        mainWindow.RunUsing(new DemoGameImplementation(gameImages), retroScreenSize);
                    }
                );
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception during program: " + e.ToString());
                Console.Write(e.StackTrace);
            }
        }
    }
}
