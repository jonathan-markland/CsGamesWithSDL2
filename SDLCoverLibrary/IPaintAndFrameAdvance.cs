
namespace SDLCoverLibrary
{
    public enum FrameAdvanceReturnStatus
    {
        ContinueRunning, StopRunning
    }

    public interface IPaintAndFrameAdvance
    {
        void OnPaint(double gameTimeSeconds, Window window);

        FrameAdvanceReturnStatus OnFrameAdvance(double gameTimeSeconds, Input input);
    }
}
