using System;
using SDL2;

namespace SDLCoverLibrary
{
    public static class Init
    {
        private const int MixerFrequency = 22050;

        public static void WithSDLDo(Action action)
        {
            if (SDL2.SDL.SDL_Init(SDL.SDL_INIT_TIMER | SDL.SDL_INIT_AUDIO) == 0)
            {
                try
                {
                    if (SDL2.SDL_mixer.Mix_OpenAudio(MixerFrequency, SDL2.SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) == 0)
                    {
                        try
                        {
                            action();
                        }
                        finally
                        {
                            SDL2.SDL_mixer.Mix_CloseAudio();
                        }
                    }
                    else
                    {
                        throw new Exception("Could not initialise the SDL_mixer library for sound output.");
                    }
                }
                finally
                {
                    SDL2.SDL.SDL_Quit();
                }
            }
            else
            {
                throw new Exception("Could not initialise the SDL2 library.");
            }
        }
    }
}
