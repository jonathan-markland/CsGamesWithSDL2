
namespace SDLCoverLibrary
{
    /// <summary>
    /// Provides access to the current state of the user's input.
    /// </summary>
    public class Input
    {
        public ButtonState Left;
        public ButtonState Right;
        public ButtonState Up;
        public ButtonState Down;
        public ButtonState Fire;
    }

    public struct ButtonState
    {
        /// <summary>
        /// True when the button has just been pressed down on this frame.
        /// </summary>
        public bool JustDown;

        /// <summary>
        /// True when the button is currently held.
        /// </summary>
        public bool CurrentlyHeld;

        /// <summary>
        /// True when the button has just been released on this frame.
        /// </summary>
        // TODO: still to implement:   public bool JustReleased;
    }
}
