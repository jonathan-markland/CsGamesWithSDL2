using System;
using AnyGameClassLibrary;
using SDLCoverLibrary;

namespace DemoProgram3
{
    public class DemoGameImplementation : IPaintAndFrameAdvance
    {
        private GameImages _gameTextures;
        private GameSounds _gameSounds;

        private const int RetroScreenWidthInPixels = 320;
        private const int RetroScreenHeightInPixels = 256;

        // vvv DATA MODEL
        private int _positionX;
        private int _positionY;
        // ^^^ DATA MODEL


        public DemoGameImplementation(GameImages gameTextures, GameSounds gameSounds)
        {
            _gameTextures = gameTextures;
            _gameSounds = gameSounds;
            _positionX = RetroScreenWidthInPixels / 2;
            _positionY = RetroScreenHeightInPixels / 2;
        }



        public FrameAdvanceReturnStatus OnFrameAdvance(double gameTimeSeconds, Input input)
        {
            // Read the cursor keys, and update the position according to the user's input.
            // **BUT** constrain the movement so we don't go off-screen!

            // LEFT / RIGHT key handling:

            if (input.Left.CurrentlyHeld && input.Right.CurrentlyHeld)
            {
                // Do nothing!  (Subtle case!)
            }
            else if (input.Left.CurrentlyHeld)
            {
                _positionX = Math.Max(_positionX - 1, 0);
            }
            else if (input.Right.CurrentlyHeld)
            {
                _positionX = Math.Min(_positionX + 1, RetroScreenWidthInPixels - 1);
            }

            // UP / DOWN key handling:

            if (input.Up.CurrentlyHeld && input.Down.CurrentlyHeld)
            {
                // Do nothing!  (Subtle case!)
            }
            else if (input.Up.CurrentlyHeld)
            {
                _positionY = Math.Max(_positionY - 1, 0);

                if (_positionY == RetroScreenHeightInPixels / 2)
                {
                    _gameSounds.Chord.Play();
                }
            }
            else if (input.Down.CurrentlyHeld)
            {
                _positionY = Math.Min(_positionY + 1, RetroScreenHeightInPixels - 1);
            }

            return FrameAdvanceReturnStatus.ContinueRunning;
        }



        public void OnPaint(double gameTimeSeconds, Window window)
        {
            var renderer = window.Renderer;

            renderer.DrawSolidFillRectangle(new Rectangle { Left = 0, Top=0, Width=RetroScreenWidthInPixels, Height=RetroScreenHeightInPixels / 2 }, 0x0000FFu );

            renderer.DrawTexture1to1Centered(_gameTextures.Bobble, new Point { x=_positionX, y=_positionY });
        }
    }
}
