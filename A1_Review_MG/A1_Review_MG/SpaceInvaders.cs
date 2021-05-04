///Author: Vibeesshanan Thevamanoharan
///File Name: A1_Review
///Project Name: SpaceInvaders
///Creation Date: February 5th 2019
///Modified Date: February 24th 2019
///Description: A recreation of the classic arcade game Space Invaders!
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace A1_Review_MG
{
    public class SpaceInvaders : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region GLOBAL VARIABLES

        #region Gamestates

        //3 Different Gamestates
        enum GameState
        {
            gameScreen,
            menuScreen,
            endScreen,
            instructionsScreen,
            instructionsScreen2
        }
        //Sets the current gamestate to the Main Menu
        GameState currentGameState = GameState.menuScreen;
        #endregion

        #region Mouse & Keyboard
        //Keyboard and Mouse
        KeyboardState kb;
        KeyboardState prevKb;
        MouseState mouse;
        MouseState prevMouse;
        #endregion

        #region PlayerInfo
        //Player
        Texture2D player;
        Rectangle playerRec;
        bool isNewGame = true;
        //PlayerHealth
        Rectangle[] healthRec = new Rectangle[4];
        byte health = 3;
        const byte PLAYER_SPEED = 5;

        //Shooting
        Texture2D laser;
        Rectangle laserRec;
        bool isBullet = true;

        //Score
        int score = 0;
        SpriteFont userText;
        SpriteFont endScore;
        Vector2 scoreTextLoc = new Vector2(125, 15);
        Vector2 healthTextLoc = new Vector2(775, 15);
        Vector2 finalScoreLoc = new Vector2(750, 530);
        #endregion

        #region Screens & Buttons
        //Menu screen backgrouns, and buttons
        Texture2D background;
        Rectangle backgroundRec;
        Texture2D background2;
        Rectangle background2Rec;
        Texture2D winScreen;
        Rectangle winScreenRec;
        Texture2D loseScreen;
        Rectangle loseScreenRec;
        Texture2D title;
        Rectangle titleRec;
        Texture2D instructionScreen;
        Rectangle instructionScreenRec;
        Texture2D instructionScreen2;
        Rectangle instructionScreen2Rec;
        //Buttons
        //Main Menu
        Texture2D playButton;
        Rectangle playButtonRec;
        Texture2D exitButton;
        Rectangle exitButtonRec;
        Texture2D instructionsButton;
        Rectangle instructionsButtonRec;
        //Instructions
        Texture2D backButton;
        Texture2D frontButton;
        Rectangle frontButtonRec;
        Rectangle backButtonRec;
        Rectangle backButton2Rec;
        //EndScreen
        Texture2D exitButton2;
        Rectangle exitButtonRec2;
        Texture2D returnButton;
        Rectangle returnButtonRec;

        #endregion

        #region Enemy Variables
        #region Invaders
        //Small enemy Textures,Rectangle, and SourceRecs
        Texture2D smallEnemy;
        Rectangle[] smallEnemyRec = new Rectangle[11];
        Rectangle smallEnemySourceRec;
        //Medium enemy Textures,Rectangle, and SourceRecs
        Texture2D mediumEnemy;
        Rectangle[,] mediumEnemyRec = new Rectangle[11, 2];
        Rectangle mediumEnemySourceRec;
        //Big enemy Textures,Rectangle, and SourceRecs
        Texture2D bigEnemy;
        Rectangle[,] bigEnemyRec = new Rectangle[11, 2];
        Rectangle bigEnemySourceRec;

        //Bullets 
        Texture2D bullet;
        bool[,] bEnemyShot = new bool[11, 2];
        bool[,] mEnemyShot = new bool[11, 2];
        bool[] sEnemyShot = new bool[11];
        Rectangle[,] bulletRecB = new Rectangle[11, 2];
        Rectangle[,] bulletRecM = new Rectangle[11, 2];
        Rectangle[] bulletRecS = new Rectangle[11];
        //Shooting Values
        int shootChance;
        Random shootRandomizer = new Random();
        Random row1Randomizer = new Random();
        Random col1Randomizer = new Random();
        Random row2Randomizer = new Random();
        Random col2Randomizer = new Random();
        Random row3Randomizer = new Random();
        int[] EnemyRow = new int[3];
        int[] EnemyCol = new int[2];
        #endregion

        //Enemy Logics
        const int ENEMY_OFFSET = 50;
        const int ENEMY_WIDTH = 50;
        const int ENEMY_HIEGHT = 30;
        double enemyMoveTimer;
        int moveFreq = 1000;
        int shootFreq = 2000;
        bool moveRight = true;
        int gamesPlayed = 0;
        int enemyCount = 55;
        #endregion

        #region Alien Variables
        //AlienInfo
        Texture2D alienShip;
        Rectangle alienShipRec;
        Random spawner = new Random();
        double spawnerNumber;
        Random alienDirection = new Random();
        int alienPosition;
        bool randomizeMovement = false;
        double spawnerTimer;
        bool isAlienSpawn = true;
        Random scoreRandomizer = new Random();
        int scoreValue;
        #endregion

        #region Barrier Variables
        //5 Different images for the barries
        Texture2D barrier1;
        Texture2D barrier2;
        Texture2D barrier3;
        Texture2D barrier4;
        Texture2D barrier5;
        //2D Arrays for each of the barriers
        Rectangle[,] barrier1Rec = new Rectangle[3, 4];
        Rectangle[,] barrier2Rec = new Rectangle[3, 4];
        Rectangle[,] barrier3Rec = new Rectangle[3, 4];
        Rectangle[,] barrier4Rec = new Rectangle[3, 4];
        //2D Arrays for sourcerec to cycle animations
        Rectangle[,] barrier1SourceRec = new Rectangle[3, 4];
        Rectangle[,] barrier2SourceRec = new Rectangle[3, 4];
        Rectangle[,] barrier3SourceRec = new Rectangle[3, 4];
        Rectangle[,] barrier4SourceRec = new Rectangle[3, 4];
        //2D Arrays for number of times barrier has been hit
        int[,] barHit1 = new int[3, 4];
        int[,] barHit2 = new int[3, 4];
        int[,] barHit3 = new int[3, 4];
        int[,] barHit4 = new int[3, 4];

        const int BARRIER_SIZE = 25;
        #endregion

        #region Borders
        //Constants for Borders around the screen
        const int RIGHT_BORDER = 1300;
        const int LEFT_BORDER = 0;
        const int TOP_BORDER = 0;
        const int BOTTOM_BORDER = 750;
        //Border to check gameloss
        const int BARRIER_BORDER = 625;
        #endregion

        #region Animations
        //Frame aniamtion
        float Elapsed = 0;
        int frames = 0;
        #endregion

        #region Audio
        //Sound Effects and Songs
        SoundEffect userShot;
        SoundEffectInstance userShotInst;
        SoundEffect enemyDeathEffect;
        SoundEffectInstance enemyDeathInst;
        SoundEffect playerDeathEffect;
        SoundEffectInstance playerDeathInst;
        SoundEffect moveEffect;
        SoundEffectInstance moveInst;
        SoundEffect alienDeathEffect;
        SoundEffectInstance alienDeathInst;
        SoundEffect alienMoveEffect;
        SoundEffectInstance alienMoveInst;
        Song backgroundMusic;
        #endregion

        #region FileIO
        //Variables for input and output of highscore
        StreamReader inFile;
        StreamWriter outFile;
        string filePath;
        int highScore;
        Vector2 highScoreLoc = new Vector2(15, 650);
        #endregion

        #endregion

        public SpaceInvaders()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Changes the dimension of the program
            graphics.PreferredBackBufferHeight = 750;
            graphics.PreferredBackBufferWidth = 1300;
            //Makes the mouse visible in the game
            IsMouseVisible = true;

            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Width and Height variables for display
            int width = (this.graphics.GraphicsDevice.Viewport.Width);
            int height = (this.graphics.GraphicsDevice.Viewport.Height);

            //Load content for all non gameplay images
            #region Images
            //Loads the different screens: Menu, Main, Instruct, LeaderBoards
            background = Content.Load<Texture2D>(@"Backgrounds\SpaceBackground");
            backgroundRec = new Rectangle(0, 0, (int)(width), (int)(height));
            background2 = Content.Load<Texture2D>(@"Backgrounds\SpaceBackground");
            background2Rec = new Rectangle(0, -750, (int)(width), (int)(height));
            winScreen = Content.Load<Texture2D>(@"Backgrounds\gameOverWin");
            winScreenRec = new Rectangle(0, 0, (int)(width), (int)(height));
            loseScreen = Content.Load<Texture2D>(@"Backgrounds\gameOverLose");
            loseScreenRec = new Rectangle(0, 0, (int)(width), (int)(height));
            title = Content.Load<Texture2D>(@"Images\SpaceInvadersTlt");
            titleRec = new Rectangle(325, 25, (int)(width * 0.5), (int)(height * 0.33));
            instructionScreen = Content.Load<Texture2D>(@"Backgrounds\instructionsScreen1");
            instructionScreenRec = new Rectangle(0, 0, (int)(width), (int)(height));
            instructionScreen2 = Content.Load<Texture2D>(@"Backgrounds\instructionsScreen2");
            instructionScreen2Rec = new Rectangle(0, 0, (int)(width), (int)(height));
            //Buttons
            playButton = Content.Load<Texture2D>(@"Images\PlayButton");
            playButtonRec = new Rectangle(488, 350, (int)(width * 0.25), (int)(height * 0.125));
            exitButton = Content.Load<Texture2D>(@"Images\ExitButton");
            exitButtonRec = new Rectangle(491, 625, (int)(width * 0.25), (int)(height * 0.14));
            exitButton2 = Content.Load<Texture2D>(@"Images\ExitButton2");
            exitButtonRec2 = new Rectangle(975, 635, (int)(width * 0.20), (int)(height * 0.10));
            returnButton = Content.Load<Texture2D>(@"Images\returnButton");
            returnButtonRec = new Rectangle(75, 625, (int)(width * 0.25), (int)(height * 0.14));
            instructionsButton = Content.Load<Texture2D>(@"Images\instructionsButton");
            instructionsButtonRec = new Rectangle(355, 475, (int)(width * 0.45), (int)(height * 0.175));
            backButton = Content.Load<Texture2D>(@"Images\greenArrow");
            backButtonRec = new Rectangle(40, 30, (int)(width * 0.09), (int)(height * 0.15));
            backButton2Rec = new Rectangle(40, 30, (int)(width * 0.09), (int)(height * 0.15));
            frontButton = Content.Load<Texture2D>(@"Images\frontArrow");
            frontButtonRec = new Rectangle(1140, 30, (int)(width * 0.09), (int)(height * 0.15));

            #endregion

            //Load content for all the player related graphics
            #region Player
            player = Content.Load<Texture2D>(@"Images\SIShip");
            playerRec = new Rectangle(630, (BOTTOM_BORDER - 50), (int)(width * 0.05), (int)(height * 0.065));
            healthRec[0] = new Rectangle(975, 5, (int)(width * 0.05), (int)(height * 0.065));
            healthRec[1] = new Rectangle(1050, 5, (int)(width * 0.05), (int)(height * 0.065));
            healthRec[2] = new Rectangle(1125, 5, (int)(width * 0.05), (int)(height * 0.065));
            healthRec[3] = new Rectangle(975, 5, (int)(width * 0.05), (int)(height * 0.065));
            laser = Content.Load<Texture2D>(@"Images\Laser");
            laserRec = new Rectangle((playerRec.X + (playerRec.Width / 2)) - 2, (playerRec.Y + player.Height), (int)(width * 0.0055), (int)(height * 0.025));
            #endregion

            //Loads all audio, such as sound effects, and songs
            #region AudioLoadContent
            userShot = Content.Load<SoundEffect>(@"Audio\shoot");
            userShotInst = userShot.CreateInstance();

            enemyDeathEffect = Content.Load<SoundEffect>(@"Audio\InvaderKilled");
            enemyDeathInst = enemyDeathEffect.CreateInstance();

            playerDeathEffect = Content.Load<SoundEffect>(@"Audio\explosion");
            playerDeathInst = playerDeathEffect.CreateInstance();

            moveEffect = Content.Load<SoundEffect>(@"Audio\fastinvader1");
            moveInst = moveEffect.CreateInstance();

            alienDeathEffect = Content.Load<SoundEffect>(@"Audio\ufo_highpitch");
            alienDeathInst = alienDeathEffect.CreateInstance();

            alienMoveEffect = Content.Load<SoundEffect>(@"Audio\ufo_lowpitch");
            alienMoveInst = alienMoveEffect.CreateInstance();

            backgroundMusic = Content.Load<Song>(@"Audio\SpaceInvadersMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            #endregion

            //Loads all the enemy textures and rectabgles
            #region Enemies
            //Load content and loop for small enemies
            smallEnemy = Content.Load<Texture2D>(@"Sprites\smallInvader");
            smallEnemySourceRec = new Rectangle(0, 0, (smallEnemy.Width / 2), (smallEnemy.Height));
            for (int s = 0; s < smallEnemyRec.Length; ++s)
            {
                smallEnemyRec[s] = new Rectangle(325 + (ENEMY_OFFSET * (s - 1)), 125, 40, 30);
                bulletRecS[s] = new Rectangle(smallEnemyRec[s].X, smallEnemyRec[s].Y, 50, 50);
            }

            //Load content and loop for medium enemies
            mediumEnemy = Content.Load<Texture2D>(@"Sprites\mediumInvader");
            mediumEnemySourceRec = new Rectangle(0, 0, (mediumEnemy.Width / 2), (mediumEnemy.Height));
            for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
            {
                for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                {
                    mediumEnemyRec[medR, medC] = new Rectangle(325 + (ENEMY_OFFSET * (medR - 1)), (175 + (ENEMY_OFFSET * medC - 1)), 40, 30);
                    bulletRecM[medR, medC] = new Rectangle(mediumEnemyRec[medR, medC].X, mediumEnemyRec[medR, medC].Y, 50, 50);
                }
            }

            //Load content and loop for big enemies
            bigEnemy = Content.Load<Texture2D>(@"Sprites\bigInvader");
            bigEnemySourceRec = new Rectangle(0, 0, (bigEnemy.Width / 2), (bigEnemy.Height));
            for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
            {
                for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                {
                    bigEnemyRec[bigR, bigC] = new Rectangle(325 + (ENEMY_OFFSET * (bigR - 1)), (275 + (ENEMY_OFFSET * bigC - 1)), 40, 30);
                    bulletRecB[bigR, bigC] = new Rectangle(bigEnemyRec[bigR, bigC].X, bigEnemyRec[bigR, bigC].Y, 50, 50);
                }
            }
            bullet = Content.Load<Texture2D>(@"Images/fastBullet");

            //Load content for the alien spaceship
            const int ALIEN_Y_LOC = 75;
            const int ALIEN_X_LOC = -100;
            const double ALIEN_SCALE = 0.06;
            alienShipRec = new Rectangle(ALIEN_X_LOC, ALIEN_Y_LOC, (int)(width * ALIEN_SCALE), (int)(height * ALIEN_SCALE));
            alienShip = Content.Load<Texture2D>(@"Images\alienShip");
            #endregion

            //Loads the textures and the rectangles for the barries
            #region Barriers
            //Textures
            barrier1 = Content.Load<Texture2D>(@"Sprites\Barrier1");
            barrier2 = Content.Load<Texture2D>(@"Sprites\Barrier2");
            barrier3 = Content.Load<Texture2D>(@"Sprites\Barrier3");
            barrier4 = Content.Load<Texture2D>(@"Sprites\Barrier4");
            barrier5 = Content.Load<Texture2D>(@"Sprites\Barrier5");

            //Rectangles
            const int BARRIER_Y = 550;
            const int BARRIER_OFFSET = 25;

            #region LoadBarrier#1
            barrier1Rec[0, 0] = new Rectangle(150, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[0, 1] = new Rectangle(175, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[0, 2] = new Rectangle(200, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[0, 3] = new Rectangle(225, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[1, 0] = new Rectangle(150, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[1, 1] = new Rectangle(175, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[1, 2] = new Rectangle(200, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[1, 3] = new Rectangle(225, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[2, 0] = new Rectangle(150, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[2, 1] = new Rectangle(175, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[2, 2] = new Rectangle(200, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier1Rec[2, 3] = new Rectangle(225, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            #endregion

            #region LoadBarrier#2
            barrier2Rec[0, 0] = new Rectangle(450, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[0, 1] = new Rectangle(475, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[0, 2] = new Rectangle(500, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[0, 3] = new Rectangle(525, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[1, 0] = new Rectangle(450, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[1, 1] = new Rectangle(475, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[1, 2] = new Rectangle(500, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[1, 3] = new Rectangle(525, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[2, 0] = new Rectangle(450, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[2, 1] = new Rectangle(475, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[2, 2] = new Rectangle(500, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier2Rec[2, 3] = new Rectangle(525, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            #endregion

            #region LoadBarrier#3
            barrier3Rec[0, 0] = new Rectangle(750, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[0, 1] = new Rectangle(775, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[0, 2] = new Rectangle(800, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[0, 3] = new Rectangle(825, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[1, 0] = new Rectangle(750, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[1, 1] = new Rectangle(775, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[1, 2] = new Rectangle(800, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[1, 3] = new Rectangle(825, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[2, 0] = new Rectangle(750, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[2, 1] = new Rectangle(775, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[2, 2] = new Rectangle(800, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier3Rec[2, 3] = new Rectangle(825, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            #endregion

            #region LoadBarrier#4
            barrier4Rec[0, 0] = new Rectangle(1050, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[0, 1] = new Rectangle(1075, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[0, 2] = new Rectangle(1100, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[0, 3] = new Rectangle(1125, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[1, 0] = new Rectangle(1050, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[1, 1] = new Rectangle(1075, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[1, 2] = new Rectangle(1100, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[1, 3] = new Rectangle(1125, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[2, 0] = new Rectangle(1050, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[2, 1] = new Rectangle(1075, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[2, 2] = new Rectangle(1100, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            barrier4Rec[2, 3] = new Rectangle(1125, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
            #endregion

            #endregion

            userText = Content.Load<SpriteFont>(@"Fonts\scoretext");
            endScore = Content.Load<SpriteFont>(@"Fonts\endFont");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //FileInput Subprogram
            FILEI();

            #region Keyboard and Mouse Update
            //Gets the state of the keyboard
            prevKb = kb;
            kb = Keyboard.GetState();
            //Gets the state of the mouse
            prevMouse = mouse;
            mouse = Mouse.GetState();
            #endregion

            #region PlayerTesting
            if (kb.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            if (kb.IsKeyDown(Keys.E))
            {
                currentGameState = GameState.endScreen;
            }
            if (kb.IsKeyDown(Keys.Enter))
            {
                currentGameState = GameState.menuScreen;
                isNewGame = true;
            }

            #endregion

            switch (currentGameState)
            {
                //All update logic for:

                //Menu - Buttons
                #region MenuScreen
                case GameState.menuScreen:
                    MainMenuButtons();
                    //Resets the background
                    backgroundRec.Y = 0;
                    background2Rec.Y = -750;
                    break;
                #endregion;

                //Gamescreen - All gameplay
                #region Gameplay
                case GameState.gameScreen:
                    PlayerControls();
                    Health();
                    Shooting();
                    Alien(gameTime);
                    EnemyMovement(gameTime);
                    Animation(gameTime);
                    Barriers();
                    EnemyShooting();
                    break;
                #endregion

                //Intructions - Buttons
                #region Instructions
                case GameState.instructionsScreen:
                    //Back Button
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > backButtonRec.X && mouse.X < (backButtonRec.X + backButtonRec.Width)
                              && mouse.Y > backButtonRec.Y && mouse.Y < (backButtonRec.Y + backButtonRec.Height))
                    {
                        currentGameState = GameState.menuScreen;
                    }
                    //Forward Button
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > frontButtonRec.X && mouse.X < (frontButtonRec.X + frontButtonRec.Width)
                              && mouse.Y > frontButtonRec.Y && mouse.Y < (frontButtonRec.Y + frontButtonRec.Height))
                    {
                        currentGameState = GameState.instructionsScreen2;
                    }
                    break;
                #endregion
                #region Instructions2
                case GameState.instructionsScreen2:
                    //Back Button
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > backButton2Rec.X && mouse.X < (backButton2Rec.X + backButton2Rec.Width)
                              && mouse.Y > backButton2Rec.Y && mouse.Y < (backButton2Rec.Y + backButton2Rec.Height))
                    {
                        currentGameState = GameState.instructionsScreen;
                    }
                    break;
                #endregion

                //Endgame - Buttons
                #region Endscreen
                case GameState.endScreen:

                    //Exit Button
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > exitButtonRec2.X && mouse.X < (exitButtonRec2.X + exitButtonRec2.Width)
                              && mouse.Y > exitButtonRec2.Y && mouse.Y < (exitButtonRec2.Y + exitButtonRec2.Height))
                    {
                        //Allows player to exit the game
                        this.Exit();
                    }
                    //Return button to change back to menuscreen
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > returnButtonRec.X && mouse.X < (returnButtonRec.X + returnButtonRec.Width)
                              && mouse.Y > returnButtonRec.Y && mouse.Y < (returnButtonRec.Y + returnButtonRec.Height))
                    {
                        currentGameState = GameState.menuScreen;
                        isNewGame = true;
                    }
                    break;
                    #endregion
            }

            //All logics for starting a new game
            NewGame();

            //FileOutput Subprogram
            FILEO();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (currentGameState)
            {
                //Everything drawn on the menuscreen 
                #region MenuScreen
                case GameState.menuScreen:
                    //Draws the image for the main menu: Title, and Bear Logo
                    spriteBatch.Draw(background, backgroundRec, Color.White);
                    spriteBatch.Draw(title, titleRec, Color.White);
                    spriteBatch.Draw(playButton, playButtonRec, Color.White);
                    spriteBatch.Draw(exitButton, exitButtonRec, Color.White);
                    spriteBatch.Draw(instructionsButton, instructionsButtonRec, Color.White);
                    spriteBatch.DrawString(userText, "Current Highscore:\n               " + highScore, highScoreLoc, Color.LimeGreen);
                    break;
                #endregion;

                //Everything drawn on the gamescreen
                #region GameScreen
                case GameState.gameScreen:
                    //Backgrounds
                    spriteBatch.Draw(background, backgroundRec, Color.White);
                    spriteBatch.Draw(background2, background2Rec, Color.White);
                    //Playership, Health and Bullets
                    spriteBatch.Draw(laser, laserRec, Color.White);
                    spriteBatch.Draw(player, playerRec, Color.White);
                    for (int h = 0; h < healthRec.Length; h++)
                    {
                        spriteBatch.Draw(player, healthRec[h], Color.White);
                    }

                    //For loops to draw all the enemies in order
                    #region LoopEnemyDraw+++

                    for (int s = 0; s < smallEnemyRec.Length; s++)
                    {
                        spriteBatch.Draw(bullet, bulletRecS[s], Color.White);
                        spriteBatch.Draw(smallEnemy, smallEnemyRec[s], smallEnemySourceRec, Color.White);
                    }

                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            spriteBatch.Draw(bullet, bulletRecM[medR, medC], Color.White);
                            spriteBatch.Draw(mediumEnemy, mediumEnemyRec[medR, medC], mediumEnemySourceRec, Color.White);
                        }
                    }

                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            spriteBatch.Draw(bullet, bulletRecB[bigR, bigC], Color.White);
                            spriteBatch.Draw(bigEnemy, bigEnemyRec[bigR, bigC], bigEnemySourceRec, Color.White);
                        }
                    }

                    spriteBatch.Draw(alienShip, alienShipRec, Color.White);
                    #endregion

                    //Draws all 4 barriers
                    #region BarriersDraw
                    //Draw the first barrier
                    #region Barrier1
                    spriteBatch.Draw(barrier4, barrier1Rec[0, 0], barrier1SourceRec[0, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[0, 1], barrier1SourceRec[0, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[0, 2], barrier1SourceRec[0, 2], Color.White);
                    spriteBatch.Draw(barrier5, barrier1Rec[0, 3], barrier1SourceRec[0, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[1, 0], barrier1SourceRec[1, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[1, 1], barrier1SourceRec[1, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[1, 2], barrier1SourceRec[1, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[1, 3], barrier1SourceRec[1, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[2, 0], barrier1SourceRec[2, 0], Color.White);
                    spriteBatch.Draw(barrier3, barrier1Rec[2, 1], barrier1SourceRec[2, 1], Color.White);
                    spriteBatch.Draw(barrier2, barrier1Rec[2, 2], barrier1SourceRec[2, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier1Rec[2, 3], barrier1SourceRec[2, 3], Color.White);
                    #endregion
                    //Draw the second barrier
                    #region Barrier2
                    spriteBatch.Draw(barrier4, barrier2Rec[0, 0], barrier2SourceRec[0, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[0, 1], barrier2SourceRec[0, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[0, 2], barrier2SourceRec[0, 2], Color.White);
                    spriteBatch.Draw(barrier5, barrier2Rec[0, 3], barrier2SourceRec[0, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[1, 0], barrier2SourceRec[1, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[1, 1], barrier2SourceRec[1, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[1, 2], barrier2SourceRec[1, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[1, 3], barrier2SourceRec[1, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[2, 0], barrier2SourceRec[2, 0], Color.White);
                    spriteBatch.Draw(barrier3, barrier2Rec[2, 1], barrier2SourceRec[2, 1], Color.White);
                    spriteBatch.Draw(barrier2, barrier2Rec[2, 2], barrier2SourceRec[2, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier2Rec[2, 3], barrier2SourceRec[2, 3], Color.White);
                    #endregion
                    //Draw the third barrier
                    #region Barrier3
                    spriteBatch.Draw(barrier4, barrier3Rec[0, 0], barrier3SourceRec[0, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[0, 1], barrier3SourceRec[0, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[0, 2], barrier3SourceRec[0, 2], Color.White);
                    spriteBatch.Draw(barrier5, barrier3Rec[0, 3], barrier3SourceRec[0, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[1, 0], barrier3SourceRec[1, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[1, 1], barrier3SourceRec[1, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[1, 2], barrier3SourceRec[1, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[1, 3], barrier3SourceRec[1, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[2, 0], barrier3SourceRec[2, 0], Color.White);
                    spriteBatch.Draw(barrier3, barrier3Rec[2, 1], barrier3SourceRec[2, 1], Color.White);
                    spriteBatch.Draw(barrier2, barrier3Rec[2, 2], barrier3SourceRec[2, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier3Rec[2, 3], barrier3SourceRec[2, 3], Color.White);
                    #endregion
                    //Draw the fourth barrier
                    #region Barrier4
                    spriteBatch.Draw(barrier4, barrier4Rec[0, 0], barrier4SourceRec[0, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[0, 1], barrier4SourceRec[0, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[0, 2], barrier4SourceRec[0, 2], Color.White);
                    spriteBatch.Draw(barrier5, barrier4Rec[0, 3], barrier4SourceRec[0, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[1, 0], barrier4SourceRec[1, 0], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[1, 1], barrier4SourceRec[1, 1], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[1, 2], barrier4SourceRec[1, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[1, 3], barrier4SourceRec[1, 3], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[2, 0], barrier4SourceRec[2, 0], Color.White);
                    spriteBatch.Draw(barrier3, barrier4Rec[2, 1], barrier4SourceRec[2, 1], Color.White);
                    spriteBatch.Draw(barrier2, barrier4Rec[2, 2], barrier4SourceRec[2, 2], Color.White);
                    spriteBatch.Draw(barrier1, barrier4Rec[2, 3], barrier4SourceRec[2, 3], Color.White);
                    #endregion
                    #endregion

                    spriteBatch.DrawString(userText, "Score: " + score, scoreTextLoc, Color.LimeGreen);
                    spriteBatch.DrawString(userText, "Health: ", healthTextLoc, Color.LimeGreen);

                    break;
                #endregion

                //Everything drawn on the first intruction screen
                #region InstructionScreen

                case GameState.instructionsScreen:
                    spriteBatch.Draw(instructionScreen, instructionScreenRec, Color.White);
                    spriteBatch.Draw(backButton, backButtonRec, Color.White);
                    spriteBatch.Draw(frontButton, frontButtonRec, Color.White);

                    break;
                #endregion

                //Everything drawn on the second intruction screen
                #region InstructionScreen2

                case GameState.instructionsScreen2:
                    spriteBatch.Draw(instructionScreen2, instructionScreen2Rec, Color.White);
                    spriteBatch.Draw(backButton, backButton2Rec, Color.White);
                    break;
                #endregion

                //Everything drawn on the endscreen
                #region EndScreen
                case GameState.endScreen:
                    if (score >= highScore)
                    {
                        spriteBatch.Draw(winScreen, winScreenRec, Color.White);
                    }

                    if (score < highScore)
                    {
                        spriteBatch.Draw(loseScreen, loseScreenRec, Color.White);
                    }
                    spriteBatch.Draw(exitButton2, exitButtonRec2, Color.White);
                    spriteBatch.Draw(returnButton, returnButtonRec, Color.White);
                    spriteBatch.DrawString(endScore, "" + score + "", finalScoreLoc, Color.LimeGreen);
                    break;
                    #endregion
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Collision Detection for the buttons on the Main Screen
        private void MainMenuButtons()
        {
            //Play Button
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > playButtonRec.X && mouse.X < (playButtonRec.X + playButtonRec.Width)
                      && mouse.Y > playButtonRec.Y && mouse.Y < (playButtonRec.Y + playButtonRec.Height))
            {
                //Switches to the gameplay screen
                currentGameState = GameState.gameScreen;
                isNewGame = false;
            }
            //Instructions Button
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > instructionsButtonRec.X && mouse.X < (instructionsButtonRec.X + instructionsButtonRec.Width)
                      && mouse.Y > instructionsButtonRec.Y && mouse.Y < (instructionsButtonRec.Y + instructionsButtonRec.Height))
            {
                //Switches to the instructions screen
                currentGameState = GameState.instructionsScreen;
            }
            //Exit Button
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && mouse.X > exitButtonRec.X && mouse.X < (exitButtonRec.X + exitButtonRec.Width)
                      && mouse.Y > exitButtonRec.Y && mouse.Y < (exitButtonRec.Y + exitButtonRec.Height))
            {
                //Allows player to exit the game
                this.Exit();
            }
        }

        //Player/Background Movement and increasing game difficulty
        private void PlayerControls()
        {
            #region Player
            //Allows the player to move right
            if (kb.IsKeyDown(Keys.Right) || (kb.IsKeyDown(Keys.D)))
            {
                playerRec.X = playerRec.X + PLAYER_SPEED;
            }
            //Allows the player to move left
            else if (kb.IsKeyDown(Keys.Left) || (kb.IsKeyDown(Keys.A)))
            {
                playerRec.X = playerRec.X - PLAYER_SPEED;

            }

            //Allows the player to not pass the left border of the screen
            if (playerRec.X <= LEFT_BORDER)
            {
                playerRec.X = LEFT_BORDER;
            }
            //Allows the player to not pass the right border of the screen
            if ((playerRec.X + playerRec.Width) >= RIGHT_BORDER)
            {
                playerRec.X = (RIGHT_BORDER - playerRec.Width);
            }
            #endregion            

            #region Background
            //Creates a moving background
            const int BACKGROUND_SPEED = 5;

            backgroundRec.Y = backgroundRec.Y + BACKGROUND_SPEED;
            background2Rec.Y = background2Rec.Y + BACKGROUND_SPEED;
            //If the background hits the bottom border it resets
            if (backgroundRec.Y >= BOTTOM_BORDER)
            {
                backgroundRec.Y = -BOTTOM_BORDER;

            }
            else if (background2Rec.Y >= BOTTOM_BORDER)
            {
                background2Rec.Y = -BOTTOM_BORDER;

            }
            #endregion

            #region Game Difficulty/Endgame
            //As the number of enemies deplete, shooting rate, and movement rate increase
            if (enemyCount == 44)
            {
                moveFreq = 850;
                shootFreq = 1750;
            }
            if (enemyCount == 33)
            {
                moveFreq = 700;
                shootFreq = 1500;
            }
            if (enemyCount == 22)
            {
                moveFreq = 550;
                shootFreq = 1000;
            }
            if (enemyCount == 11)
            {
                moveFreq = 400;
                shootFreq = 500;
            }
            if (enemyCount == 5)
            {
                moveFreq = 250;
                shootFreq = 100;
            }
            //When enemies all die. reset all enemy rectangles, 1 level lower than starting
            if (enemyCount == 0)
            {
                moveFreq = 1000;
                shootFreq = 2000;
                gamesPlayed++;
                for (int s = 0; s < smallEnemyRec.Length; ++s)
                {
                    smallEnemyRec[s].X = (325 + (ENEMY_OFFSET * (s - 1)));
                    smallEnemyRec[s].Y = 125 + (ENEMY_OFFSET * gamesPlayed);
                }
                for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                {
                    for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                    {
                        mediumEnemyRec[medR, medC].X = (325 + (ENEMY_OFFSET * (medR - 1)));
                        mediumEnemyRec[medR, 0].Y = (175 + (ENEMY_OFFSET * gamesPlayed));
                        mediumEnemyRec[medR, 1].Y = (175 + (ENEMY_OFFSET * (gamesPlayed + 1)));
                    }
                }
                for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                {
                    for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                    {
                        bigEnemyRec[bigR, bigC].X = (325 + (ENEMY_OFFSET * (bigR - 1)));
                        bigEnemyRec[bigR, 0].Y = 225 + (ENEMY_OFFSET * (gamesPlayed + 1));
                        bigEnemyRec[bigR, 1].Y = 225 + (ENEMY_OFFSET * (gamesPlayed + 2));
                    }
                }
                //Add health aswell
                health++;
                moveRight = true;
                enemyCount = 55;
            }
            #endregion
        }

        //Health system to determine win or lose in the game
        private void Health()
        {
            //If health is full all ships are active
            if (health == 3)
            {
                healthRec[0].X = 975;
                healthRec[1].X = 1050;
                healthRec[2].X = 1125;
                healthRec[3].X = 975;
            }
            //Eachtime a health is lost 1 ship will deplete
            if (health == 2)
            {
                healthRec[0].X = 975;
                healthRec[1].X = 1050;
                healthRec[2].X = 975;
                healthRec[3].X = 975;
            }
            if (health == 1)
            {
                healthRec[0].X = 975;
                healthRec[1].X = 975;
                healthRec[2].X = 975;
                healthRec[3].X = 975;
            }
            //If extra life is gained add a life
            if (health >= 4)
            {
                health = 4;
                healthRec[0].X = 975;
                healthRec[1].X = 1050;
                healthRec[2].X = 1125;
                healthRec[3].X = 1200;
            }
            //Lose the game if health reaches 0
            if (health == 0)
            {
                currentGameState = GameState.endScreen;
            }
        }

        //All PLAYER shooting logic, and collision with Invaders/Aliens
        private void Shooting()
        {
            int bulletSpeed = 15;
            #region Shooting Logic
            //If a bullet is available it will be hidden behind the player
            if (isBullet == true)
            {
                laserRec.X = (playerRec.X + (playerRec.Width / 2));
                laserRec.Y = (playerRec.Y);
            }
            //When space bar is pressed a bullet is shot
            if (kb.IsKeyDown(Keys.Space) && (isBullet == true))
            {
                isBullet = false;
                userShotInst.Play();

            }
            //The bullet begins to travel up
            if (isBullet == false)
            {
                laserRec.Y = laserRec.Y - bulletSpeed;

            }
            //When the bullet hits the top of the screen 
            if (laserRec.Y <= 0)
            {
                isBullet = true;
            }
            #endregion

            #region EnemyCollision
            //Collision detection for bullet -> small enemy
            for (int s = 0; s < smallEnemyRec.Length; ++s)
            {
                if (laserRec.X <= (smallEnemyRec[s].X + smallEnemyRec[s].Width) && (laserRec.X + laserRec.Width) >= smallEnemyRec[s].X
                    && (laserRec.Y <= (smallEnemyRec[s].Y + smallEnemyRec[s].Height) && (laserRec.Y + laserRec.Height) >= smallEnemyRec[s].Y))
                {
                    smallEnemyRec[s].X = -10000;
                    smallEnemyRec[s].Y = -10000;
                    enemyDeathInst.Play();
                    score = score + 30;
                    enemyCount--;
                    isBullet = true;
                }
            }
            //Collision detection for bullet -> medium enemy
            for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
            {
                for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                {
                    if (laserRec.X <= (mediumEnemyRec[medR, medC].X + mediumEnemyRec[medR, medC].Width) && (laserRec.X + laserRec.Width) >= mediumEnemyRec[medR, medC].X
                    && (laserRec.Y <= (mediumEnemyRec[medR, medC].Y + mediumEnemyRec[medR, medC].Height) && (laserRec.Y + laserRec.Height) >= mediumEnemyRec[medR, medC].Y))
                    {
                        mediumEnemyRec[medR, medC].X = -10000;
                        mediumEnemyRec[medR, medC].Y = -10000;
                        enemyDeathInst.Play();
                        score = score + 20;
                        enemyCount--;
                        isBullet = true;
                    }
                }
            }
            //Collision detection for bullet -> big enemy
            for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
            {
                for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                {
                    if (laserRec.X <= (bigEnemyRec[bigR, bigC].X + bigEnemyRec[bigR, bigC].Width) && (laserRec.X + laserRec.Width) >= bigEnemyRec[bigR, bigC].X
                    && (laserRec.Y <= (bigEnemyRec[bigR, bigC].Y + bigEnemyRec[bigR, bigC].Height) && (laserRec.Y + laserRec.Height) >= bigEnemyRec[bigR, bigC].Y))
                    {
                        bigEnemyRec[bigR, bigC].X = -10000;
                        bigEnemyRec[bigR, bigC].Y = -10000;
                        enemyDeathInst.Play();
                        score = score + 10;
                        enemyCount--;
                        isBullet = true;
                    }
                }
            }
            #endregion

            #region AlienCollision

            int alienScore = 50;
            if (laserRec.X <= (alienShipRec.X + alienShipRec.Width) && (laserRec.X + laserRec.Width) >= alienShipRec.X
                   && (laserRec.Y <= (alienShipRec.Y + alienShipRec.Height) && (laserRec.Y + laserRec.Height) >= alienShipRec.Y))
            {
                alienDeathInst.Play();
                isBullet = true;
                alienShipRec.X = LEFT_BORDER - alienShipRec.Width;
                scoreValue = scoreRandomizer.Next(1, 6);

                if (scoreValue == 1)
                {
                    score = score + alienScore;
                }
                else if (scoreValue == 2)
                {
                    score = score + (alienScore * 2);
                }
                else if (scoreValue == 3)
                {
                    score = score + (alienScore * 3);
                }
                else if (scoreValue == 4)
                {
                    score = score + (alienScore * 4);
                }
                else if (scoreValue == 5)
                {
                    score = score + (alienScore * 5);
                }

                alienPosition = 0;
            }

            #endregion
        }

        //Alien Movement and score
        private void Alien(GameTime gameTime)
        {
            //Checks whether or not the alien is able to be spawned
            if (alienPosition == 0)
            {
                isAlienSpawn = true;
            }

            if (isAlienSpawn == true && alienPosition == 0)
            {
                //Creates a random number between 3 and 20 secs
                spawnerNumber = spawner.Next(3, 21);
                alienPosition = 5;
                isAlienSpawn = false;
            }

            //A timer begins from the point the randomizer is created
            spawnerTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //When the timer reaches the value of the random number
            if (spawnerTimer >= spawnerNumber)
            {
                randomizeMovement = true;
            }
            //The aliens position is randomized (left or right)
            if (randomizeMovement == true)
            {
                alienPosition = alienDirection.Next(1, 3);
                spawnerTimer = 0;
                randomizeMovement = false;
            }
            //The next 4 if statments randomize the location and the movement
            if (alienPosition == 1)
            {
                alienShipRec.X = LEFT_BORDER - alienShipRec.Width;
                alienPosition = 3;
            }

            else if (alienPosition == 2)
            {
                alienShipRec.X = RIGHT_BORDER;
                alienPosition = 4;
            }

            if (alienPosition == 3)
            {
                spawnerTimer = 0;
                alienShipRec.X = alienShipRec.X + 3;
                alienMoveInst.Play();
            }

            else if (alienPosition == 4)
            {
                spawnerTimer = 0;
                alienShipRec.X = alienShipRec.X - 3;
                alienMoveInst.Play();
            }
            //When the alien passes by the screen it is back to respawning
            if (alienShipRec.X >= RIGHT_BORDER && alienPosition == 3)
            {
                alienPosition = 0;
            }

            else if (alienShipRec.X <= (LEFT_BORDER - alienShipRec.Width) && alienPosition == 4)
            {
                alienPosition = 0;
            }
        }

        //Invaders Movement as time progresses through the game
        private void EnemyMovement(GameTime gameTime)
        {
            const int ENEMY_SPEED = 25;
            //Timer for how often enemies should move
            enemyMoveTimer += Math.Round(gameTime.ElapsedGameTime.TotalMilliseconds, 0);

            //All enemy right movement
            #region RightMovement
            //All right movement for enemies
            if (moveRight == true)
            {

                //When the timer reaches 1secs the following occurs
                if (enemyMoveTimer >= moveFreq)
                {
                    #region SmallRMove
                    for (int s = 0; s < smallEnemyRec.Length; ++s)
                    {
                        smallEnemyRec[s].X = smallEnemyRec[s].X + ENEMY_SPEED;
                        if (smallEnemyRec[s].X >= RIGHT_BORDER - smallEnemyRec[s].Width)
                        {
                            for (int ss = 0; ss < smallEnemyRec.Length; ++ss)
                            {
                                smallEnemyRec[ss].Y = smallEnemyRec[ss].Y + ENEMY_SPEED;
                                moveRight = false;
                            }
                        }
                    }
                    #endregion

                    #region MediumedRMove
                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            mediumEnemyRec[medR, medC].X = mediumEnemyRec[medR, medC].X + ENEMY_SPEED;
                            if (mediumEnemyRec[medR, medC].X >= RIGHT_BORDER - mediumEnemyRec[medR, medC].Width || smallEnemyRec[medR].X >= RIGHT_BORDER - smallEnemyRec[medR].Width)
                            {
                                for (int medRr = 0; medRr < mediumEnemyRec.GetLength(0); ++medRr)
                                {
                                    mediumEnemyRec[medRr, medC].Y = mediumEnemyRec[medRr, medC].Y + ENEMY_SPEED;
                                    moveRight = false;
                                }
                            }
                        }
                    }
                    #endregion

                    #region BigRMove
                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            bigEnemyRec[bigR, bigC].X = bigEnemyRec[bigR, bigC].X + ENEMY_SPEED;
                            if (bigEnemyRec[bigR, bigC].X >= RIGHT_BORDER - bigEnemyRec[bigR, bigC].Width || mediumEnemyRec[bigR, bigC].X >= RIGHT_BORDER - mediumEnemyRec[bigR, bigC].Width
                                || smallEnemyRec[bigR].X >= RIGHT_BORDER - smallEnemyRec[bigR].Width)
                            {
                                for (int bigRr = 0; bigRr < bigEnemyRec.GetLength(0); ++bigRr)
                                {
                                    bigEnemyRec[bigRr, bigC].Y = bigEnemyRec[bigRr, bigC].Y + ENEMY_SPEED;
                                    moveRight = false;
                                }
                            }
                        }
                    }



                    #endregion
                    moveInst.Play();
                    enemyMoveTimer = 0;
                }
            }
            #endregion

            //All enemy left movement
            #region LeftMovement
            //All left movement for enemies
            if (moveRight == false)
            {
                //When the timer reaches 1secs the following occurs
                if (enemyMoveTimer >= moveFreq)
                {
                    #region SmallLMove
                    for (int s = 0; s < smallEnemyRec.Length; ++s)
                    {
                        smallEnemyRec[s].X = smallEnemyRec[s].X - ENEMY_SPEED;
                        if (smallEnemyRec[s].X == LEFT_BORDER)
                        {
                            for (int ss = 0; ss < smallEnemyRec.Length; ++ss)
                            {
                                smallEnemyRec[ss].Y = smallEnemyRec[ss].Y + ENEMY_SPEED;
                                moveRight = true;
                            }
                        }

                    }
                    #endregion

                    #region MediumLMove
                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            mediumEnemyRec[medR, medC].X = mediumEnemyRec[medR, medC].X - ENEMY_SPEED;
                            if (mediumEnemyRec[medR, medC].X == LEFT_BORDER || smallEnemyRec[medR].X == LEFT_BORDER)
                            {
                                for (int medRr = 0; medRr < mediumEnemyRec.GetLength(0); ++medRr)
                                {
                                    mediumEnemyRec[medRr, medC].Y = mediumEnemyRec[medRr, medC].Y + ENEMY_SPEED;
                                    moveRight = true;
                                }
                            }
                        }
                    }
                    #endregion

                    #region BigLMove
                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            bigEnemyRec[bigR, bigC].X = bigEnemyRec[bigR, bigC].X - ENEMY_SPEED;
                            if (bigEnemyRec[bigR, bigC].X == LEFT_BORDER || mediumEnemyRec[bigR, bigC].X == LEFT_BORDER || smallEnemyRec[bigR].X == LEFT_BORDER)
                            {
                                for (int bigRr = 0; bigRr < bigEnemyRec.GetLength(0); ++bigRr)
                                {
                                    bigEnemyRec[bigRr, bigC].Y = bigEnemyRec[bigRr, bigC].Y + ENEMY_SPEED;
                                    moveRight = true;
                                }
                            }
                        }
                    }
                    #endregion
                    moveInst.Play();
                    enemyMoveTimer = 0;
                }
            }
            #endregion
        }

        //Animation for the invaders movement
        private void Animation(GameTime gameTime)
        {
            Elapsed += (float)(gameTime.ElapsedGameTime.TotalMilliseconds);
            //Sets the animations to change between the sprites
            if (Elapsed >= enemyMoveTimer)
            {
                frames = (frames + 1) % 2;
                Elapsed = 0;
            }
            //Sets the source rectangle for the three enemies' animations
            smallEnemySourceRec = new Rectangle((smallEnemy.Width / 2) * frames, 0, (smallEnemy.Width / 2), (smallEnemy.Height));
            mediumEnemySourceRec = new Rectangle((mediumEnemy.Width / 2) * frames, 0, (mediumEnemy.Width / 2), (mediumEnemy.Height));
            bigEnemySourceRec = new Rectangle((bigEnemy.Width / 2) * frames, 0, (bigEnemy.Width / 2), (bigEnemy.Height));

        }

        //Placement, and collision of barriers
        private void Barriers()
        {
            //Animation and shooting logic behind the 4 barries
            #region Barrier1
            barrier1SourceRec[0, 0] = new Rectangle((barrier4.Width / 4) * barHit1[0, 0], 0, (barrier4.Width / 4), (barrier4.Height));
            barrier1SourceRec[0, 1] = new Rectangle((barrier1.Width / 4) * barHit1[0, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[0, 2] = new Rectangle((barrier1.Width / 4) * barHit1[0, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[0, 3] = new Rectangle((barrier5.Width / 4) * barHit1[0, 3], 0, (barrier5.Width / 4), (barrier5.Height));
            barrier1SourceRec[1, 0] = new Rectangle((barrier1.Width / 4) * barHit1[1, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[1, 1] = new Rectangle((barrier1.Width / 4) * barHit1[1, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[1, 2] = new Rectangle((barrier1.Width / 4) * barHit1[1, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[1, 3] = new Rectangle((barrier1.Width / 4) * barHit1[1, 3], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[2, 0] = new Rectangle((barrier1.Width / 4) * barHit1[2, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier1SourceRec[2, 1] = new Rectangle((barrier3.Width / 4) * barHit1[2, 1], 0, (barrier3.Width / 4), (barrier3.Height));
            barrier1SourceRec[2, 2] = new Rectangle((barrier2.Width / 4) * barHit1[2, 2], 0, (barrier2.Width / 4), (barrier2.Height));
            barrier1SourceRec[2, 3] = new Rectangle((barrier1.Width / 4) * barHit1[2, 3], 0, (barrier1.Width / 4), (barrier1.Height));

            //Collision #1
            for (int BarRow1 = 0; BarRow1 < barrier1Rec.GetLength(0); ++BarRow1)
            {
                for (int BarCol1 = 0; BarCol1 < barrier1Rec.GetLength(1); ++BarCol1)
                {
                    if (laserRec.X <= (barrier1Rec[BarRow1, BarCol1].X + barrier1Rec[BarRow1, BarCol1].Width) && (laserRec.X + laserRec.Width) >= barrier1Rec[BarRow1, BarCol1].X
                    && (laserRec.Y <= (barrier1Rec[BarRow1, BarCol1].Y + barrier1Rec[BarRow1, BarCol1].Height) && (laserRec.Y + laserRec.Height) >= barrier1Rec[BarRow1, BarCol1].Y))
                    {
                        barHit1[BarRow1, BarCol1]++;
                        isBullet = true;
                    }

                    //Collision between bigEnemy bullet and Barrier 1
                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            if (barrier1Rec[BarRow1, BarCol1].X <= (bulletRecB[bigR, bigC].X + bulletRecB[bigR, bigC].Width) && (barrier1Rec[BarRow1, BarCol1].X + barrier1Rec[BarRow1, BarCol1].Width) >= bulletRecB[bigR, bigC].X
                            && (barrier1Rec[BarRow1, BarCol1].Y <= (bulletRecB[bigR, bigC].Y + bulletRecB[bigR, bigC].Height) && (barrier1Rec[BarRow1, BarCol1].Y + barrier1Rec[BarRow1, BarCol1].Height) >= bulletRecB[bigR, bigC].Y))
                            {
                                barHit1[BarRow1, BarCol1]++;
                                bEnemyShot[bigR, bigC] = false;
                            }
                        }
                    }
                    //Collision between mediumEnemy bullet and Barrier 1
                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            if (barrier1Rec[BarRow1, BarCol1].X <= (bulletRecM[medR, medC].X + bulletRecM[medR, medC].Width) && (barrier1Rec[BarRow1, BarCol1].X + barrier1Rec[BarRow1, BarCol1].Width) >= bulletRecM[medR, medC].X
                            && (barrier1Rec[BarRow1, BarCol1].Y <= (bulletRecM[medR, medC].Y + bulletRecM[medR, medC].Height) && (barrier1Rec[BarRow1, BarCol1].Y + barrier1Rec[BarRow1, BarCol1].Height) >= bulletRecM[medR, medC].Y))
                            {
                                barHit1[BarRow1, BarCol1]++;
                                mEnemyShot[medR, medC] = false;
                            }
                        }
                    }
                    //Collision between smallEnemy bullet and Barrier 1
                    for (int s = 0; s < smallEnemyRec.Length; ++s)
                    {
                        if (barrier1Rec[BarRow1, BarCol1].X <= (bulletRecS[s].X + bulletRecS[s].Width) && (barrier1Rec[BarRow1, BarCol1].X + barrier1Rec[BarRow1, BarCol1].Width) >= bulletRecS[s].X
                        && (barrier1Rec[BarRow1, BarCol1].Y <= (bulletRecS[s].Y + bulletRecS[s].Height) && (barrier1Rec[BarRow1, BarCol1].Y + barrier1Rec[BarRow1, BarCol1].Height) >= bulletRecS[s].Y))
                        {
                            barHit1[BarRow1, BarCol1]++;
                            sEnemyShot[s] = false;
                        }
                    }


                    if (barHit1[BarRow1, BarCol1] == 4)
                    {
                        barrier1Rec[BarRow1, BarCol1].X = 10000;
                    }
                }


            }
            #endregion

            #region Barrier2

            barrier2SourceRec[0, 0] = new Rectangle((barrier4.Width / 4) * barHit2[0, 0], 0, (barrier4.Width / 4), (barrier4.Height));
            barrier2SourceRec[0, 1] = new Rectangle((barrier1.Width / 4) * barHit2[0, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[0, 2] = new Rectangle((barrier1.Width / 4) * barHit2[0, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[0, 3] = new Rectangle((barrier5.Width / 4) * barHit2[0, 3], 0, (barrier5.Width / 4), (barrier5.Height));
            barrier2SourceRec[1, 0] = new Rectangle((barrier1.Width / 4) * barHit2[1, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[1, 1] = new Rectangle((barrier1.Width / 4) * barHit2[1, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[1, 2] = new Rectangle((barrier1.Width / 4) * barHit2[1, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[1, 3] = new Rectangle((barrier1.Width / 4) * barHit2[1, 3], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[2, 0] = new Rectangle((barrier1.Width / 4) * barHit2[2, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier2SourceRec[2, 1] = new Rectangle((barrier3.Width / 4) * barHit2[2, 1], 0, (barrier3.Width / 4), (barrier3.Height));
            barrier2SourceRec[2, 2] = new Rectangle((barrier2.Width / 4) * barHit2[2, 2], 0, (barrier2.Width / 4), (barrier2.Height));
            barrier2SourceRec[2, 3] = new Rectangle((barrier1.Width / 4) * barHit2[2, 3], 0, (barrier1.Width / 4), (barrier1.Height));

            //Collision #2
            for (int BarRow2 = 0; BarRow2 < barrier2Rec.GetLength(0); ++BarRow2)
            {
                for (int BarCol2 = 0; BarCol2 < barrier2Rec.GetLength(1); ++BarCol2)
                {
                    if (laserRec.X <= (barrier2Rec[BarRow2, BarCol2].X + barrier2Rec[BarRow2, BarCol2].Width) && (laserRec.X + laserRec.Width) >= barrier2Rec[BarRow2, BarCol2].X
                    && (laserRec.Y <= (barrier2Rec[BarRow2, BarCol2].Y + barrier2Rec[BarRow2, BarCol2].Height) && (laserRec.Y + laserRec.Height) >= barrier2Rec[BarRow2, BarCol2].Y))
                    {
                        barHit2[BarRow2, BarCol2]++;
                        isBullet = true;
                    }

                    //Collision between bigEnemy bullet and Barrier 2
                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            if (barrier2Rec[BarRow2, BarCol2].X <= (bulletRecB[bigR, bigC].X + bulletRecB[bigR, bigC].Width) && (barrier2Rec[BarRow2, BarCol2].X + barrier2Rec[BarRow2, BarCol2].Width) >= bulletRecB[bigR, bigC].X
                            && (barrier2Rec[BarRow2, BarCol2].Y <= (bulletRecB[bigR, bigC].Y + bulletRecB[bigR, bigC].Height) && (barrier2Rec[BarRow2, BarCol2].Y + barrier2Rec[BarRow2, BarCol2].Height) >= bulletRecB[bigR, bigC].Y))
                            {
                                barHit2[BarRow2, BarCol2]++;
                                bEnemyShot[bigR, bigC] = false;
                            }
                        }
                    }
                    //Collision between mediumEnemy bullet and Barrier 2
                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            if (barrier2Rec[BarRow2, BarCol2].X <= (bulletRecM[medR, medC].X + bulletRecM[medR, medC].Width) && (barrier2Rec[BarRow2, BarCol2].X + barrier2Rec[BarRow2, BarCol2].Width) >= bulletRecM[medR, medC].X
                            && (barrier2Rec[BarRow2, BarCol2].Y <= (bulletRecM[medR, medC].Y + bulletRecM[medR, medC].Height) && (barrier2Rec[BarRow2, BarCol2].Y + barrier2Rec[BarRow2, BarCol2].Height) >= bulletRecM[medR, medC].Y))
                            {
                                barHit2[BarRow2, BarCol2]++;
                                mEnemyShot[medR, medC] = false;
                            }
                        }
                    }
                    //Collision between smallEnemy bullet and Barrier 2
                    for (int s = 0; s < smallEnemyRec.Length; ++s)
                    {
                        if (barrier2Rec[BarRow2, BarCol2].X <= (bulletRecS[s].X + bulletRecS[s].Width) && (barrier2Rec[BarRow2, BarCol2].X + barrier2Rec[BarRow2, BarCol2].Width) >= bulletRecS[s].X
                        && (barrier2Rec[BarRow2, BarCol2].Y <= (bulletRecS[s].Y + bulletRecS[s].Height) && (barrier2Rec[BarRow2, BarCol2].Y + barrier2Rec[BarRow2, BarCol2].Height) >= bulletRecS[s].Y))
                        {
                            barHit2[BarRow2, BarCol2]++;
                            sEnemyShot[s] = false;
                        }
                    }
                    if (barHit2[BarRow2, BarCol2] == 4)
                    {
                        barrier2Rec[BarRow2, BarCol2].X = 20000;
                    }
                }


            }
            #endregion

            #region Barrier3
            barrier3SourceRec[0, 0] = new Rectangle((barrier4.Width / 4) * barHit3[0, 0], 0, (barrier4.Width / 4), (barrier4.Height));
            barrier3SourceRec[0, 1] = new Rectangle((barrier1.Width / 4) * barHit3[0, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[0, 2] = new Rectangle((barrier1.Width / 4) * barHit3[0, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[0, 3] = new Rectangle((barrier5.Width / 4) * barHit3[0, 3], 0, (barrier5.Width / 4), (barrier5.Height));
            barrier3SourceRec[1, 0] = new Rectangle((barrier1.Width / 4) * barHit3[1, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[1, 1] = new Rectangle((barrier1.Width / 4) * barHit3[1, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[1, 2] = new Rectangle((barrier1.Width / 4) * barHit3[1, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[1, 3] = new Rectangle((barrier1.Width / 4) * barHit3[1, 3], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[2, 0] = new Rectangle((barrier1.Width / 4) * barHit3[2, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier3SourceRec[2, 1] = new Rectangle((barrier3.Width / 4) * barHit3[2, 1], 0, (barrier3.Width / 4), (barrier3.Height));
            barrier3SourceRec[2, 2] = new Rectangle((barrier2.Width / 4) * barHit3[2, 2], 0, (barrier2.Width / 4), (barrier2.Height));
            barrier3SourceRec[2, 3] = new Rectangle((barrier1.Width / 4) * barHit3[2, 3], 0, (barrier1.Width / 4), (barrier1.Height));

            //Collision #3 
            for (int BarRow3 = 0; BarRow3 < barrier3Rec.GetLength(0); ++BarRow3)
            {
                for (int BarCol3 = 0; BarCol3 < barrier3Rec.GetLength(1); ++BarCol3)
                {

                    if (laserRec.X <= (barrier3Rec[BarRow3, BarCol3].X + barrier3Rec[BarRow3, BarCol3].Width) && (laserRec.X + laserRec.Width) >= barrier3Rec[BarRow3, BarCol3].X
                    && (laserRec.Y <= (barrier3Rec[BarRow3, BarCol3].Y + barrier3Rec[BarRow3, BarCol3].Height) && (laserRec.Y + laserRec.Height) >= barrier3Rec[BarRow3, BarCol3].Y))
                    {
                        barHit3[BarRow3, BarCol3]++;
                        isBullet = true;
                    }

                    //Collision between bigEnemy bullet and Barrier 3
                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            if (barrier3Rec[BarRow3, BarCol3].X <= (bulletRecB[bigR, bigC].X + bulletRecB[bigR, bigC].Width) && (barrier3Rec[BarRow3, BarCol3].X + barrier3Rec[BarRow3, BarCol3].Width) >= bulletRecB[bigR, bigC].X
                            && (barrier3Rec[BarRow3, BarCol3].Y <= (bulletRecB[bigR, bigC].Y + bulletRecB[bigR, bigC].Height) && (barrier3Rec[BarRow3, BarCol3].Y + barrier3Rec[BarRow3, BarCol3].Height) >= bulletRecB[bigR, bigC].Y))
                            {
                                barHit3[BarRow3, BarCol3]++;
                                bEnemyShot[bigR, bigC] = false;
                            }
                        }
                    }
                    //Collision between mediumEnemy bullet and Barrier 3
                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            if (barrier3Rec[BarRow3, BarCol3].X <= (bulletRecM[medR, medC].X + bulletRecM[medR, medC].Width) && (barrier3Rec[BarRow3, BarCol3].X + barrier3Rec[BarRow3, BarCol3].Width) >= bulletRecM[medR, medC].X
                            && (barrier3Rec[BarRow3, BarCol3].Y <= (bulletRecM[medR, medC].Y + bulletRecM[medR, medC].Height) && (barrier3Rec[BarRow3, BarCol3].Y + barrier3Rec[BarRow3, BarCol3].Height) >= bulletRecM[medR, medC].Y))
                            {
                                barHit3[BarRow3, BarCol3]++;
                                mEnemyShot[medR, medC] = false;
                            }
                        }
                    }
                    //Collision between smallEnemy bullet and Barrier 3
                    for (int s = 0; s < smallEnemyRec.Length; ++s)
                    {
                        if (barrier3Rec[BarRow3, BarCol3].X <= (bulletRecS[s].X + bulletRecS[s].Width) && (barrier3Rec[BarRow3, BarCol3].X + barrier3Rec[BarRow3, BarCol3].Width) >= bulletRecS[s].X
                        && (barrier3Rec[BarRow3, BarCol3].Y <= (bulletRecS[s].Y + bulletRecS[s].Height) && (barrier3Rec[BarRow3, BarCol3].Y + barrier3Rec[BarRow3, BarCol3].Height) >= bulletRecS[s].Y))
                        {
                            barHit3[BarRow3, BarCol3]++;
                            sEnemyShot[s] = false;
                        }
                    }


                    if (barHit3[BarRow3, BarCol3] == 4)
                    {
                        barrier3Rec[BarRow3, BarCol3].X = 30000;
                    }

                }
            }
            #endregion

            #region Barrier4

            barrier4SourceRec[0, 0] = new Rectangle((barrier4.Width / 4) * barHit4[0, 0], 0, (barrier4.Width / 4), (barrier4.Height));
            barrier4SourceRec[0, 1] = new Rectangle((barrier1.Width / 4) * barHit4[0, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[0, 2] = new Rectangle((barrier1.Width / 4) * barHit4[0, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[0, 3] = new Rectangle((barrier5.Width / 4) * barHit4[0, 3], 0, (barrier5.Width / 4), (barrier5.Height));
            barrier4SourceRec[1, 0] = new Rectangle((barrier1.Width / 4) * barHit4[1, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[1, 1] = new Rectangle((barrier1.Width / 4) * barHit4[1, 1], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[1, 2] = new Rectangle((barrier1.Width / 4) * barHit4[1, 2], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[1, 3] = new Rectangle((barrier1.Width / 4) * barHit4[1, 3], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[2, 0] = new Rectangle((barrier1.Width / 4) * barHit4[2, 0], 0, (barrier1.Width / 4), (barrier1.Height));
            barrier4SourceRec[2, 1] = new Rectangle((barrier3.Width / 4) * barHit4[2, 1], 0, (barrier3.Width / 4), (barrier3.Height));
            barrier4SourceRec[2, 2] = new Rectangle((barrier2.Width / 4) * barHit4[2, 2], 0, (barrier2.Width / 4), (barrier2.Height));
            barrier4SourceRec[2, 3] = new Rectangle((barrier1.Width / 4) * barHit4[2, 3], 0, (barrier1.Width / 4), (barrier1.Height));

            //Collision #4
            for (int BarRow4 = 0; BarRow4 < barrier4Rec.GetLength(0); ++BarRow4)
            {
                for (int BarCol4 = 0; BarCol4 < barrier4Rec.GetLength(1); ++BarCol4)
                {
                    if (laserRec.X <= (barrier4Rec[BarRow4, BarCol4].X + barrier4Rec[BarRow4, BarCol4].Width) && (laserRec.X + laserRec.Width) >= barrier4Rec[BarRow4, BarCol4].X
                    && (laserRec.Y <= (barrier4Rec[BarRow4, BarCol4].Y + barrier4Rec[BarRow4, BarCol4].Height) && (laserRec.Y + laserRec.Height) >= barrier4Rec[BarRow4, BarCol4].Y))
                    {
                        barHit4[BarRow4, BarCol4]++;
                        isBullet = true;
                    }

                    //Collision between bigEnemy bullet and Barrier 4
                    for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                    {
                        for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                        {
                            if (barrier4Rec[BarRow4, BarCol4].X <= (bulletRecB[bigR, bigC].X + bulletRecB[bigR, bigC].Width) && (barrier4Rec[BarRow4, BarCol4].X + barrier4Rec[BarRow4, BarCol4].Width) >= bulletRecB[bigR, bigC].X
                            && (barrier4Rec[BarRow4, BarCol4].Y <= (bulletRecB[bigR, bigC].Y + bulletRecB[bigR, bigC].Height) && (barrier4Rec[BarRow4, BarCol4].Y + barrier4Rec[BarRow4, BarCol4].Height) >= bulletRecB[bigR, bigC].Y))
                            {
                                barHit4[BarRow4, BarCol4]++;
                                bEnemyShot[bigR, bigC] = false;
                            }
                        }
                    }
                    //Collision between mediumEnemy bullet and Barrier 4
                    for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                    {
                        for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                        {
                            if (barrier4Rec[BarRow4, BarCol4].X <= (bulletRecM[medR, medC].X + bulletRecM[medR, medC].Width) && (barrier4Rec[BarRow4, BarCol4].X + barrier4Rec[BarRow4, BarCol4].Width) >= bulletRecM[medR, medC].X
                            && (barrier4Rec[BarRow4, BarCol4].Y <= (bulletRecM[medR, medC].Y + bulletRecM[medR, medC].Height) && (barrier4Rec[BarRow4, BarCol4].Y + barrier4Rec[BarRow4, BarCol4].Height) >= bulletRecM[medR, medC].Y))
                            {
                                barHit4[BarRow4, BarCol4]++;
                                mEnemyShot[medR, medC] = false;
                            }
                        }
                    }
                    //Collision between smallEnemy bullet and Barrier 4
                    for (int s = 0; s < smallEnemyRec.Length; ++s)
                    {
                        if (barrier4Rec[BarRow4, BarCol4].X <= (bulletRecS[s].X + bulletRecS[s].Width) && (barrier4Rec[BarRow4, BarCol4].X + barrier4Rec[BarRow4, BarCol4].Width) >= bulletRecS[s].X
                        && (barrier4Rec[BarRow4, BarCol4].Y <= (bulletRecS[s].Y + bulletRecS[s].Height) && (barrier4Rec[BarRow4, BarCol4].Y + barrier4Rec[BarRow4, BarCol4].Height) >= bulletRecS[s].Y))
                        {
                            barHit4[BarRow4, BarCol4]++;
                            sEnemyShot[s] = false;
                        }
                    }


                    if (barHit4[BarRow4, BarCol4] == 4)
                    {
                        barrier4Rec[BarRow4, BarCol4].X = 40000;
                    }
                }


            }
            #endregion

        }

        //Invaders shooting at player logic
        private void EnemyShooting()
        {
            //Big Enemy shooting logic
            #region BigEnemy
            for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
            {
                for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                {
                    shootChance = shootRandomizer.Next(0, shootFreq);

                    //If enemy is not shooting the bullet is set to enemy location
                    if (bEnemyShot[bigR, bigC] == false)
                    {
                        bulletRecB[bigR, bigC].X = bigEnemyRec[bigR, bigC].X + bigEnemyRec[bigR, bigC].Width / 2;
                        bulletRecB[bigR, bigC].Y = bigEnemyRec[bigR, bigC].Y;
                    }
                    if (shootChance == 1)
                    {
                        EnemyRow[0] = row1Randomizer.Next(0, 11);
                        EnemyCol[0] = col1Randomizer.Next(0, 2);
                        bEnemyShot[EnemyRow[0], EnemyCol[0]] = true;
                        shootChance = 0;

                    }
                    //Enemy Shoots a bullet
                    if (bEnemyShot[bigR, bigC] == true)
                    {
                        bulletRecB[bigR, bigC].Y = bulletRecB[bigR, bigC].Y + 4;
                    }
                    //If bullet hits the bottom of the screen reset the bullet
                    if (bulletRecB[bigR, bigC].Y >= BOTTOM_BORDER)
                    {
                        bEnemyShot[bigR, bigC] = false;
                    }
                    //If bullet hits the player, lose health and reset bullet
                    if (playerRec.X <= (bulletRecB[bigR, bigC].X + bulletRecB[bigR, bigC].Width) && (playerRec.X + playerRec.Width) >= bulletRecB[bigR, bigC].X
                    && (playerRec.Y <= (bulletRecB[bigR, bigC].Y + bulletRecB[bigR, bigC].Height) && (playerRec.Y + playerRec.Height) >= bulletRecB[bigR, bigC].Y))
                    {
                        health--;
                        enemyMoveTimer = -1000;
                        playerDeathInst.Play();
                        alienShipRec.X = LEFT_BORDER - alienShipRec.Width;
                        alienPosition = 0;
                        bEnemyShot[bigR, bigC] = false;
                    }
                    //When the enemy reaches below the border game is over
                    if (bigEnemyRec[bigR, bigC].Y + +bigEnemyRec[bigR, bigC].Height >= BARRIER_BORDER)
                    {
                        currentGameState = GameState.endScreen;
                    }
                }
            }
            #endregion

            //Medium Enemy shooting logic
            #region MediumEnemy

            for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
            {
                for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                {
                    shootChance = shootRandomizer.Next(0, shootFreq);
                    //If enemy is not shooting the bullet is set to enemy location
                    if (mEnemyShot[medR, medC] == false)
                    {
                        bulletRecM[medR, medC].X = mediumEnemyRec[medR, medC].X + mediumEnemyRec[medR, medC].Width / 2;
                        bulletRecM[medR, medC].Y = mediumEnemyRec[medR, medC].Y;
                    }
                    if (shootChance == 2)
                    {
                        EnemyRow[1] = row2Randomizer.Next(0, 11);
                        EnemyCol[1] = col2Randomizer.Next(0, 2);
                        mEnemyShot[EnemyRow[1], EnemyCol[1]] = true;
                    }

                    //Enemy Shoots a bullet
                    if (mEnemyShot[medR, medC] == true)
                    {
                        bulletRecM[medR, medC].Y = bulletRecM[medR, medC].Y + 4;
                    }
                    //If bullet hits the bottom of the screen reset the bullet
                    if (bulletRecM[medR, medC].Y >= BOTTOM_BORDER)
                    {
                        mEnemyShot[medR, medC] = false;
                    }
                    //If bullet hits the player, lose health and reset bullet
                    if (playerRec.X <= (bulletRecM[medR, medC].X + bulletRecM[medR, medC].Width) && (playerRec.X + playerRec.Width) >= bulletRecM[medR, medC].X
                    && (playerRec.Y <= (bulletRecM[medR, medC].Y + bulletRecM[medR, medC].Height) && (playerRec.Y + playerRec.Height) >= bulletRecM[medR, medC].Y))
                    {
                        health--;
                        enemyMoveTimer = -1000;
                        playerDeathInst.Play();
                        alienPosition = 0;
                        alienShipRec.X = LEFT_BORDER - alienShipRec.Width;
                        mEnemyShot[medR, medC] = false;
                    }
                    //Prevents enemies on top from shooting when enemies are below
                    if (bigEnemyRec[medR, medC].X <= (bulletRecM[medR, medC].X + bulletRecM[medR, medC].Width) && (bigEnemyRec[medR, medC].X + bigEnemyRec[medR, medC].Width) >= bulletRecM[medR, medC].X
                    && (bigEnemyRec[medR, medC].Y <= (bulletRecM[medR, medC].Y + bulletRecM[medR, medC].Height) && (bigEnemyRec[medR, medC].Y + bigEnemyRec[medR, medC].Height) >= bulletRecM[medR, medC].Y))
                    {
                        mEnemyShot[medR, medC] = false;
                    }
                    //When the enemy reaches below the border game is over
                    if (mediumEnemyRec[medR, medC].Y + mediumEnemyRec[medR, medC].Height >= BARRIER_BORDER)
                    {
                        currentGameState = GameState.endScreen;
                    }
                }
            }
            #endregion

            //Small Enemy shooting logic
            #region SmallEnemy

            for (int s = 0; s < smallEnemyRec.Length; ++s)
            {
                shootChance = shootRandomizer.Next(0, shootFreq);
                //If enemy is not shooting the bullet is set to enemy location
                if (sEnemyShot[s] == false)
                {
                    bulletRecS[s].X = smallEnemyRec[s].X + smallEnemyRec[s].Width / 2;
                    bulletRecS[s].Y = smallEnemyRec[s].Y;
                }

                if (shootChance == 3)
                {
                    EnemyRow[2] = row2Randomizer.Next(0, 11);
                    sEnemyShot[EnemyRow[2]] = true;
                }
                //Enemy Shoots a bullet
                if (sEnemyShot[s] == true)
                {
                    bulletRecS[s].Y = bulletRecS[s].Y + 4;
                }
                //If bullet hits the bottom of the screen reset the bullet
                if (bulletRecS[s].Y >= BOTTOM_BORDER)
                {
                    sEnemyShot[s] = false;
                }
                //If bullet hits the player, lose health and reset bullet
                if (playerRec.X <= (bulletRecS[s].X + bulletRecS[s].Width) && (playerRec.X + playerRec.Width) >= bulletRecS[s].X
                && (playerRec.Y <= (bulletRecS[s].Y + bulletRecS[s].Height) && (playerRec.Y + playerRec.Height) >= bulletRecS[s].Y))
                {
                    health--;
                    enemyMoveTimer = -1000;
                    playerDeathInst.Play();
                    alienPosition = 0;
                    alienShipRec.X = LEFT_BORDER - alienShipRec.Width;
                    sEnemyShot[s] = false;
                }
                //Prevents enemies on top from shooting when enemies are below
                if (mediumEnemyRec[s, 0].X <= (bulletRecS[s].X + bulletRecS[s].Width) && (mediumEnemyRec[s, 0].X + mediumEnemyRec[s, 0].Width) >= bulletRecS[s].X
                && (mediumEnemyRec[s, 0].Y <= (bulletRecS[s].Y + bulletRecS[s].Height) && (mediumEnemyRec[s, 0].Y + mediumEnemyRec[s, 0].Height) >= bulletRecS[s].Y))
                {
                    sEnemyShot[s] = false;
                }
                //When the enemy reaches below the border game is over
                if (smallEnemyRec[s].Y + smallEnemyRec[s].Height >= BARRIER_BORDER)
                {
                    currentGameState = GameState.endScreen;
                }
            }
            #endregion
        }

        //New game is created and resests all value to their origins
        private void NewGame()
        {
            //Resests all values if new game is started
            if (isNewGame == true)
            {
                score = 0;
                health = 3;
                enemyCount = 55;
                gamesPlayed = 0;
                enemyMoveTimer = 0;
                shootFreq = 2000;
                moveFreq = 1000;
                alienPosition = 0;
                alienShipRec.X = (LEFT_BORDER - alienShipRec.Width);
                isBullet = true;
                moveRight = true;


                #region Reseting Enemies
                //Resests enemy rectangles to the starting position
                for (int s = 0; s < smallEnemyRec.Length; ++s)
                {
                    smallEnemyRec[s] = new Rectangle(325 + (ENEMY_OFFSET * (s - 1)), 125, 40, 30);
                    bulletRecS[s] = new Rectangle(smallEnemyRec[s].X, smallEnemyRec[s].Y, 10, 15);
                    sEnemyShot[s] = false;
                }
                for (int medR = 0; medR < mediumEnemyRec.GetLength(0); ++medR)
                {
                    for (int medC = 0; medC < mediumEnemyRec.GetLength(1); ++medC)
                    {
                        mediumEnemyRec[medR, medC] = new Rectangle(325 + (ENEMY_OFFSET * (medR - 1)), (175 + (ENEMY_OFFSET * medC - 1)), 40, 30);
                        bulletRecM[medR, medC] = new Rectangle(mediumEnemyRec[medR, medC].X, mediumEnemyRec[medR, medC].Y, 10, 15);
                        mEnemyShot[medR, medC] = false;
                    }
                }
                for (int bigR = 0; bigR < bigEnemyRec.GetLength(0); ++bigR)
                {
                    for (int bigC = 0; bigC < bigEnemyRec.GetLength(1); ++bigC)
                    {
                        bigEnemyRec[bigR, bigC] = new Rectangle(325 + (ENEMY_OFFSET * (bigR - 1)), (275 + (ENEMY_OFFSET * bigC - 1)), 40, 30);
                        bulletRecB[bigR, bigC] = new Rectangle(bigEnemyRec[bigR, bigC].X, bigEnemyRec[bigR, bigC].Y, 10, 15);
                        bEnemyShot[bigR, bigC] = false;
                    }
                }
                #endregion

                #region Reseting Barriers
                //Resets the frame for the barriers
                for (int BarRow1 = 0; BarRow1 < barrier1Rec.GetLength(0); ++BarRow1)
                {
                    for (int BarCol1 = 0; BarCol1 < barrier1Rec.GetLength(1); ++BarCol1)
                    {
                        barHit1[BarRow1, BarCol1] = 0;
                        barHit2[BarRow1, BarCol1] = 0;
                        barHit3[BarRow1, BarCol1] = 0;
                        barHit4[BarRow1, BarCol1] = 0;
                    }
                }
                //Reloads the barriers into the original rectangles
                const int BARRIER_Y = 550;
                const int BARRIER_OFFSET = 25;

                #region LoadBarrier#1
                barrier1Rec[0, 0] = new Rectangle(150, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[0, 1] = new Rectangle(175, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[0, 2] = new Rectangle(200, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[0, 3] = new Rectangle(225, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[1, 0] = new Rectangle(150, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[1, 1] = new Rectangle(175, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[1, 2] = new Rectangle(200, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[1, 3] = new Rectangle(1225, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[2, 0] = new Rectangle(150, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[2, 1] = new Rectangle(175, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[2, 2] = new Rectangle(200, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier1Rec[2, 3] = new Rectangle(225, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                #endregion

                #region LoadBarrier#2
                barrier2Rec[0, 0] = new Rectangle(450, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[0, 1] = new Rectangle(475, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[0, 2] = new Rectangle(500, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[0, 3] = new Rectangle(525, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[1, 0] = new Rectangle(450, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[1, 1] = new Rectangle(475, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[1, 2] = new Rectangle(500, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[1, 3] = new Rectangle(525, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[2, 0] = new Rectangle(450, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[2, 1] = new Rectangle(475, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[2, 2] = new Rectangle(500, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier2Rec[2, 3] = new Rectangle(525, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                #endregion

                #region LoadBarrier#3
                barrier3Rec[0, 0] = new Rectangle(750, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[0, 1] = new Rectangle(775, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[0, 2] = new Rectangle(800, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[0, 3] = new Rectangle(825, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[1, 0] = new Rectangle(750, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[1, 1] = new Rectangle(775, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[1, 2] = new Rectangle(800, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[1, 3] = new Rectangle(825, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[2, 0] = new Rectangle(750, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[2, 1] = new Rectangle(775, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[2, 2] = new Rectangle(800, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier3Rec[2, 3] = new Rectangle(825, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                #endregion

                #region LoadBarrier#4
                barrier4Rec[0, 0] = new Rectangle(1050, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[0, 1] = new Rectangle(1075, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[0, 2] = new Rectangle(1100, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[0, 3] = new Rectangle(1125, BARRIER_Y, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[1, 0] = new Rectangle(1050, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[1, 1] = new Rectangle(1075, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[1, 2] = new Rectangle(1100, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[1, 3] = new Rectangle(1125, BARRIER_Y + BARRIER_OFFSET, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[2, 0] = new Rectangle(1050, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[2, 1] = new Rectangle(1075, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[2, 2] = new Rectangle(1100, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                barrier4Rec[2, 3] = new Rectangle(1125, BARRIER_Y + BARRIER_OFFSET * 2, BARRIER_SIZE, BARRIER_SIZE);
                #endregion
                #endregion
            }
        }

        //Reads the txt file to retrieve the current highscore
        private void FILEI()
        {
            string HS;
            //Sets the name inputted to the path of the file
            filePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            filePath = Path.GetDirectoryName(filePath);
            filePath = filePath.Substring(6);

            //Set the file name
            filePath = filePath + "/siHighScore.txt";

            //Open the file
            inFile = File.OpenText(filePath);

            //Write the highscores to the file
            HS = inFile.ReadLine();
            highScore = Convert.ToInt32(HS);

            //Close the file
            inFile.Close();

        }

        //When the player reaches a score higher than the highscore, set the new score as the highscore
        private void FILEO()
        {
            if (score >= highScore) 
            {
                //Open the file for writing
                outFile = File.CreateText(filePath);

                //Write the score as the new highscore
                outFile.WriteLine(score);

                //Close the compelte file write
                outFile.Close();
            }
        }
    }
}