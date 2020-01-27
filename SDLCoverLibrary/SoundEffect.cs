using System;

namespace SDLCoverLibrary
{
    public class SoundEffect
    {
        internal readonly IntPtr _wavFileHandle;
        private string _filePath;

        public SoundEffect(string filePath)
        {
            _filePath = filePath;
            _wavFileHandle = SDL2.SDL_mixer.Mix_LoadWAV(filePath);
            if (_wavFileHandle == IntPtr.Zero)
            {
                throw new Exception($"Failed to load WAV file '{filePath}'.");
            }
        }

        public void Play()
        {
            if (SDL2.SDL_mixer.Mix_PlayChannel(-1, _wavFileHandle, 0) == -1)
            {
                // Commented out because it can be overloaded, in which case we don't care.
                // throw new Exception($"Failed to play the SoundEffect identified by file '{_filePath}'.  Load succeeded, but playback failed."); ;
            }
        }
    }
}
