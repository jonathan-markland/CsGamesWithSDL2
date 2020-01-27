
using SDLCoverLibrary;

namespace DemoProgram4
{
    public class GameSounds
    {
        public SoundEffect EnemyFiringSound;
        public SoundEffect ExplosionSound;
        public SoundEffect GameOverSound;
        public SoundEffect PlayerFiringSound;
        public SoundEffect ExtraLifeSound;

        public GameSounds()
        {
            EnemyFiringSound = new SoundEffect(@"Sound\EnemyFiringSound.wav");
            ExplosionSound = new SoundEffect(@"Sound\ExplosionSound.wav");
            GameOverSound = new SoundEffect(@"Sound\GameOverSound.wav");
            PlayerFiringSound = new SoundEffect(@"Sound\PlayerFiringSound.wav");
            ExtraLifeSound = new SoundEffect(@"Sound\ExtraLifeSound.wav");
        }
    }
}
