using SDLCoverLibrary;

namespace DemoProgram4
{
    /// <summary>
    /// A class to conveniently hold all of the BMP image files needed
    /// after they are loaded into memory, and converted to Texture objects.
    /// </summary>
    public class GameImages
    {
        public Texture Alien1;
        public Texture EnemyBullet;
        public Texture Explosion;
        public Texture PlayerBullet;
        public Texture Ship;
        public Texture Star;
        public Texture YellowFont;

        public GameImages(Renderer renderer)
        {
            var keyColour = 0x000000u; // black

            Alien1 = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\Alien1.bmp", keyColour);
            EnemyBullet = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\EnemyBullet.bmp", keyColour);
            Explosion = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\Explosion.bmp", keyColour);
            PlayerBullet = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\PlayerBullet.bmp", keyColour);
            Ship = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\Ship.bmp", keyColour);
            Star = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\Star.bmp", keyColour);
            YellowFont = renderer.CreateColourKeyTransparentTextureFromBmpFile(@"Images\YellowFont.bmp", keyColour);
        }
    }
}
