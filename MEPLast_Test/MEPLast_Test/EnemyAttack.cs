using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class EnemyAttack
    {
        public fk_Model model;
        private bool del;

        double delTime;
        Player player;
        fk_Scene scene;
        fk_AppWindow win;

        bool isEnter;

        public EnemyAttack(fk_Vector _pos, fk_Angle _angle, Player _player, fk_Scene _scene, fk_AppWindow _win)
        {
            del = false;
            delTime = 0.5;
            player = _player;
            isEnter = false;
            win = _win;

            model = new fk_Model();
            model.Shape = new fk_Block(1.0, 1.0, 1.0);
            model.Material = fk_Material.LavaRed;
            model.GlMoveTo(_pos);
            model.GlAngle(_angle);
            model.BMode = fk_BoundaryMode.OBB;
            model.AdjustOBB();
            scene = _scene;
            scene.EntryModel(model);
        }

        public void update()
        {
            delTime -= 1.0 / 60.0;
            if (delTime <= 0)
            {
                del = true;
            }

            onCollider();
        }

        public void onCollider()
        {
            if (model.IsInter(player.GetModel) && !isEnter)
            {
                player.damage(1);
                isEnter = true;
            }
        }

        public bool isDelete
        {
            get
            {
                return del;
            }
        }
    }

    class EnemyArrow
    {
        public fk_Model model;
        bool del;
        double delTime;
        bool isStop;

        public Player player;
        public fk_Scene scene;
        public fk_AppWindow win;

        int soundTime = 0;

        public EnemyArrow(fk_Vector _pos, fk_Angle _angle, Player _player, fk_Scene _scene, fk_AppWindow _win)
        {
            player = _player;
            win = _win;
            delTime = 10.0;
            del = false;
            isStop = false;

            model = new fk_Model();
            //model.Shape = new fk_Prism(8, 0.0001, 0.05, 1.0);
            model.Shape = new fk_Sphere(1, 0.2);
            model.Material = fk_Material.LavaRed;
            model.BMode = fk_BoundaryMode.OBB;
            model.AdjustOBB();
            model.GlMoveTo(_pos);
            model.GlAngle(_angle);

            scene = _scene;
            scene.EntryModel(model);
        }

        public void update()
        {
            delTime -= 1.0 / 60.0;
            if (delTime <= 0)
            {
                del = true;
            }

            move();
            attack();
            actSound();
        }

        public void move()
        {
            if (!isStop)
            {
                model.LoTranslate(0.0, 0.0, -0.2);
            }
        }

        public void attack()
        {
            if (model.IsInter(player.GetModel) && !isStop)
            {
                player.damage(1);
                isStop = true;
                del = true;
            }

            foreach (Floor f in Program.floors)
            {
                if (model.IsInter(f.model))
                {
                    Effect_Bomb eff = new Effect_Bomb(model.Position, scene, 7);
                    eff.setModelLength(10);
                    eff.setSize(0.05);
                    eff.setMaterial(fk_Material.LavaRed);
                    eff.setDelCountSpan(30);
                    Program.effectBombs.Add(eff);
                    del = true;
                }
            }
        }

        public void actSound()
        {
            double magnitude = (Program.player.GetPos - model.Position).Dist();
            soundTime++;
            if (magnitude > 15.0)
            {
                if (soundTime % 60 == 0)
                {
                    double gain = Math.Max(0.1, 5.0 / magnitude);
                    gain = Math.Min(1.0, gain);
                    Program.se.SetGain(10, gain);
                    Program.se.StartSE(10);
                    soundTime = 0;
                }
            }
            else if (magnitude > 7.0)
            {
                if (soundTime % 30 == 0)
                {
                    double gain = Math.Max(0.1, 5.0 / magnitude);
                    gain = Math.Min(1.0, gain);
                    Program.se.SetGain(10, gain);
                    Program.se.StartSE(10);
                    soundTime = 0;
                }
            }
            else
            {
                if (soundTime % 10 == 0)
                {
                    double gain = Math.Max(0.1, 5.0 / magnitude);
                    gain = Math.Min(1.0, gain);
                    Program.se.SetGain(10, gain);
                    Program.se.StartSE(10);
                    soundTime = 0;
                }
            }
        }

        public bool isDelete
        {
            get
            {
                return del;
            }
        }

        public bool setDelete
        {
            set
            {
                Program.addScore(500);
                del = value;
            }
        }
    }
}
