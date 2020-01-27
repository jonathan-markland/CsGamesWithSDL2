using System;
using SDL2;
using AnyGameClassLibrary;

namespace SDLCoverLibrary
{
    public enum RetroScreenDisplayMode
    {
        /// <summary>
        /// The retro-screen will be scaled to fit the host's desktop window
        /// using Nearest Neighbour stretching (pixellated appearance).
        /// </summary>
        NearestNeighbour,

        /// <summary>
        /// The retro-screen will be scaled to fit the host's desktop window
        /// using Linear Interpolation stretching (smoothed / possibly blurry appearance).
        /// </summary>
        Smoothed
    }

    public enum RetroScreenDisplayAspect
    {
        /// <summary>
        /// The retro-screen will fill the entirety of the host's desktop window,
        /// irrespective of the aspect ratio.
        /// </summary>
        StretchToFillWindow,

        /// <summary>
        /// The retro-screen will be scaled to fill the host's desktop window,
        /// while retaining its aspect ratio.  Black bars will be added to
        /// the sides of the host's window as necessary.
        /// </summary>
        FillWindowKeepingAspectRatio
    }

    public class Window
    {
        private readonly IntPtr _windowHandle;
        private readonly IntPtr _rendererHandle;
        private readonly Renderer _renderer;
        private Texture _retroScreenTexture;

        /// <summary>
        /// Creates a window on the host's desktop of specified dimensions in pixels.
        /// </summary>
        public Window(string windowTitle, Dimensions windowDimensions)
        {
            var windowFlags =
                SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |
                SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;

            var windowNativeInt =
                SDL.SDL_CreateWindow(
                        windowTitle,
                        32, 32,   // TODO: Calculate central position on the screen?
                        windowDimensions.Width,
                        windowDimensions.Height,
                        windowFlags);

            if (windowNativeInt == IntPtr.Zero)
            {
                throw new Exception("Failed to create SDL window.");
            }

            var renderFlags =
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED;

            var rendererNativeInt =
                SDL.SDL_CreateRenderer(windowNativeInt, -1, renderFlags);

            if (rendererNativeInt == IntPtr.Zero)
            {
                SDL.SDL_DestroyWindow(windowNativeInt);
                throw new Exception("Failed to create SDL renderer for SDL window.");
            }

            _rendererHandle = rendererNativeInt;
            _windowHandle = windowNativeInt;

            // TODO: We don't yet do proper clean up in the event of exceptions.

            _renderer = new Renderer(_rendererHandle);

            ClearRetroScreenTextureBeforeOnPaint = true;
            DisplayMode = RetroScreenDisplayMode.NearestNeighbour;
            DisplayAspect = RetroScreenDisplayAspect.StretchToFillWindow;
        }

        /// <summary>
        /// Obtains the Texture object that hosts the client's Retro game screen.
        /// </summary>
        public Texture RetroScreenTexture
        {
            get { return _retroScreenTexture; }
        }

        /// <summary>
        /// Selects whether this library will clear the retro-screen texture
        /// </summary>
        public bool ClearRetroScreenTextureBeforeOnPaint
        {
            get; set;
        }

        public RetroScreenDisplayMode DisplayMode
        {
            get; /*set; TODO */
        }

        public RetroScreenDisplayAspect DisplayAspect
        {
            get; /*set; TODO */
        }

        /// <summary>
        /// Obtain the renderer object that will draw onto this window, or Texture objects that
        /// are compatible with this window.
        /// </summary>
        public Renderer Renderer
        {
            get { return _renderer; }
        }

        /// <summary>
        /// Called once, from your main function, to start the event loop.
        /// </summary>
        /// <param name="handlers">An object that you have defined that will handle paint and update events.</param>
        /// <param name="retroScreenDimensions">The dimensions of your retro game screen.</param>
        public void RunUsing(IPaintAndFrameAdvance handlers, Dimensions retroScreenDimensions)
        {
            _retroScreenTexture = _renderer.CreateBlankTexture(retroScreenDimensions);

            var timerID = SDL.SDL_AddTimer(20, new SDL.SDL_TimerCallback(TimerCallback), IntPtr.Zero);
            if (timerID == 0)
            {
                throw new Exception("Failed to install the gameplay timer.");
            }

            var retroScreenRectangle = new Rectangle { Left = 0, Top = 0, Width = retroScreenDimensions.Width, Height = retroScreenDimensions.Height };

            uint tickCount = 0;

            var input = new Input();

            bool quitLoop = false;
            while(!quitLoop)
            {
                var ev = new SDL.SDL_Event();
                while (SDL.SDL_WaitEvent(out ev) != 0 && !quitLoop)
                {
                    var msg = ev.type;

                    if (msg == SDL.SDL_EventType.SDL_QUIT)
                    {
                        quitLoop = true;
                    }
                    else if (msg == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        MapKeydown(SDL.SDL_Scancode.SDL_SCANCODE_LEFT, ref ev, ref input.Left);
                        MapKeydown(SDL.SDL_Scancode.SDL_SCANCODE_RIGHT, ref ev, ref input.Right);
                        MapKeydown(SDL.SDL_Scancode.SDL_SCANCODE_UP, ref ev, ref input.Up);
                        MapKeydown(SDL.SDL_Scancode.SDL_SCANCODE_DOWN, ref ev, ref input.Down);
                        MapKeydown(SDL.SDL_Scancode.SDL_SCANCODE_Z, ref ev, ref input.Fire);
                    }
                    else if (msg == SDL.SDL_EventType.SDL_KEYUP)
                    {
                        MapKeyUp(SDL.SDL_Scancode.SDL_SCANCODE_LEFT, ref ev, ref input.Left);
                        MapKeyUp(SDL.SDL_Scancode.SDL_SCANCODE_RIGHT, ref ev, ref input.Right);
                        MapKeyUp(SDL.SDL_Scancode.SDL_SCANCODE_UP, ref ev, ref input.Up);
                        MapKeyUp(SDL.SDL_Scancode.SDL_SCANCODE_DOWN, ref ev, ref input.Down);
                        MapKeyUp(SDL.SDL_Scancode.SDL_SCANCODE_Z, ref ev, ref input.Fire);
                    }
                    else if (msg == SDL.SDL_EventType.SDL_USEREVENT)  // This is inserted into the message queue by TimerCallback()
                    {
                        double gameTimeSeconds = ((double)tickCount) / 50.0;
                        SDL.SDL_SetRenderTarget(_rendererHandle, _retroScreenTexture.TextureHandle);
                        if (ClearRetroScreenTextureBeforeOnPaint)
                        {
                            _renderer.DrawSolidFillRectangle(retroScreenRectangle, 0x000000);
                        }
                        handlers.OnPaint(gameTimeSeconds, this);
                        SDL.SDL_SetRenderTarget(_rendererHandle, IntPtr.Zero);
                        SDL.SDL_RenderCopy(_rendererHandle, _retroScreenTexture.TextureHandle, IntPtr.Zero, IntPtr.Zero);
                        SDL.SDL_RenderPresent(_rendererHandle);
                        var resultStatus = handlers.OnFrameAdvance(gameTimeSeconds, input);
                        if (resultStatus == FrameAdvanceReturnStatus.StopRunning)
                        {
                            quitLoop = true;
                        }
                        ++tickCount;
                    }
                }
            }

            // TODO: Usually game terminates, but we should clean up all Textures created.
        }

        private static void MapKeydown(SDL.SDL_Scancode sdlScanCode, ref SDL.SDL_Event ev, ref ButtonState buttonState)
        {
            if (ev.key.keysym.scancode == sdlScanCode)
            {
                buttonState.JustDown = !buttonState.CurrentlyHeld;
                buttonState.CurrentlyHeld = true;
            }
        }

        private static void MapKeyUp(SDL.SDL_Scancode sdlScanCode, ref SDL.SDL_Event ev, ref ButtonState buttonState)
        {
            if (ev.key.keysym.scancode == sdlScanCode)
            {
                buttonState.JustDown = false;
                buttonState.CurrentlyHeld = false;
            }
        }

        private static uint TimerCallback(uint interval, IntPtr param)
        {
            var ev = new SDL2.SDL.SDL_Event();
            ev.type = SDL.SDL_EventType.SDL_USEREVENT;
            ev.user.code = 0;
            ev.user.data1 = IntPtr.Zero;
            ev.user.data2 = IntPtr.Zero;
            SDL.SDL_PushEvent(ref ev);
            return interval;  // We can return 0 to cancel the timer here, or interval to keep it going.
        }
    }
}