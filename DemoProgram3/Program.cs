using System;
using SDLCoverLibrary;
using AnyGameClassLibrary;

namespace DemoProgram3
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
                        var mainWindowSize = new Dimensions { Width = 1280, Height = 1024 };
                        var retroScreenSize = new Dimensions { Width = 320, Height = 256 };
                        var mainWindow = new Window("Data model + input driven - DemoProgram3", mainWindowSize);
                        var gameImages = new GameImages(mainWindow.Renderer);
                        var gameSounds = new GameSounds();
                        mainWindow.RunUsing(new DemoGameImplementation(gameImages, gameSounds), retroScreenSize);
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
