using System;
using SDLCoverLibrary;

namespace DemoProgram4
{
    public class DemoGameImplementation : IPaintAndFrameAdvance
    {
        private GameImages _gameTextures;         // TODO:  Consider:  Underscores, or not?
        private GameSounds _gameSounds;
        private BasicFont _yellowFont;

        private WorldModel _world;


        public DemoGameImplementation(GameImages gameTextures, GameSounds gameSounds)
        {
            _gameTextures = gameTextures;
            _gameSounds = gameSounds;
            _yellowFont = new BasicFont(gameTextures.YellowFont);
            _world = new WorldModel(gameTextures, gameSounds, _yellowFont, gameTextures.Ship.Dimensions.Width, gameTextures.Alien1.Dimensions.Width);
        }



        public FrameAdvanceReturnStatus OnFrameAdvance(double gameTimeSeconds, Input input)
        {
            _world.OnFrameAdvance(gameTimeSeconds, input);
            return FrameAdvanceReturnStatus.ContinueRunning;
        }



        public void OnPaint(double gameTimeSeconds, Window window)
        {
            _world.OnPaint(gameTimeSeconds, window.Renderer);
        }
    }
}
