using System;
using AnyGameClassLibrary;

namespace SDLCoverLibrary
{
    public class Renderer
    {
        private readonly IntPtr _rendererHandle;

        internal Renderer(IntPtr rendererHandle)
        {
            _rendererHandle = rendererHandle;
        }

        /// <summary>
        /// Create a blank texture, for use with this renderer.
        /// This kind of texture can be selected with SelectTextureAsTarget()
        /// as the target for this Renderer's drawing operations.
        /// </summary>
        public Texture CreateBlankTexture(Dimensions dimensions)
        {
            var textureHandle =
                SDL2.SDL.SDL_CreateTexture(
                    _rendererHandle,
                    SDL2.SDL.SDL_PIXELFORMAT_RGBA8888,
                    (int) (SDL2.SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET),
                    dimensions.Width,
                    dimensions.Height);

            if (textureHandle == IntPtr.Zero)
            {
                throw new Exception($"Failed to create SDL texture of dimensions {dimensions.Width} x {dimensions.Height}.");
            }

            try
            {
                return new Texture(textureHandle, dimensions, selectableForDrawingOnto: true);
            }
            catch
            {
                SDL2.SDL.SDL_DestroyTexture(textureHandle);
                throw;
            }
        }

        /// <summary>
        /// Create a Texture by loading it from a BMP file.
        /// The returned texture must be used with this Renderer!
        /// </summary>
        public Texture CreateTextureFromBmpFile(string filePath)
        {
            var handle = SDL2.SDL.SDL_LoadBMP(filePath);
            if (handle == IntPtr.Zero)
            {
                throw new Exception($"Failed to load BMP file '{filePath}'.");
            }

            var t = typeof(SDL2.SDL.SDL_Surface);
            var s = (SDL2.SDL.SDL_Surface) System.Runtime.InteropServices.Marshal.PtrToStructure(handle, t);

            var textureHandle = SDL2.SDL.SDL_CreateTextureFromSurface(_rendererHandle, handle);
            if (textureHandle == IntPtr.Zero)
            {
                throw new Exception($"Failed to create texture for BMP file '{filePath}'.");
            }

            return new Texture(textureHandle, new Dimensions { Width = s.w, Height = s.h }, selectableForDrawingOnto: false);
        }

        /// <summary>
        /// Create a colour-key-transparent Texture by loading it from a BMP file.
        /// The returned texture must be used with this Renderer!
        /// </summary>
        public Texture CreateColourKeyTransparentTextureFromBmpFile(string filePath, uint transparencyColourRGB)
        {
            var handle = SDL2.SDL.SDL_LoadBMP(filePath);
            if (handle == IntPtr.Zero)
            {
                throw new Exception($"Failed to load BMP file '{filePath}'.");
            }

            var t = typeof(SDL2.SDL.SDL_Surface);
            var s = (SDL2.SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(handle, t);

            SDL2.SDL.SDL_SetColorKey(
                handle, 0x00001000 /*SDL_SRCCOLORKEY */,  // TODO: Investigate:  Is this constant seriously not defined in SDL2CS???
                SDL2.SDL.SDL_MapRGB(
                    s.format, 
                    (byte)(transparencyColourRGB >> 16),
                    (byte)(transparencyColourRGB >> 8),
                    (byte)(transparencyColourRGB)));

            var textureHandle = SDL2.SDL.SDL_CreateTextureFromSurface(_rendererHandle, handle);
            if (textureHandle == IntPtr.Zero)
            {
                throw new Exception($"Failed to create texture for BMP file '{filePath}'.");
            }

            return new Texture(textureHandle, new Dimensions { Width = s.w, Height = s.h }, selectableForDrawingOnto: false);
        }

        /// <summary>
        /// Selects a Texture instance as the target for subsequent drawing operations.
        /// </summary>
        public void SelectTextureAsTarget(Texture texture)
        {
            if (texture == null)
            {
                throw new Exception("Failed to set render target to Texture because object reference is null.");
            }
            if (!texture.SelectableForDrawingOnto)
            {
                throw new Exception("Failed to set render target to Texture because you cannot draw onto this Texture instance.");
            }
            if (SDL2.SDL.SDL_SetRenderTarget(_rendererHandle, texture.TextureHandle) != 0)
            {
                throw new Exception("Failed to set render target to Texture.");
            }
        }

        internal SDL2.SDL.SDL_Rect ToSDLRect(Rectangle r)
        {
            SDL2.SDL.SDL_Rect s;
            s.x = r.Left;
            s.y = r.Top;
            s.w = r.Width;
            s.h = r.Height;
            return s;
        }

        /// <summary>
        /// Draw a filled rectangle onto the surface at given position in given colour
        /// </summary>
        public void DrawSolidFillRectangle(Rectangle rectangle, uint rgb)
        {
            SDL2.SDL.SDL_SetRenderDrawColor(
                _rendererHandle,
                (byte) (rgb >> 16),
                (byte) (rgb >> 8),
                (byte) (rgb),
                0xFF);

            var rect = ToSDLRect(rectangle);
            SDL2.SDL.SDL_RenderFillRect(_rendererHandle, ref rect);
        }

        /// <summary>
        /// Draw a texture at given position, at original image size.
        /// </summary>
        public void DrawTexture1to1(Texture texture, Point topLeft)
        {
            var dstRect = new SDL2.SDL.SDL_Rect { x = topLeft.x, y = topLeft.y, w = texture.Dimensions.Width, h = texture.Dimensions.Height };
            var srcRect = new SDL2.SDL.SDL_Rect { x = 0, y = 0, w = texture.Dimensions.Width, h = texture.Dimensions.Height };
            if (SDL2.SDL.SDL_RenderCopy(_rendererHandle, texture.TextureHandle, ref srcRect, ref dstRect) != 0)
            {
                throw new Exception("Failed to draw Texture with DrawTexture1to1.");
            }
        }

        /// <summary>
        /// Draw a texture, stretched to fill the given target rectangle.
        /// </summary>
        public void DrawStretchedTexture(Texture texture, Rectangle targetRectangle)
        {
            var srcRect = new SDL2.SDL.SDL_Rect { x = 0, y = 0, w = texture.Dimensions.Width, h = texture.Dimensions.Height };
            var dstRect = ToSDLRect(targetRectangle);
            SDL2.SDL.SDL_RenderCopy(_rendererHandle, texture.TextureHandle, ref srcRect, ref dstRect);
        }

        /// <summary>
        /// Draw a rectangular portion of a texture, stretched to fill the given target rectangle.
        /// </summary>
        public void DrawStretchedTexturePortion(
                Texture texture, 
                Rectangle sourceRectangle,
                Rectangle targetRectangle)
        {
            var srcRect = ToSDLRect(sourceRectangle);
            var dstRect = ToSDLRect(targetRectangle);
            SDL2.SDL.SDL_RenderCopy(_rendererHandle, texture.TextureHandle, ref srcRect, ref dstRect);
        }
    }
}
