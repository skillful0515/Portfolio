using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FK_CLI;
using System.Windows.Forms;
using System.Drawing;

namespace MEPLast_Test
{
    class Program
    {
        public enum sceneStateCTRL
        {
            TITLE,
            STAGESELECT,
            STAGE01,
            STAGE02,
        }

        public static sceneStateCTRL sceneState;

        public static fk_Angle camAngle;

        public static fk_Scene titleScene;
        public static fk_Scene selectScene;
        public static fk_Scene stage01Scene;
        public static fk_Scene stage02Scene;

        public static bool selectSetup = false;
        public static bool stage01SetUp = false;
        public static bool stage02SetUp = false;

        public static fk_Model cameraSelectScene;
        public static fk_Model camera01;
        public static fk_Model camera02;

        public static fk_Light[] lightSelectScene;
        public static fk_Light[] light01;
        public static fk_Light[] light02;

        public static fk_Model[] lightModelSelectScene;
        public static fk_Model[] lightModel01;
        public static fk_Model[] lightModel02;

        private static int lightNum = 2;

        //public static int winWidth = 800;
        //public static int winWidth = 1280;
        public static int winWidth = 960;
        //public static int winHeight = 600;
        //public static int winHeight = 720;
        public static int winHeight = 540;

        public static fk_AppWindow win;

        public static Player player;
        public static List<Enemy01> enemies01;
        public static List<Enemy02> enemies02;
        public static List<Enemy03> enemies03;
        public static List<Shot> shots;
        public static List<EnemyAttack> enemyAttacks;
        public static List<EnemyArrow> enemyArrows;
        public static List<Effect_Bomb> effectBombs;

        public static List<Floor> floors;
        public static List<SelectButton> selectButtons;

        public static GoalPoint goal;
        public static bool isClear;
        public static int isClearCount;
        public static bool isGameOver;

        public static fk_Model clearPanel;
        public static fk_Model gameOverPanel;

        public static fk_Model[] limitImage;
        public static fk_Model[] lifeImage;
        public static fk_Model scoreModel;
        public static fk_Model timeModel;

        public static fk_Model[] scoreImage;
        public static fk_RectTexture[] scoreTexture;
        public static fk_Model[] timeImage;

        public static Slinger[] slinger = new Slinger[2];

        public static double mouseX = 0;
        public static double mouseY = 0;

        public static bool isHide = false;

        public static int score = 0;
        public static int scoreBef = 0;
        public static double scoreShow = 0.0;
        public static double scoreCount = 0.0;

        public static double time = 0.0;
        public static DateTime timeStart;
        public static int timeLimit = 500;

        public static fk_Image floorImage;
        public static fk_Image[] woodenBoxImage;

        public static fk_Image[] buttonImage;

        // ランダム用変数
        public static Random rand;

        public static MySE se;

        static void Main(string[] args)
        {
            // ランダム用変数
            rand = new Random();

            Console.WriteLine("_____________________________________________");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|         _____________                      |");
            Console.WriteLine("|         |   TITLE   |                      |");
            Console.WriteLine("|         |     o     |                      |");
            Console.WriteLine("|         |   ＼|／   |                      |");
            Console.WriteLine("|         |_____|_____|                      |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|____________________________________________|");


            Console.WriteLine("\n↓ Please Move To The Upper Left Corner !!! ↓");


            Console.WriteLine("_____________________________________________");
            Console.WriteLine("|   TITLE   |                                |");
            Console.WriteLine("|     o     |                                |");
            Console.WriteLine("|   ＼|／   |                                |");
            Console.WriteLine("|_____|_____|                                |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|                                            |");
            Console.WriteLine("|____________________________________________|");

            fk_Material.InitDefault();

            //winWidth = Screen.PrimaryScreen.Bounds.Width - 10;
            //winHeight = Screen.PrimaryScreen.Bounds.Height - 80;

            win = new fk_AppWindow();
            win.Size = new fk_Dimension(winWidth, winHeight);
            win.BGColor = new fk_Color(0.3, 0.6, 0.8);
            win.ShowGuide(fk_GuideMode.GRID_XZ);

            // ステージセレクト諸設定
            selectScene = new fk_Scene();
            cameraSelectScene = new fk_Model();
            lightSelectScene = new fk_Light[lightNum];
            lightModelSelectScene = new fk_Model[lightNum];
            for (int i = 0; i < lightNum; i++)
            {
                lightSelectScene[i] = new fk_Light();
                lightModelSelectScene[i] = new fk_Model();
            }
            GameSceneSetUp(selectScene, cameraSelectScene, lightSelectScene, lightModelSelectScene);

            // ステージ1諸設定
            stage01Scene = new fk_Scene();
            camera01 = new fk_Model();
            light01 = new fk_Light[lightNum];
            lightModel01 = new fk_Model[lightNum];
            for (int i = 0; i < lightNum; i++)
            {
                light01[i] = new fk_Light();
                lightModel01[i] = new fk_Model();
            }
            GameSceneSetUp(stage01Scene, camera01, light01, lightModel01);

            // ステージ2諸設定
            stage02Scene = new fk_Scene();
            camera02 = new fk_Model();
            light02 = new fk_Light[lightNum];
            lightModel02 = new fk_Model[lightNum];
            for (int i = 0; i < lightNum; i++)
            {
                light02[i] = new fk_Light();
                lightModel02[i] = new fk_Model();
            }
            GameSceneSetUp(stage02Scene, camera02, light02, lightModel02);

            // 床用テクスチャ
            var floorTexture = new fk_RectTexture();
            if (floorTexture.ReadPNG("images/stone.png") == false)
            {
                Console.WriteLine("stone" + " read error");
            }
            floorImage = floorTexture.Image;

            // 木箱用テクスチャ
            var woodenTexture = new fk_RectTexture[7];
            woodenBoxImage = new fk_Image[7];
            for (int i = 0; i < woodenTexture.Length; i++)
            {
                woodenTexture[i] = new fk_RectTexture();
                if (woodenTexture[i].ReadPNG("images/box/" + i + ".png") == false)
                {
                    Console.WriteLine("wooden" + " read error");
                }
                woodenBoxImage[i] = woodenTexture[i].Image;
            }

            // ボタン用テクスチャ
            var buttonTexture = new fk_RectTexture[2];
            buttonImage = new fk_Image[2];
            for (int i = 0; i < buttonTexture.Length; i++)
            {
                buttonTexture[i] = new fk_RectTexture();
                if (buttonTexture[i].ReadPNG("images/STAGE" + (i + 1) + ".png") == false)
                {
                    Console.WriteLine("button" + " read error");
                }
                buttonImage[i] = buttonTexture[i].Image;
            }

            // タイトル画面のシーン設定
            titleScene = new fk_Scene();
            titleScene.BGColor = new fk_Color(0.0, 0.0, 0.0);

            // タイトル画面画像
            var titleModel = new fk_Model();

            // タイトル画面用テクスチャ設定
            var titleTextures = new fk_RectTexture[5];
            for (int i = 0; i < titleTextures.Length; i++)
            {
                titleTextures[i] = new fk_RectTexture();
                int num = i + 1;
                if (titleTextures[i].ReadPNG("images/title/new" + num + ".png") == false)
                {
                    Console.WriteLine(num + ".png read error");
                }
                titleTextures[i].TextureSize = new fk_TexCoord(1.6, 0.9);
                titleTextures[i].RendMode = fk_TexRendMode.SMOOTH;
            }

            // モデルにテクスチャを割りふり
            titleModel.Shape = titleTextures[0];
            titleModel.Material = fk_Material.TrueWhite;
            titleModel.GlMoveTo(0.0, 0.0, 0.0);
            titleScene.EntryModel(titleModel);

            // タイトル用カメラ設定
            var titleCamera = new fk_Model();
            titleCamera.GlMoveTo(0.0, 0.0, 1.25);
            titleCamera.GlFocus(0.0, 0.0, 0.0);
            titleScene.Camera = titleCamera;

            // クリアパネル設定
            clearPanel = new fk_Model();
            var clearPanelTexture = new fk_RectTexture();
            if (clearPanelTexture.ReadPNG("images/clear/1.png") == false)
            {
                Console.WriteLine("clear Panel image error");
            }
            clearPanelTexture.TextureSize = new fk_TexCoord(4.5, 1.0);
            clearPanelTexture.RendMode = fk_TexRendMode.SMOOTH;
            clearPanel.Shape = clearPanelTexture;
            clearPanel.GlMoveTo(0.0, 0.0, -5.0);
            //clearPanel.Parent = camera;
            clearPanel.Material = fk_Material.TrueWhite;

            // ゲームオーバー
            gameOverPanel = new fk_Model();
            var gameOverTexture = new fk_RectTexture();
            if (gameOverTexture.ReadPNG("images/gameover/1.png") == false)
            {
                Console.WriteLine("gameOver Panel image error");
            }
            gameOverTexture.TextureSize = new fk_TexCoord(4.5, 1.0);
            gameOverTexture.RendMode = fk_TexRendMode.SMOOTH;
            gameOverPanel.Shape = gameOverTexture;
            gameOverPanel.GlMoveTo(0.0, 0.0, -5.0);
            //gameOverPanel.Parent = camera;
            gameOverPanel.Material = fk_Material.TrueWhite;

            // 残機画像設定
            limitImage = new fk_Model[3];
            var limitTexture = new fk_RectTexture();
            if (limitTexture.ReadPNG("images/patinkoman01.png") == false)
            {
                Console.WriteLine("limit image error");
            }
            limitTexture.TextureSize = new fk_TexCoord(0.8, 1.0);
            limitTexture.RendMode = fk_TexRendMode.SMOOTH;
            for (int i = 0; i < limitImage.Length; i++)
            {
                limitImage[i] = new fk_Model();
                limitImage[i].Shape = limitTexture;
                limitImage[i].Material = fk_Material.TrueWhite;
                //limitImage[i].GlMoveTo(-7.7 + i, -4.0, -15.0);
                limitImage[i].GlMoveTo(-7.7 + i, 4.5, -15.0);
                //limitImage[i].Parent = camera;
                //stage01Scene.EntryOverlayModel(limitImage[i]);
            }

            // ライフ画像設定
            lifeImage = new fk_Model[3];
            var lifeTexture = new fk_RectTexture();
            if (lifeTexture.ReadPNG("images/heart.png") == false)
            {
                Console.WriteLine("life image error");
            }
            lifeTexture.TextureSize = new fk_TexCoord(0.8, 0.8);
            lifeTexture.RendMode = fk_TexRendMode.SMOOTH;
            for (int i = 0; i < lifeImage.Length; i++)
            {
                lifeImage[i] = new fk_Model();
                lifeImage[i].Shape = lifeTexture;
                lifeImage[i].Material = fk_Material.TrueWhite;
                lifeImage[i].GlMoveTo(-1.0 + i, -4.5, -20.0);
                //lifeImage[i].Parent = camera;
                //stage01Scene.EntryOverlayModel(lifeImage[i]);
            }

            // スコア用画像
            scoreImage = new fk_Model[8];
            scoreTexture = new fk_RectTexture[10];
            for (int i = 0; i < scoreTexture.Length; i++)
            {
                scoreTexture[i] = new fk_RectTexture();
                if (scoreTexture[i].ReadPNG("images/no/" + i + ".png") == false)
                {
                    Console.WriteLine("Num image error");
                }
                scoreTexture[i].TextureSize = new fk_TexCoord(0.5, 0.7);
                scoreTexture[i].RendMode = fk_TexRendMode.SMOOTH;
            }

            for (int i = 0; i < scoreImage.Length; i++)
            {
                scoreImage[i] = new fk_Model();
                scoreImage[i].Shape = scoreTexture[7];
                scoreImage[i].Material = fk_Material.TrueWhite;
                scoreImage[i].GlMoveTo(7.7 - (i / 1.5), 4.5, -15.0);
            }

            // スコアの文字
            var scoreStrTexture = new fk_RectTexture();
            if (scoreStrTexture.ReadPNG("images/score.png") == false)
            {
                Console.WriteLine("score image error");
            }
            scoreStrTexture.TextureSize = new fk_TexCoord(2.57, 1.0);
            scoreStrTexture.RendMode = fk_TexRendMode.SMOOTH;

            scoreModel = new fk_Model();
            scoreModel.Shape = scoreStrTexture;
            scoreModel.Material = fk_Material.TrueWhite;
            scoreModel.GlMoveTo(4.9, 8.5, -25.0);

            // タイム用画像
            timeImage = new fk_Model[3];
            for (int i = 0; i < timeImage.Length; i++)
            {
                timeImage[i] = new fk_Model();
                timeImage[i].Shape = scoreTexture[7];
                timeImage[i].Material = fk_Material.TrueWhite;
                timeImage[i].GlMoveTo((1.0 / 1.5) - (i / 1.5), 4.5, -15.0);
            }

            // タイムの文字
            var timeStrTexture = new fk_RectTexture();
            if (timeStrTexture.ReadPNG("images/time.png") == false)
            {
                Console.WriteLine("time image error");
            }
            timeStrTexture.TextureSize = new fk_TexCoord(1.93, 1.0);
            timeStrTexture.RendMode = fk_TexRendMode.SMOOTH;

            timeModel = new fk_Model();
            timeModel.Shape = timeStrTexture;
            timeModel.Material = fk_Material.TrueWhite;
            timeModel.GlMoveTo(-4.0, 8.5, -25.0);

            player = new Player(new fk_Vector(0.0, 2.0, 0.0), enemies01, enemies02, enemies03, shots, floors, cameraSelectScene, clearPanel, gameOverPanel, win.Scene, win);

            enemies01 = new List<Enemy01>();
            enemies02 = new List<Enemy02>();
            enemies03 = new List<Enemy03>();

            shots = new List<Shot>();
            enemyAttacks = new List<EnemyAttack>();
            enemyArrows = new List<EnemyArrow>();
            effectBombs = new List<Effect_Bomb>();

            floors = new List<Floor>();
            selectButtons = new List<SelectButton>();

            win.Scene = titleScene;
            sceneState = sceneStateCTRL.TITLE;

            isClear = false;
            isClearCount = 1;
            isGameOver = false;

            camAngle = new fk_Angle();

            // BGMの各種設定
            var bgm = new MyBGM("sounds/o13.ogg");
            var bgmTask = new Task(bgm.Start);
            double volume = 0.5;

            // SEの各種設定
            se = new MySE(11);
            var seTask = new Task(se.Start);
            se.LoadData(0, "sounds/laser4.wav");
            se.LoadData(1, "sounds/hit.wav");
            se.LoadData(2, "sounds/slap1.wav");
            se.LoadData(3, "sounds/bomb1.wav");
            se.LoadData(4, "sounds/cancel4.wav");
            se.LoadData(5, "sounds/glass-break4.wav");
            se.LoadData(6, "sounds/glass-break2.wav");
            se.LoadData(7, "sounds/cancel1.wav");
            se.LoadData(8, "sounds/trumpet1.wav");
            se.LoadData(9, "sounds/bubble-burst1.wav");
            se.LoadData(10, "sounds/warning1_parts.wav");

            // SEの各種音量設定
            se.SetGain(0, 0.5);

            win.Open();
            bgmTask.Start();    // BGMスレッド開始
            seTask.Start();     // SEスレッド開始

            while (win.Update() == true)
            {
                switch (sceneState)
                {
                    case sceneStateCTRL.TITLE:
                        sceneTitle(win, titleModel, titleTextures);
                        break;
                    case sceneStateCTRL.STAGESELECT:
                        sceneSelect(cameraSelectScene, camAngle);
                        break;
                    case sceneStateCTRL.STAGE01:
                        sceneStage01(camera01, camAngle);
                        break;
                    case sceneStateCTRL.STAGE02:
                        sceneStage02(camera02, camAngle);
                        break;
                    default:
                        break;
                }

                //Console.WriteLine("score = " + score);

                bgm.Gain = volume;

                if (win == null)
                {
                    break;
                }
            }

            // BGM変数とSE変数に終了を指示
            bgm.EndStatus = true;
            se.EndStatus = true;

            // BGM, SE 両スレッドが終了するまで待機
            Task.WaitAll(new[] { bgmTask, seTask });
        }

        static void GameSceneSetUp(fk_Scene _scene, fk_Model _camera, fk_Light[] _light, fk_Model[] _lightModel)
        {
            _scene.BGColor = new fk_Color(0.3, 0.6, 0.8);

            _scene.Camera = _camera;
            _camera.GlMoveTo(0.0, 0.0, 0.0);
            _camera.GlFocus(0.0, 0.0, -1.0);
            _scene.EntryModel(_camera);

            for (int i = 0; i < _light.Length; i++)
            {
                _light[i].Type = fk_LightType.PARALLEL;
                _lightModel[i].Shape = _light[i];
                _lightModel[i].Material = fk_Material.White;
                _scene.EntryModel(_lightModel[i]);
            }
            _lightModel[0].GlFocus(-1.0, -1.0, -1.0);
            if (lightNum >= 2) _lightModel[1].GlFocus(1.0, -1.0, 1.0);
        }

        static void SetTitleTexture(fk_RectTexture _texture, string _name, fk_TexCoord _texC)
        {
            _texture = new fk_RectTexture();
            if (_texture.ReadPNG(_name) == false)
            {
                Console.WriteLine(_name + " read error");
            }
            _texture.TextureSize = _texC;
        }

        // クリア処理
        static void ClearAction()
        {
            if (isClear && win.GetSpecialKeyStatus(fk_SpecialKey.ENTER, fk_SwitchStatus.PRESS)
                || win.GetSpecialKeyStatus(fk_SpecialKey.BACKSPACE, fk_SwitchStatus.PRESS))
            {
                win.Scene = titleScene;
                sceneState = sceneStateCTRL.TITLE;
                //Cursor.Show();
                switchDebug(false);
                isClear = false;
                isClearCount = 1;
                time = 0.0;
                stage01Scene.RemoveOverlayModel(clearPanel);
                stage02Scene.RemoveOverlayModel(clearPanel);
                player.GetModel.GlMoveTo(0.0, 2.0, 0.0);
                AllDestroy();
            }
        }

        static void GameOverAction()
        {
            if (isGameOver && win.GetSpecialKeyStatus(fk_SpecialKey.ENTER, fk_SwitchStatus.PRESS))
            {
                win.Scene = titleScene;
                sceneState = sceneStateCTRL.TITLE;
                //Cursor.Show();
                switchDebug(false);
                isGameOver = false;
                time = 0.0;
                selectScene.RemoveOverlayModel(gameOverPanel);
                stage01Scene.RemoveOverlayModel(gameOverPanel);
                stage02Scene.RemoveOverlayModel(gameOverPanel);
                player.GetModel.GlMoveTo(0.0, 2.0, 0.0);
                AllDestroy();
                score = 0;
                scoreShow = 0;
                scoreBef = 0;
                for (int i = 0; i < scoreImage.Length; i++)
                {
                    selectScene.RemoveOverlayModel(scoreImage[i]);
                    stage01Scene.RemoveOverlayModel(scoreImage[i]);
                    stage02Scene.RemoveOverlayModel(scoreImage[i]);
                }
            }
        }

        // マウス座標から3Dへ
        static fk_Vector GetMousePos2D(fk_AppWindow argWin)
        {
            // 2D→3D 投影用平面
            var plane = new fk_Plane();
            plane.SetPosNormal(new fk_Vector(0.0, 0.0, 0.0), new fk_Vector(0.0, 0.0, 1.0));
            var pos3D = new fk_Vector();

            var pos2D = argWin.MousePosition;
            argWin.GetProjectPosition(pos2D.x, pos2D.y, plane, pos3D);
            return pos3D;
        }

        static void setUpPanel(fk_Model _camera, fk_Scene _scene)
        {
            clearPanel.Parent = _camera;
            gameOverPanel.Parent = _camera;
            for (int i = 0; i < limitImage.Length; i++)
            {
                limitImage[i].Parent = _camera;
            }
            for (int i = 0; i < lifeImage.Length; i++)
            {
                lifeImage[i].Parent = _camera;
            }
            for (int i = 0; i < scoreImage.Length; i++)
            {
                scoreImage[i].Parent = _camera;
            }
            for (int i = 0; i < timeImage.Length; i++)
            {
                timeImage[i].Parent = _camera;
            }

            // 登録した分をいったん削除してから登録
            for (int i = 0; i < timeImage.Length; i++)
            {
                _scene.RemoveOverlayModel(timeImage[i]);
            }

            scoreModel.Parent = _camera;
            _scene.EntryOverlayModel(scoreModel);
            timeModel.Parent = _camera;
            _scene.EntryOverlayModel(timeModel);
        }

        // タイトルシーン
        static void sceneTitle(fk_AppWindow _win, fk_Model _titleModel, fk_RectTexture[] _titleTextures)
        {
            Cursor preCursor = Cursor.Current;

            var p = GetMousePos2D(_win);
            if (_win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.DOWN, false))
            {
                //Console.WriteLine(p);
            }

            if (p.x >= -0.700980 && p.x <= -0.353860 && p.y >= -0.392616 && p.y <= -0.244332)
            {
                Cursor.Current = Cursors.Hand;

                _titleModel.Shape = _titleTextures[1];

                if (_win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.PRESS, false))
                {
                    _titleModel.Shape = _titleTextures[2];
                }
                if (_win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.UP, false))
                {
                    sceneState = sceneStateCTRL.STAGESELECT;
                    _win.Scene = selectScene;
                    switchDebug(true);
                    selectSetup = true;
                }
            }
            else if (p.x >= 0.353860 && p.x <= 0.700980 && p.y >= -0.392616 && p.y <= -0.244332)
            {
                Cursor.Current = Cursors.Hand;

                _titleModel.Shape = _titleTextures[3];
                if (_win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.PRESS, false))
                {
                    _titleModel.Shape = _titleTextures[4];
                }
                if (_win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.UP, false))
                {
                    // ウィンドウを閉じる処理
                    win.Close();
                }
            }
            else
            {
                Cursor.Current = preCursor;

                _titleModel.Shape = _titleTextures[0];
            }
        }


        // stageSelectシーン
        static void sceneSelect(fk_Model _camera, fk_Angle _camAngle)
        {
            if (selectSetup == true)
            {
                timeStart = DateTime.Now;
                SetUp_SelectScene(selectScene);
                setUpPanel(_camera, selectScene);
                win.ShowGuide(fk_GuideMode.GRID_XZ);
                selectSetup = false;
            }

            baseUpdate(_camera, _camAngle, selectScene);
        }

        // stage01シーン
        static void sceneStage01(fk_Model _camera, fk_Angle _camAngle)
        {
            if (stage01SetUp == true)
            {
                timeStart = DateTime.Now;
                SetUp_Stage01(stage01Scene);
                setUpPanel(_camera, stage01Scene);
                win.ShowGuide(fk_GuideMode.GRID_XZ);
                stage01SetUp = false;
            }

            baseUpdate(_camera, _camAngle, stage01Scene);
        }
        // stage02シーン
        static void sceneStage02(fk_Model _camera, fk_Angle _camAngle)
        {
            if (stage02SetUp == true)
            {
                timeStart = DateTime.Now;
                SetUp_Stage02(stage02Scene);
                setUpPanel(_camera, stage02Scene);
                win.ShowGuide(fk_GuideMode.GRID_XZ);
                stage02SetUp = false;
            }

            baseUpdate(_camera, _camAngle, stage02Scene);
        }

        static void baseUpdate(fk_Model _camera, fk_Angle _camAngle, fk_Scene _scene)
        {
            // タイム表示
            actTime();

            // カメラ操作
            actCamera(_camera, _camAngle, win);

            player.update();

            foreach (Slinger s in slinger)
            {
                s.update();
            }

            foreach (Enemy01 e in enemies01)
            {
                e.update();
            }

            for (int i = enemies01.Count - 1; i >= 0; i--)
            {
                if (enemies01[i].isDelete == true)
                {
                    enemies01.RemoveAt(i);
                }
            }

            foreach (Enemy02 e in enemies02)
            {
                e.update();
            }

            for (int i = enemies02.Count - 1; i >= 0; i--)
            {
                if (enemies02[i].isDelete == true)
                {
                    enemies02.RemoveAt(i);
                }
            }

            foreach (Enemy03 e in enemies03)
            {
                e.update();
            }

            for (int i = enemies03.Count - 1; i >= 0; i--)
            {
                if (enemies03[i].isDelete == true)
                {
                    enemies03.RemoveAt(i);
                }
            }

            //foreach (Shot s in shots)
            //{
            //    s.update();
            //}

            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].update();
            }

            for (int i = shots.Count - 1; i >= 0; i--)
            {
                if (shots[i].isDelete == true)
                {
                    win.Remove(shots[i].model);
                    shots.RemoveAt(i);
                }
            }

            foreach (EnemyAttack e in enemyAttacks)
            {
                e.update();
            }

            for (int i = enemyAttacks.Count - 1; i >= 0; i--)
            {
                if (enemyAttacks[i].isDelete == true)
                {
                    win.Remove(enemyAttacks[i].model);
                    enemyAttacks.RemoveAt(i);
                }
            }

            foreach (EnemyArrow e in enemyArrows)
            {
                e.update();
            }

            for (int i = enemyArrows.Count - 1; i >= 0; i--)
            {
                if (enemyArrows[i].isDelete == true)
                {
                    win.Remove(enemyArrows[i].model);
                    enemyArrows.RemoveAt(i);
                }
            }
            //Console.WriteLine("arrow.Count = {0}", enemyArrows.Count);

            foreach (Floor f in floors)
            {
                f.update();
            }

            for (int i = floors.Count - 1; i >= 0; i--)
            {
                if (floors[i].isDelete == true)
                {
                    win.Remove(floors[i].model);
                    floors.RemoveAt(i);
                }
            }

            foreach (Effect_Bomb e in effectBombs)
            {
                e.update();
            }

            for (int i = effectBombs.Count - 1; i >= 0; i--)
            {
                if (effectBombs[i].isDelete == true)
                {
                    for (int j = 0; j < effectBombs[i].models.Length; j++)
                    {
                        win.Remove(effectBombs[i].models[j]);
                    }
                    effectBombs.RemoveAt(i);
                }
            }

            // ライフ表示
            for (int i = 0; i < lifeImage.Length; i++)
            {
                if (i < player.hp)
                {
                    _scene.EntryOverlayModel(lifeImage[i]);
                }
                else
                {
                    _scene.RemoveOverlayModel(lifeImage[i]);
                }
            }
            // 残機表示
            for (int i = 0; i < limitImage.Length; i++)
            {
                if (i < player.zanki)
                {
                    _scene.EntryOverlayModel(limitImage[i]);
                }
                else
                {
                    _scene.RemoveOverlayModel(limitImage[i]);
                }
            }

            // スコア表示
            if (scoreShow >= 0) _scene.EntryOverlayModel(scoreImage[0]);
            if (scoreShow >= 10) _scene.EntryOverlayModel(scoreImage[1]);
            if (scoreShow >= 100) _scene.EntryOverlayModel(scoreImage[2]);
            if (scoreShow >= 1000) _scene.EntryOverlayModel(scoreImage[3]);
            if (scoreShow >= 10000) _scene.EntryOverlayModel(scoreImage[4]);
            if (scoreShow >= 100000) _scene.EntryOverlayModel(scoreImage[5]);
            if (scoreShow >= 1000000) _scene.EntryOverlayModel(scoreImage[6]);
            if (scoreShow >= 10000000) _scene.EntryOverlayModel(scoreImage[7]);
            actScore();

            // タイム表示
            if (timeLimit - (int)time >= 0) _scene.EntryOverlayModel(timeImage[0]);
            if (timeLimit - (int)time >= 10) _scene.EntryOverlayModel(timeImage[1]);
            if (timeLimit - (int)time >= 100) _scene.EntryOverlayModel(timeImage[2]);

            goal.update();

            // デバッグモード切替
            switchDebug(win);

            // チート機能
            actCheat();

            // クリア時切り替え
            ClearAction();

            // ゲームオーバー時切り替え
            GameOverAction();
        }

        // リストに格納（セットアップ）
        static void SetUp_SelectScene(fk_Scene _scene)
        {
            slinger[0] = new Slinger(new fk_Vector(2.0, 1.5, -7.0), _scene, win);
            slinger[1] = new Slinger(new fk_Vector(-2.0, 1.5, -7.0), _scene, win);

            player = new Player(new fk_Vector(0.0, 2.0, 0.0), enemies01, enemies02, enemies03, shots, floors, cameraSelectScene, clearPanel, gameOverPanel, _scene, win);
            goal = new GoalPoint(new fk_Vector(16.0, 12.0, -34.0), player, _scene, win);

            floors.Add(new Floor(new fk_Vector(0.0, 1.0, 0.0), new fk_Vector(30.0, 1.0, 30.0), _scene, win));

            selectButtons.Add(new SelectButton(new fk_Vector(-2.0, 3.3, -7.0), new fk_Vector(1.0, 1.0, 0.01), "stage01Scene", _scene, buttonImage[0], win));
            selectButtons.Add(new SelectButton(new fk_Vector(2.0, 3.3, -7.0), new fk_Vector(1.0, 1.0, 0.01), "stage02Scene", _scene, buttonImage[1], win));
        }

        // リストに格納（セットアップ）
        static void SetUp_Stage01(fk_Scene _scene)
        {
            slinger[0] = new Slinger(new fk_Vector(2.0, 1.5, -7.0), _scene, win);
            slinger[1] = new Slinger(new fk_Vector(-2.0, 1.5, -7.0), _scene, win);

            player = new Player(new fk_Vector(0.0, 2.0, 0.0), enemies01, enemies02, enemies03, shots, floors, camera01, clearPanel, gameOverPanel, _scene, win);
            goal = new GoalPoint(new fk_Vector(16.0, 12.0, -34.0), player, _scene, win);

            Floor fl = new Floor(new fk_Vector(-5.5, 1.5, 5.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(101);

            //floors.Add(new Floor(new fk_Vector(0.0, 0.0, 0.0), new fk_Vector(15.0, 1.0, 15.0), _scene, win));
            //floors.Add(new Floor(new fk_Vector(0.0, 0.0, -15.0), new fk_Vector(15.0, 1.0, 15.0), _scene, win));
            //floors.Add(new Floor(new fk_Vector(0.0, 0.0, -30.0), new fk_Vector(15.0, 1.0, 15.0), _scene, win));
            //floors.Add(new Floor(new fk_Vector(0.0, 0.0, -45.0), new fk_Vector(15.0, 1.0, 15.0), 1, _scene, win));
            //floors.Add(new Floor(new fk_Vector(0.0, 0.0, -60.0), new fk_Vector(15.0, 1.0, 15.0), 2, _scene, win));
            //floors.Add(new Floor(new fk_Vector(0.0, 0.0, -75.0), new fk_Vector(15.0, 1.0, 15.0), 3, _scene, win));

            floors.Add(new Floor(new fk_Vector(7.0, 1.0, 0.0), new fk_Vector(1.0, 1.0, 13.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(0.0, 1.0, 7.0), new fk_Vector(15.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-7.0, 1.0, 0.0), new fk_Vector(1.0, 1.0, 13.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(4.5, 1.0, -7.0), new fk_Vector(6.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-4.5, 1.0, -7.0), new fk_Vector(6.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));

            //enemies01.Add(new Enemy01(new fk_Vector(0.0, 1.0, -3.0), 5, player, enemyAttacks, _scene, win));

            //enemies02.Add(new Enemy02(new fk_Vector(3.0, 1.0, -3.0), 10, player, enemyArrows, _scene, win, 15.0, 1.0, 15.0, 0.0, 1.0, -15.0));

            //enemies03.Add(new Enemy03(new fk_Vector(5.0, 2.0, -10.0), new fk_Vector(-1.0, 0.0, 0.0), -4, 100, player, floors, _scene, win, 15.0, 1.0, 15.0, 0.0, 1.0, -15.0));

            floors.Add(new Floor(new fk_Vector(0.0, 0.0, 0.0), new fk_Vector(15.0, 1.0, 15.0), _scene, win));
            floors.Add(new Floor(new fk_Vector(0.0, 0.0, -15.0), new fk_Vector(15.0, 1.0, 15.0), _scene, win));

            //
            Floor f = new Floor(new fk_Vector(-5.0, 0.0, -32.25), new fk_Vector(5.0, 1.0, 19.5), 1, _scene, win, fk_Material.AshGray);
            floors.Add(f);
            f.setSpeed(0.3);
            f.setLimit(330);

            floors.Add(new Floor(new fk_Vector(0.0, 0.0, -47.0), new fk_Vector(15.0, 1.0, 10.0), _scene, win));
            floors.Add(new Floor(new fk_Vector(15.0, 0.0, -47.0), new fk_Vector(15.0, 1.0, 10.0), _scene, win));

            f = new Floor(new fk_Vector(25.0, 0.0, -47.0), new fk_Vector(5.0, 1.0, 5.0), 3, _scene, win, fk_Material.AshGray);
            floors.Add(f);
            f.reSetCount(270);

            f = new Floor(new fk_Vector(36.5, 0.0, -47.0), new fk_Vector(5.0, 1.0, 5.0), 3, _scene, win, fk_Material.AshGray);
            floors.Add(f);
            f.reSetCount(90);

            floors.Add(new Floor(new fk_Vector(44.0, 0.0, -47.0), new fk_Vector(10.0, 1.0, 10.0), _scene, win));
            floors.Add(new Floor(new fk_Vector(44.0, 0.0, -17.0), new fk_Vector(10.0, 1.0, 50.0), _scene, win));
            for (int i = 0; i < 10; i++)
            {
                floors.Add(new Floor(new fk_Vector(37.0 - i * 2.0, i, 5.5), new fk_Vector(4.0, 1.0, 5.0), _scene, win));
            }
            floors.Add(new Floor(new fk_Vector(16.0, 10.0, 0.0), new fk_Vector(6.0, 1.0, 16.0), _scene, win));
            floors.Add(new Floor(new fk_Vector(16.0, 10.0, -20.0), new fk_Vector(6.0, 1.0, 24.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(16.0, 10.0, -35.0), new fk_Vector(6.0, 1.0, 6.0), _scene, win));

            Enemy01 ene01 = new Enemy01(new fk_Vector(48.0, 1.0, -35.0), 3, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            enemies01.Add(ene01);
            enemies01.Add(new Enemy01(new fk_Vector(40.0, 1.0, -25.0), 3, player, enemyAttacks, _scene, win));
            enemies01.Add(new Enemy01(new fk_Vector(48.0, 1.0, -15.0), 3, player, enemyAttacks, _scene, win));

            //enemies02.Add(new Enemy02(new fk_Vector(40.0, 1.0, 6.0), 3, player, enemyArrows, _scene, win, 10.0, 1.0, 10.0, 44.0, 1.0, -47.0));
            enemies02.Add(new Enemy02(new fk_Vector(42.0, 1.0, 6.0), 3, player, enemyArrows, _scene, win, 10.0, 1.0, 10.0, 44.0, 1.0, -38.0));
            //enemies02.Add(new Enemy02(new fk_Vector(44.0, 1.0, 6.0), 3, player, enemyArrows, _scene, win, 6.0, 1.0, 6.0, 44.0, 1.0, -47.0));
            enemies02.Add(new Enemy02(new fk_Vector(46.0, 1.0, 6.0), 3, player, enemyArrows, _scene, win, 10.0, 1.0, 10.0, 44.0, 1.0, -38.0));
            //enemies02.Add(new Enemy02(new fk_Vector(48.0, 1.0, 6.0), 3, player, enemyArrows, _scene, win, 2.0, 1.0, 2.0, 44.0, 1.0, -47.0));

            enemies02.Add(new Enemy02(new fk_Vector(0.0, 1.0, -6.0), 3, player, enemyArrows, _scene, win, 10.0, 1.0, 10.0, 44.0, 1.0, -38.0));

            enemies03.Add(new Enemy03(new fk_Vector(9.0, 1.5, -50.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 20.0, 1.0, 10.0, 12.5, 1.0, -47.0));
            enemies03.Add(new Enemy03(new fk_Vector(20.0, 1.5, -42.0), new fk_Vector(0.0, 0.0, -1.0), -5, 5, player, floors, _scene, win, 20.0, 1.0, 10.0, 12.5, 1.0, -47.0));

            enemies03.Add(new Enemy03(new fk_Vector(14.0, 11.5, -28.0), new fk_Vector(0.0, 0.0, 1.0), -2, 10, player, floors, _scene, win, 6.0, 3.0, 34.0, 16.0, 12.0, -14.0));
            enemies03.Add(new Enemy03(new fk_Vector(18.0, 11.5, -28.0), new fk_Vector(0.0, 0.0, 1.0), -2, 10, player, floors, _scene, win, 6.0, 3.0, 34.0, 16.0, 12.0, -14.0));
            //enemies03.Add(new Enemy03(new fk_Vector(16.0, 11.5, -30.0), new fk_Vector(0.0, 0.0, 1.0), -2, 10, player, floors, _scene, win, 6.0, 3.0, 34.0, 16.0, 12.0, -14.0));
        }

        // リストに格納（セットアップ）
        static void SetUp_Stage02(fk_Scene _scene)
        {
            slinger[0] = new Slinger(new fk_Vector(3.0, 1.5, -7.0), _scene, win);
            slinger[1] = new Slinger(new fk_Vector(-3.0, 1.5, -7.0), _scene, win);

            player = new Player(new fk_Vector(0.0, 2.0, 0.0), enemies01, enemies02, enemies03, shots, floors, camera02, clearPanel, gameOverPanel, _scene, win);
            goal = new GoalPoint(new fk_Vector(0.0, 1.5, -56.0), player, _scene, win);

            floors.Add(new Floor(new fk_Vector(0.0, 0.0, -5.0), new fk_Vector(7.0, 1.0, 20.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(-3.0, 1.0, -5.0), new fk_Vector(1.0, 1.0, 20.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(3.0, 1.0, -5.0), new fk_Vector(1.0, 1.0, 20.0), _scene, win, fk_Material.BurntTitan));

            floors.Add(new Floor(new fk_Vector(7.5, 0.0, -17.5), new fk_Vector(20.0, 1.0, 5.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(20.0, 0.0, 5.0), new fk_Vector(5.0, 1.0, 50.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(18.0, 1.5, -14.5), new fk_Vector(1.0, 2.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(22.0, 1.5, -4.5), new fk_Vector(1.0, 2.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(18.0, 1.5, 5.5), new fk_Vector(1.0, 2.0, 1.0), _scene, win, fk_Material.BurntTitan));

            floors.Add(new Floor(new fk_Vector(20.0, 0.0, 5.0), new fk_Vector(5.0, 1.0, 50.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(-2.5, 0.0, 32.5), new fk_Vector(50.0, 1.0, 5.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(-30.0, 0.0, 2.5), new fk_Vector(5.0, 1.0, 65.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(-17.0, 0.0, -31.5), new fk_Vector(31.0, 1.0, 3.0), _scene, win));

            floors.Add(new Floor(new fk_Vector(0.0, 0.0, -45.0), new fk_Vector(3.0, 1.0, 30.0), _scene, win));


            enemies02.Add(new Enemy02(new fk_Vector(20.0, 1.0, 20.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(20.5, 1.0, 21.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(19.5, 1.0, 21.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(21.0, 1.0, 22.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(20.0, 1.0, 22.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(19.0, 1.0, 22.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(19.5, 1.0, 23.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(18.5, 1.0, 23.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(20.5, 1.0, 23.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));
            enemies02.Add(new Enemy02(new fk_Vector(21.5, 1.0, 23.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, 20.0, 1.0, -17.5));

            Enemy01 ene01 = new Enemy01(new fk_Vector(-15.0, 1.0, 31.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-15.0, 1.0, 34.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-18.0, 1.0, 31.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-18.0, 1.0, 34.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-21.0, 1.0, 31.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-21.0, 1.0, 34.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-24.0, 1.0, 31.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-24.0, 1.0, 34.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-27.0, 1.0, 31.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            ene01 = new Enemy01(new fk_Vector(-27.0, 1.0, 34.0), 2, player, enemyAttacks, _scene, win);
            ene01.setSpeed(0.075);
            ene01.setVec(new fk_Vector(1.0, 0.0, 0.0));
            enemies01.Add(ene01);

            enemies02.Add(new Enemy02(new fk_Vector(-36.0, 2.0, 15.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-24.0, 2.0, 15.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-36.0, 2.0, 5.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-24.0, 2.0, 5.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-36.0, 2.0, -5.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-24.0, 2.0, -5.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-36.0, 2.0, -15.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-24.0, 2.0, -15.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-36.0, 2.0, -25.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));
            enemies02.Add(new Enemy02(new fk_Vector(-24.0, 2.0, -25.0), 3, player, enemyArrows, _scene, win, 5.0, 1.0, 5.0, -30.0, 1.0, 32.5));

            floors.Add(new Floor(new fk_Vector(-36.0, 1.0, 15.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-24.0, 1.0, 15.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-36.0, 1.0,5.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-24.0, 1.0, 5.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-36.0, 1.0, -5.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-24.0, 1.0, -5.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-36.0, 1.0, -15.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-24.0, 1.0, -15.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-36.0, 1.0, -25.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));
            floors.Add(new Floor(new fk_Vector(-24.0, 1.0, -25.0), new fk_Vector(1.0, 1.0, 1.0), _scene, win, fk_Material.BurntTitan));

            Floor fl = new Floor(new fk_Vector(-26.0, 2.0, -31.5), new fk_Vector(3.0, 3.0, 3.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(3);

            fl = new Floor(new fk_Vector(-26.0 + 23.0 / 3.0, 2.0, -31.5), new fk_Vector(3.0, 3.0, 3.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(5);

            fl = new Floor(new fk_Vector(-26.0 + 46.0 / 3.0, 2.0, -31.5), new fk_Vector(3.0, 3.0, 3.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(8);

            fl = new Floor(new fk_Vector(-3.0, 2.0, -31.5), new fk_Vector(3.0, 3.0, 3.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(10);


            fl = new Floor(new fk_Vector(0.0, 1.0, 4.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);
            fl = new Floor(new fk_Vector(-1.0, 1.0, 4.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);
            fl = new Floor(new fk_Vector(1.0, 1.0, 4.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);
            fl = new Floor(new fk_Vector(2.0, 1.0, 4.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);
            fl = new Floor(new fk_Vector(-2.0, 1.0, 4.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);

            fl = new Floor(new fk_Vector(-3.0, 2.0, -14.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);
            fl = new Floor(new fk_Vector(3.0, 2.0, -14.5), new fk_Vector(1.0, 1.0, 1.0), _scene, win);
            floors.Add(fl);
            fl.setTextuteImage_Wooden(2);

            //for (int y = 0; y < 5; y++)
            //{
            //    for (int x = 0; x < 5; x++)
            //    {
            //        Floor f = new Floor(new fk_Vector(3 * x - 4.0, 3 * y + 2.0, -5.0), new fk_Vector(3.0, 3.0, 3.0), _scene, win);
            //        floors.Add(f);
            //        f.setTextuteImage_Wooden(3);
            //    }
            //}

            //Floor fl = new Floor(new fk_Vector(0.0, 3.0, -60.0), new fk_Vector(5.0, 5.0, 5.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(10);

            //fl = new Floor(new fk_Vector(0.0, -0.5, -3.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -5.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -7.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -9.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -11.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(2.0, -0.5, -11.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(4.0, -0.5, -11.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(6.0, -0.5, -11.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(8.0, -0.5, -11.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(8.0, -0.5, -13.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(8.0, -0.5, -15.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(8.0, -0.5, -17.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(8.0, -0.5, -19.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(6.0, -0.5, -21.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(4.0, -0.5, -23.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(2.0, -0.5, -25.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -27.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -29.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -34.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -39.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -44.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -49.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -54.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);
            //fl = new Floor(new fk_Vector(0.0, -0.5, -59.5), new fk_Vector(2.0, 2.0, 2.0), _scene, win);
            //floors.Add(fl);
            //fl.setTextuteImage_Wooden(1);

            //for (int j = 0; j < 2; j++)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        enemies01.Add(new Enemy01(new fk_Vector(-5.0 + j * 10.0, 1.0, i * -5.0), 3, player, enemyAttacks, _scene, win));
            //    }
            //}

            //enemies03.Add(new Enemy03(new fk_Vector(-2.0, 1.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(0.0, 1.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(2.0, 1.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(4.0, 1.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(-1.0, 3.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(1.0, 3.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(3.0, 3.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(0.0, 5.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(2.0, 5.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
            //enemies03.Add(new Enemy03(new fk_Vector(1.0, 7.5, -20.0), new fk_Vector(0.0, 0.0, 1.0), -2, 5, player, floors, _scene, win, 10.0, 10.0, 10.0, 0.0, 1.0, -10.0));
        }

        // リストに格納した要素を全削除
        static void AllDestroy()
        {
            foreach (var m in player.models)
            {
                selectScene.RemoveModel(m);
                stage01Scene.RemoveModel(m);
                stage02Scene.RemoveModel(m);
            }
            player.DeleteShadow(selectScene);
            player.DeleteShadow(stage01Scene);
            player.DeleteShadow(stage02Scene);

            player.DeleteTrajectoryModel();

            foreach (var m in player.slinger.models)
            {
                selectScene.RemoveModel(m);
                stage01Scene.RemoveModel(m);
                stage02Scene.RemoveModel(m);
            }

            for (int i = enemies01.Count - 1; i >= 0; i--)
            {
                foreach (var m in enemies01[i].models)
                {
                    selectScene.RemoveModel(m);
                    stage01Scene.RemoveModel(m);
                    stage02Scene.RemoveModel(m);
                }
                enemies01.RemoveAt(i);
            }
            for (int i = enemies02.Count - 1; i >= 0; i--)
            {
                foreach (var m in enemies02[i].models)
                {
                    selectScene.RemoveModel(m);
                    stage01Scene.RemoveModel(m);
                    stage02Scene.RemoveModel(m);
                }
                enemies02.RemoveAt(i);
            }
            for (int i = enemies03.Count - 1; i >= 0; i--)
            {
                foreach (var m in enemies03[i].models)
                {
                    selectScene.RemoveModel(m);
                    stage01Scene.RemoveModel(m);
                    stage02Scene.RemoveModel(m);
                }
                enemies03.RemoveAt(i);
            }
            for (int i = shots.Count - 1; i >= 0; i--)
            {
                selectScene.RemoveModel(shots[i].model);
                stage01Scene.RemoveModel(shots[i].model);
                stage02Scene.RemoveModel(shots[i].model);
                shots.RemoveAt(i);
            }
            for (int i = enemyAttacks.Count - 1; i >= 0; i--)
            {
                selectScene.RemoveModel(enemyAttacks[i].model);
                stage01Scene.RemoveModel(enemyAttacks[i].model);
                stage02Scene.RemoveModel(enemyAttacks[i].model);
                enemyAttacks.RemoveAt(i);
            }
            for (int i = enemyArrows.Count - 1; i >= 0; i--)
            {
                selectScene.RemoveModel(enemyArrows[i].model);
                stage01Scene.RemoveModel(enemyArrows[i].model);
                stage02Scene.RemoveModel(enemyArrows[i].model);
                enemyArrows.RemoveAt(i);
            }
            for (int i = effectBombs.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < effectBombs[i].models.Length; j++)
                {
                    selectScene.RemoveModel(effectBombs[i].models[j]);
                    stage01Scene.RemoveModel(effectBombs[i].models[j]);
                    stage02Scene.RemoveModel(effectBombs[i].models[j]);
                }
                effectBombs.RemoveAt(i);
            }
            for (int i = floors.Count - 1; i >= 0; i--)
            {
                selectScene.RemoveModel(floors[i].model);
                stage01Scene.RemoveModel(floors[i].model);
                stage02Scene.RemoveModel(floors[i].model);
                floors[i].DeleteTexture(selectScene);
                floors[i].DeleteTexture(stage01Scene);
                floors[i].DeleteTexture(stage02Scene);
                floors.RemoveAt(i);
            }
            for (int i = selectButtons.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < 2; j++)
                {
                    selectScene.RemoveModel(selectButtons[i].textureModel[j]);
                    stage01Scene.RemoveModel(selectButtons[i].textureModel[j]);
                    stage02Scene.RemoveModel(selectButtons[i].textureModel[j]);
                }
                selectButtons.RemoveAt(i);
            }
        }

        // タイム処理
        static void actTime()
        {
            if (isClear == false && timeLimit - time > 0)
            {
                time = (DateTime.Now - timeStart).TotalSeconds;
                isClearCount = 0;
            }
            if (isClear == true)
            {
                isClearCount++;
                if (isClearCount == 1)
                {
                    int val = (timeLimit - (int)time) * 100;
                    if (val > 0)
                    {
                        addScore(val);
                    }
                }


                if (isClearCount <= 150 && isClearCount % 30 == 0)
                {
                    // クリアエフェクト
                    for (int i = 0; i < 5; i++)
                    {
                        var pos = new fk_Vector(player.GetPos);
                        var randVec = new fk_Vector((rand.NextDouble() - 0.5), (rand.NextDouble() - 0.5), -(rand.NextDouble() + 0.5));
                        randVec.Normalize();
                        randVec *= 5.0;
                        //randVec.x *= player.GetVec.x;
                        //randVec.y *= player.GetVec.y;
                        //randVec.z *= player.GetVec.z;
                        ////Console.WriteLine("randVec = " + randVec.x + ", " + randVec.y + ", " + randVec.z);

                        //pos += randVec * 5.0;
                        //if (pos.y < player.GetPos.y)
                        //{
                        //    pos.y = player.GetPos.y;
                        //}

                        Effect_Bomb eff = new Effect_Bomb();
                        if (i == 0) eff = new Effect_Bomb(randVec, win.Scene, 9);
                        else eff = new Effect_Bomb(randVec, win.Scene, -1);
                        eff.setModelLength(5);
                        eff.setDelCountSpan(30);
                        eff.setSpeed(0.05);
                        eff.setParent(player.GetCamera);
                        if (i == 0) eff.setMaterial(fk_Material.LightGreen);
                        if (i == 1) eff.setMaterial(fk_Material.LightBlue);
                        if (i == 2) eff.setMaterial(fk_Material.Lilac);
                        if (i == 3) eff.setMaterial(fk_Material.Pink);
                        if (i == 4) eff.setMaterial(fk_Material.Violet);
                        effectBombs.Add(eff);
                    }
                }
            }

            for (int i = 0; i < timeImage.Length; i++)
            {
                timeImage[i].Shape = scoreTexture[desigFloor(timeLimit - (int)time, i + 1)];
            }

            if (time >= timeLimit + 1)
            {
                // タイムオーバー処理

            }
        }

        // カメラ操作
        static void actCamera(fk_Model _cam, fk_Angle _camAngle, fk_AppWindow _win)
        {
            _cam.GlMoveTo(player.GetPos + new fk_Vector(0, 1.0, 0));
            //Console.WriteLine("player = {0}, cam = {1}", player.GetPos, _cam.Position);

            var speed = new fk_Vector();
            double rate = 0.002;
            var origin = _cam.Position;

            if (Math.Abs(mouseX - _win.MousePosition.x) < winWidth / 2.0) speed.x = mouseX - _win.MousePosition.x;
            if (Math.Abs(mouseY - _win.MousePosition.y) < winHeight / 2.0) speed.y = mouseY - _win.MousePosition.y;

            speed *= rate;

            //if (_camAngle.p > FK.PI / 2.0)
            //{
            //    _camAngle.p = FK.PI / 2.0;
            //    speed.y = 0;
            //}
            //else if (_camAngle.p < -FK.PI / 2.0)
            //{
            //    _camAngle.p = -FK.PI / 2.0;
            //    speed.y = 0;
            //}
            //_camAngle.p += speed.y;

            //_camAngle.h -= speed.x;

            //_cam.GlAngle(_camAngle);

            _camAngle.p += speed.y;

            _camAngle.h -= speed.x;

            if (_camAngle.p > FK.PI / 2.0)
            {
                _camAngle.p -= speed.y;
            }

            if (_camAngle.p < -FK.PI / 2.0)
            {
                _camAngle.p -= speed.y;
            }

            _cam.GlAngle(_camAngle);

            mouseX = _win.MousePosition.x;
            mouseY = _win.MousePosition.y;

            // カーソル操作
            if (isHide)
            {
                actCursor();
            }
        }

        // カーソル操作
        static void actCursor()
        {
            int cursorX = Cursor.Position.X;
            int cursorY = Cursor.Position.Y;

            if (cursorX > winWidth - 50) Cursor.Position = new Point(50, cursorY);
            else if (cursorX < 50) Cursor.Position = new Point(winWidth - 50, cursorY);

            if (cursorY > winHeight - 50) Cursor.Position = new Point(cursorX, 50);
            else if (cursorY < 50) Cursor.Position = new Point(cursorX, winHeight - 50);
        }

        // スコア表示操作
        static double scoreSpeed = 60.0;
        static void actScore()
        {
            scoreCount += 1.0 / scoreSpeed;
            if (scoreCount > 1.0)
            {
                scoreCount = 1.0;
            }

            scoreShow = (1.0 - scoreCount) * scoreBef + scoreCount * score;
            for (int i = 0; i < scoreImage.Length; i++)
            {
                scoreImage[i].Shape = scoreTexture[desigFloor((int)scoreShow, i + 1)];
            }
        }

        // スコア追加処理（このメソッドから追加するように）
        static public void addScore(int _value)
        {
            scoreBef = score;

            scoreCount = 0;

            score += _value;
        }

        // セレクトボタンからのリファレンス
        static public void setupTrigger(string _scene)
        {
            AllDestroy();

            if (_scene == "stage01Scene")
            {
                sceneState = sceneStateCTRL.STAGE01;
                stage01SetUp = true;
                win.Scene = stage01Scene;
            }
            else if (_scene == "stage02Scene")
            {
                sceneState = sceneStateCTRL.STAGE02;
                stage02SetUp = true;
                win.Scene = stage02Scene;
            }
        }

        // デバッグモード
        static void switchDebug(fk_AppWindow _win)
        {
            if (_win.GetSpecialKeyStatus(fk_SpecialKey.F3, fk_SwitchStatus.DOWN))
            {
                switchDebug(!isHide);
            }
        }
        // デバッグモード
        static public void switchDebug(bool _isDebug)
        {
            isHide = _isDebug;
            if (isHide) Cursor.Hide();
            else Cursor.Show();
            Console.WriteLine(isHide);
        }

        // チート
        static void actCheat()
        {
            if (win.GetKeyStatus('j', fk_SwitchStatus.PRESS))
            {
                player.setCheatJump = false;
            }
        }

        // 指定した位の数字を取得するメソッド
        public static int desigFloor(int _val, int _dig)
        {
            int i = 0;
            if (_val >= 10)
            {
                i = _val / (int)(Math.Pow(10, _dig - 1)) % 10;
            }
            else
            {
                i = _val;
                if (_dig > 1)
                {
                    i = 0;
                }
            }
            return i;
        }
    }
}
