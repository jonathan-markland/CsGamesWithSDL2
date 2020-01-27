using System;
using System.Collections.Generic;
using SDLCoverLibrary;   // TODO: Let's try not to have this in here.
using AnyGameClassLibrary;

namespace DemoProgram4
{
    public static class RetroScreen
    {
        public const int WidthInPixels = 320;
        public const int HeightInPixels = 256;
    }



    public class ShipModel
    {
        public const double RespawnDelay = 6.0;  // Must be enough to allow all residual aliens to move past.
        public const int Margin = 30;
        public const int HighestY = ShipModel.Margin;
        public const int LowestY = RetroScreen.HeightInPixels - ShipModel.Margin;
        public const int Step = 3;
        public const double MinFiringIntervalForPlayer = 0.2;  // seconds

        public int Width;
        public Point CentrePoint = new Point(Margin, RetroScreen.HeightInPixels / 2);
        public double MostRecentPlayerFiringTime = 0.0;
        public bool IsDestroyed = false;
        public double TimeOfDestruction;
    }

    public class AlienListModel
    {
        public const int MaxAliensOnScreenAtOnce = 5;
        public const double TimeBetweenAddingAliens = 1.0;  // seconds

        // Quiz:  What would be the impact of making the list private?

        public List<AlienModel> Aliens = new List<AlienModel>();
        public double LastTimeWeConsideredAddingAnAlien = 0.0;  // TODO: We *assume* the game start time will always be 0.0  (which it is at the time of writing!)
    }

    public class AlienModel
    {
        public const int PercentChangeAlienFiresWhenCreated = 30;

        public const int Margin = 20;
        public const int HighestY = Margin;
        public const int LowestY = RetroScreen.HeightInPixels - Margin;
        public const int IntroductionX = RetroScreen.WidthInPixels + Margin;  // so they come from off-screen!

        public Point CentrePoint { get; private set; }
        private int Speed;
        public static int AlienWidth; // Is the same for all (for now!)

        public AlienModel(Point initialCentrePoint, int speed)
        {
            CentrePoint = initialCentrePoint;
            Speed = speed;
        }

        public void ShuntPosition()
        {
            CentrePoint = CentrePoint.ShuntedBy(-Speed, 0);
        }
    }

    public class AlienBulletListModel
    {
        // Quiz:  What would be the impact of making the list private?
        public List<AlienBulletModel> AlienBullets = new List<AlienBulletModel>();
    }

    public class AlienBulletModel
    {
        private const int BulletStep = 5;

        public Point CentrePoint { get; private set; }

        public AlienBulletModel(Point initialCentrePoint)
        {
            CentrePoint = initialCentrePoint;
        }

        public void ShuntPosition()
        {
            CentrePoint = CentrePoint.ShuntedBy(-BulletStep, 0);
        }
    }

    public class ShipBulletListModel
    {
        // Quiz:  What would be the impact of making the list private?
        public List<ShipBulletModel> ShipBullets = new List<ShipBulletModel>();
    }

    public class ShipBulletModel
    {
        private const int BulletStep = 5;

        public Point CentrePoint { get; private set; }

        public ShipBulletModel(Point initialCentrePoint)
        {
            CentrePoint = initialCentrePoint;
        }

        public void ShuntPosition()
        {
            CentrePoint = CentrePoint.ShuntedBy(BulletStep, 0);
        }
    }

    public class StarscapeModel
    {
        public const int MaxStarsOnScreenAtOnce = 25;
        public const double TimeBetweenAddingStars = 0.1;  // seconds

        // Quiz:  What would be the impact of making the list private?
        public List<StarModel> Stars = new List<StarModel>();
        public double LastTimeWeConsideredAddingAStar = 0.0;  // TODO: We *assume* the game start time will always be 0.0  (which it is at the time of writing!)
    }

    public class StarModel
    {
        public Point CentrePoint { get; private set; }
        public int Speed { get; private set; }

        public StarModel(Point initialCentrePoint, int speed)
        {
            CentrePoint = initialCentrePoint;
            Speed = speed;
        }

        public void ShuntPosition()
        {
            CentrePoint = CentrePoint.ShuntedBy(-Speed, 0);
        }
    }

    public class ExplosionListModel
    {
        // Quiz:  What would be the impact of making the list private?
        public List<ExplosionModel> Explosions = new List<ExplosionModel>();
    }

    public class ExplosionModel
    {
        public const double ExplosionDurationSeconds = 0.2;

        public Point ExplosionCentre { get; private set; }
        public double StartTime { get; private set; }

        public ExplosionModel(Point explosionCentre, double gameTimeSeconds)
        {
            ExplosionCentre = explosionCentre;
            StartTime = gameTimeSeconds;
        }
    }

    public class ScoreModel
    {
        public const int ScoreMargin = 20;
        public const int ScoreX = RetroScreen.WidthInPixels - ScoreMargin;
        public const int ScoreY = ScoreMargin;

        public int Score { get; private set; }
        public string ScoreText { get; private set; }

        public ScoreModel()
        {
            SetScore(0);
        }

        public void SetScore(int n)
        {
            var tail = (n == 0) ? "" : "00";
            Score = n;
            ScoreText = n.ToString() + tail;   // Quiz:  Why store the text?
        }
    }



    public static class Physics
    {
        private const int CollisionDetectRange = 10;



        public static void MoveShipAccordingToKeys(Input input, ShipModel ship)
        {
            if (!ship.IsDestroyed)
            {
                var y = ship.CentrePoint.y;

                if (input.Up.CurrentlyHeld && input.Down.CurrentlyHeld)
                {
                    // Do nothing!         // Quiz:  Why do I check this case?
                }
                else if (input.Up.CurrentlyHeld)
                {
                    y = Math.Max(y - ShipModel.Step, ShipModel.HighestY);
                }
                else if (input.Down.CurrentlyHeld)
                {
                    y = Math.Min(y + ShipModel.Step, ShipModel.LowestY);
                }

                ship.CentrePoint.y = y;
            }
        }



        public static void MoveShipBullets(ShipBulletListModel shipBulletsList)
        {
            foreach (var shipBullet in shipBulletsList.ShipBullets)
            {
                shipBullet.ShuntPosition();
            }
        }



        public static void MoveAlienBullets(AlienBulletListModel alienBulletsList)
        {
            foreach (var alienBullet in alienBulletsList.AlienBullets)
            {
                alienBullet.ShuntPosition();
            }
        }



        public static void MoveAliens(AlienListModel alienList)
        {
            foreach (var alien in alienList.Aliens)
            {
                alien.ShuntPosition();
            }
        }



        public static void MoveStars(StarscapeModel starList)
        {
            foreach (var star in starList.Stars)
            {
                star.ShuntPosition();
            }
        }



        public static void RemoveOffScreenObjects(AlienListModel aliensList, ShipBulletListModel shipBulletList, AlienBulletListModel alienBulletList, StarscapeModel starList, ScoreModel scoring)
        {
            shipBulletList.ShipBullets.RemoveAll(b => b.CentrePoint.x > RetroScreen.WidthInPixels);
            alienBulletList.AlienBullets.RemoveAll(b => b.CentrePoint.x < 0);
            starList.Stars.RemoveAll(s => s.CentrePoint.x < 0);
            int numberOfAliensRemoved = aliensList.Aliens.RemoveAll(a => a.CentrePoint.x < 0);
            scoring.SetScore(Math.Max(0, scoring.Score - numberOfAliensRemoved));
        }



        public static void RemoveCompletedExplosions(double gameTimeSeconds, ExplosionListModel explosionsList)
        {
            explosionsList.Explosions.RemoveAll(explosion =>
            {
                var elapsedSinceExplosionStart = gameTimeSeconds - explosion.StartTime;
                return elapsedSinceExplosionStart > ExplosionModel.ExplosionDurationSeconds;
            });
        }



        public static void ConsiderAddingAlienAndThen(double gameTimeSeconds, AlienListModel alienList, Random randomGenerator, Action<AlienModel> andThen)
        {
            var elapsedTime = gameTimeSeconds - alienList.LastTimeWeConsideredAddingAnAlien;       // Quiz:  Why work things out in seconds (as given by gameTimeSeconds), compared to the Alien.Speed and Star.Speed technique?   What's the difference, and is one technique best?
            if (elapsedTime > AlienListModel.TimeBetweenAddingAliens)
            {
                if (alienList.Aliens.Count < AlienListModel.MaxAliensOnScreenAtOnce)   // Quiz:  Why limit this?
                {
                    var alienY = randomGenerator.Next(AlienModel.HighestY, AlienModel.LowestY);  // NB: Y axis runs the other way up from normal mathematics!
                    var speed = randomGenerator.Next(1, 5);
                    var newAlien = new AlienModel(new Point(AlienModel.IntroductionX, alienY), speed);
                    alienList.Aliens.Add(newAlien);
                    andThen(newAlien);
                }
                alienList.LastTimeWeConsideredAddingAnAlien = gameTimeSeconds;  // Quiz:  Why are we doing this here?
            }
        }



        public static void ConsiderAddingAlien(double gameTimeSeconds, ShipModel ship, AlienListModel alienList, Random randomGenerator, AlienBulletListModel alienBulletList, GameSounds gameSounds)
        {
            if (!ship.IsDestroyed)  // Quiz:  Why not add aliens in the period while the ship is "destroyed"?
            {
                ConsiderAddingAlienAndThen(gameTimeSeconds, alienList, randomGenerator, alien =>
                {
                    if (randomGenerator.Next(0, 100) > AlienModel.PercentChangeAlienFiresWhenCreated)    // Quiz:   Does my hard-coded constant really 100 matter here?  Is this bad practice?
                    {
                        CauseAlienToFire(alien, alienBulletList, gameSounds);
                    }
                });
            }
        }



        public static void CauseAlienToFire(AlienModel alien, AlienBulletListModel alienBulletList, GameSounds gameSounds)      // Quiz:   What future purpose have I catered for by separating this out?  If you can guess, how could that purpose be fulfilled?
        {
            alienBulletList.AlienBullets.Add(new AlienBulletModel(alien.CentrePoint.ShuntedBy(-AlienModel.AlienWidth, 0)));
            gameSounds.EnemyFiringSound.Play();
        }



        public static void ConsiderAddingStar(double gameTimeSeconds, StarscapeModel StarsList, Random RandomGenerator)    // Quiz:  This is very similar to ConsiderAddingAlien().  What could you do about that?
        {
            var elapsedTime = gameTimeSeconds - StarsList.LastTimeWeConsideredAddingAStar;
            if (elapsedTime > StarscapeModel.TimeBetweenAddingStars)
            {
                if (StarsList.Stars.Count < StarscapeModel.MaxStarsOnScreenAtOnce)
                {
                    var starY = RandomGenerator.Next(0, RetroScreen.HeightInPixels);  // NB: Y axis runs the other way up from normal mathematics!
                    var speed = RandomGenerator.Next(1, 4);
                    var newStar = new StarModel(new Point(RetroScreen.WidthInPixels, starY), speed);
                    StarsList.Stars.Add(newStar);
                }
                StarsList.LastTimeWeConsideredAddingAStar = gameTimeSeconds;  // Quiz:  Why are we doing this here?
            }
        }



        public static void ConsiderPlayerFiring(double gameTimeSeconds, Input input, ShipBulletListModel shipBulletList, ShipModel ship, GameSounds gameSounds)
        {
            if (!ship.IsDestroyed && input.Fire.JustDown)
            {
                var elapsedTime = gameTimeSeconds - ship.MostRecentPlayerFiringTime;
                if (elapsedTime > ShipModel.MinFiringIntervalForPlayer)
                {
                    shipBulletList.ShipBullets.Add(new ShipBulletModel(ship.CentrePoint.ShuntedBy(ship.Width, 0)));
                    ship.MostRecentPlayerFiringTime = gameTimeSeconds; // Quiz:  Why are we doing this?
                    gameSounds.PlayerFiringSound.Play();
                }
            }
        }



        public static void AddExplosion(double gameTimeSeconds, Point explosionCentre, ExplosionListModel explosionList, GameSounds gameSounds)
        {
            explosionList.Explosions.Add(new ExplosionModel(explosionCentre, gameTimeSeconds));
            gameSounds.ExplosionSound.Play();
        }



        public static void CheckIfAlienShotAndThen(double gameTimeSeconds, AlienListModel alienList, ShipBulletListModel shipBulletsList, Action<AlienModel> andThen)
        {
            var bulletsToRemove = new List<ShipBulletModel>();
            var aliensToRemove = new List<AlienModel>();

            foreach (var bullet in shipBulletsList.ShipBullets)
            {
                foreach (var alien in alienList.Aliens)
                {
                    if (CollisionDetection.Hits(bullet.CentrePoint, alien.CentrePoint, CollisionDetectRange))    // Quiz:  This design means hitting NEAR the centre of the alien!
                    {
                        bulletsToRemove.Add(bullet);
                        aliensToRemove.Add(alien);
                        andThen(alien);
                    }
                }
            }

            alienList.Aliens.RemoveAll(a => aliensToRemove.Contains(a));
            shipBulletsList.ShipBullets.RemoveAll(b => bulletsToRemove.Contains(b));
        }



        public static void CheckIfAlienShot(double gameTimeSeconds, AlienListModel alienList, ShipBulletListModel shipBulletsList, ExplosionListModel explosionsList, ScoreModel Scoring, GameSounds gameSounds)
        {
            CheckIfAlienShotAndThen(
                gameTimeSeconds, alienList, shipBulletsList, alien =>
                {
                    AddExplosion(gameTimeSeconds, alien.CentrePoint, explosionsList, gameSounds);
                    Scoring.SetScore(Scoring.Score + 1);
                });
        }



        public static void CheckIfShipShot(double gameTimeSeconds, ShipModel ship, AlienListModel aliensList, AlienBulletListModel alienBulletList, ExplosionListModel explosions, GameSounds gameSounds)
        {
            if (!ship.IsDestroyed)
            {
                bool shipDestroyed =
                    alienBulletList.AlienBullets.Exists(b => CollisionDetection.Hits(b.CentrePoint, ship.CentrePoint, CollisionDetectRange))
                    || aliensList.Aliens.Exists(a => CollisionDetection.Hits(a.CentrePoint, ship.CentrePoint, CollisionDetectRange));

                if (shipDestroyed)
                {
                    DestroyShip(gameTimeSeconds, ship, explosions, gameSounds);
                }
            }
        }



        public static void DestroyShip(double gameTimeSeconds, ShipModel ship, ExplosionListModel explosions, GameSounds gameSounds)
        {
            ship.IsDestroyed = true;
            ship.TimeOfDestruction = gameTimeSeconds;
            AddExplosion(gameTimeSeconds, ship.CentrePoint, explosions, gameSounds);
        }



        public static void ConsiderRespawningShip(double gameTimeSeconds, ShipModel ship, GameSounds gameSounds)
        {
            if (ship.IsDestroyed)
            {
                double elapsedTime = gameTimeSeconds - ship.TimeOfDestruction;
                if (elapsedTime > ShipModel.RespawnDelay)
                {
                    ship.IsDestroyed = false;
                    gameSounds.ExtraLifeSound.Play();
                }
            }
        }
    }



    public class WorldModel
    {
        private ShipModel Ship = new ShipModel();
        private AlienListModel Aliens = new AlienListModel();
        private AlienBulletListModel AlienBullets = new AlienBulletListModel();
        private ShipBulletListModel ShipBullets = new ShipBulletListModel();
        private StarscapeModel Stars = new StarscapeModel();
        private ExplosionListModel Explosions = new ExplosionListModel();
        private ScoreModel Scoring = new ScoreModel();
        private Random RandomGenerator = new Random(92343467);  // Quiz:  What's this constant for?

        private GameImages Images;
        private GameSounds Sounds;
        private BasicFont YellowFont;
        private MagnificationFactors TextMagnificationLevel = new MagnificationFactors { HorizontalMagnification = 1, VerticalMagnification = 2 };



        public WorldModel(GameImages gameTextures, GameSounds gameSounds, BasicFont yellowFont, int shipWidth, int alienWidth)
        {
            YellowFont = yellowFont;
            Images = gameTextures;
            Sounds = gameSounds;
            Ship.Width = shipWidth;  // Quiz:  Why store this?
            AlienModel.AlienWidth = alienWidth;
        }



        public FrameAdvanceReturnStatus OnFrameAdvance(double gameTimeSeconds, Input input)
        {
            // Hint:  When dealing with the physics, we only reveal the absolutely 
            //        necessary sub-objects to each Physics handler:

            Physics.MoveShipAccordingToKeys(input, Ship);
            Physics.MoveShipBullets(ShipBullets);
            Physics.MoveAlienBullets(AlienBullets);
            Physics.MoveAliens(Aliens);
            Physics.MoveStars(Stars);
            Physics.RemoveOffScreenObjects(Aliens, ShipBullets, AlienBullets, Stars, Scoring);
            Physics.RemoveCompletedExplosions(gameTimeSeconds, Explosions);
            Physics.ConsiderAddingStar(gameTimeSeconds, Stars, RandomGenerator);
            Physics.ConsiderAddingAlien(gameTimeSeconds, Ship, Aliens, RandomGenerator, AlienBullets, Sounds);
            Physics.ConsiderPlayerFiring(gameTimeSeconds, input, ShipBullets, Ship, Sounds);
            Physics.CheckIfAlienShot(gameTimeSeconds, Aliens, ShipBullets, Explosions, Scoring, Sounds);
            Physics.CheckIfShipShot(gameTimeSeconds, Ship, Aliens, AlienBullets, Explosions, Sounds);
            Physics.ConsiderRespawningShip(gameTimeSeconds, Ship, Sounds);

            return FrameAdvanceReturnStatus.ContinueRunning;
        }



        public void OnPaint(double gameTimeSeconds, Renderer renderer)
        {
            Stars.Stars.ForEach(
                star => renderer.DrawTexture1to1Centered(Images.Star, star.CentrePoint));

            if (!Ship.IsDestroyed)
            {
                renderer.DrawTexture1to1Centered(Images.Ship, Ship.CentrePoint);
            }

            Aliens.Aliens.ForEach(
                alien => renderer.DrawTexture1to1Centered(Images.Alien1, alien.CentrePoint));

            AlienBullets.AlienBullets.ForEach(
                bullet => renderer.DrawTexture1to1Centered(Images.EnemyBullet, bullet.CentrePoint));

            ShipBullets.ShipBullets.ForEach(
                bullet => renderer.DrawTexture1to1Centered(Images.PlayerBullet, bullet.CentrePoint));

            Explosions.Explosions.ForEach(
                explosion => renderer.DrawTexture1to1Centered(Images.Explosion, explosion.ExplosionCentre));

            renderer.DrawTextMagnified(
                YellowFont, ScoreModel.ScoreX, ScoreModel.ScoreY, TextAlignH.RightAlign, TextAlignV.MiddleAlign, TextMagnificationLevel, Scoring.ScoreText);
        }



    }
}
