using System;
using AnyGameClassLibrary;

namespace SDLCoverLibrary
{
    public class Texture
    {
        // TODO:  This could do with destroying the texture instances with SDL, may need IDispose for that.

        private readonly IntPtr _textureHandle;
        internal readonly bool SelectableForDrawingOnto;

        internal Texture(IntPtr textureHandle, Dimensions dimensions, bool selectableForDrawingOnto)
        {
            _textureHandle = textureHandle;
            Dimensions = dimensions;
            SelectableForDrawingOnto = selectableForDrawingOnto;
        }

        /// <summary>
        /// The dimensions of this Texture in pixels.
        /// </summary>
        public Dimensions Dimensions
        {
            get; private set;
        }

        internal IntPtr TextureHandle
        {
            get { return _textureHandle; }
        }

    }
}
