using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class Shot
    {
        public fk_Model model;
        public fk_Model colliderModel;
        private fk_Vector befPos;

        private fk_Vector pos;
        private fk_Vector vec;
        private bool del;

        int delTime;
        private double speed;

        public List<Enemy01> enemies01;
        public List<Enemy02> enemies02;
        public List<Enemy03> enemies03;
        public List<Floor> floors;

        fk_Scene scene;
        fk_AppWindow win;

        public Shot(fk_Vector _pos, fk_Vector _vec, List<Enemy01> _enemies01, List<Enemy02> _enemies02, List<Enemy03> _enemies03, List<Floor> _floors, fk_Scene _scene, fk_AppWindow _win)
        {
            model = new fk_Model();
            model.Shape = new fk_Sphere(1, 0.1);
            model.Material = fk_Material.Orange;
            model.DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;
            model.GlMoveTo(_pos);
            model.LoFocus(_vec);
            //model.BMode = fk_BoundaryMode.OBB;
            //model.AdjustOBB();
            //model.BDraw = true;

            colliderModel = new fk_Model();
            colliderModel.Shape = new fk_Block(0.1, 0.1, 0.1);
            colliderModel.GlMoveTo(_pos);
            colliderModel.GlFocus(_vec);
            colliderModel.BMode = fk_BoundaryMode.OBB;
            colliderModel.AdjustOBB();
            colliderModel.BDraw = true;

            pos = _pos;
            vec = _vec;
            del = false;
            delTime = 1200;
            speed = 0.3;

            enemies01 = _enemies01;
            enemies02 = _enemies02;
            enemies03 = _enemies03;
            floors = _floors;

            scene = _scene;
            scene.EntryModel(model);
            //scene.EntryModel(colliderModel);

            win = _win;

            Program.se.StartSE(0);
        }

        public void update()
        {
            move();
            colliderModelMove();
            onCollider();

            delTime--;
            if (delTime <= 0) del = true;
        }

        public void move()
        {
            vec.y -= 0.005;
            befPos = new fk_Vector(model.Position);
            model.GlTranslate(vec * speed);
            pos = new fk_Vector(model.Position);
            if (model.Position.y <= -50.0)
            {
                del = true;
            }
        }

        public void colliderModelMove()
        {
            colliderModel.GlMoveTo(((befPos + 0.05 * vec) + (pos + 0.05 * vec)) / 2.0);

            colliderModel.GlFocus(pos);
            colliderModel.Shape = new fk_Block(0.1, 0.1, (pos - befPos).Dist());
            colliderModel.AdjustOBB();
        }

        public void onCollider()
        {
            foreach (Enemy01 e in enemies01)
            {
                if (colliderModel.IsInter(e.getCollider) == true)
                {
                    e.damage(1);
                    del = true;
                }
            }
            foreach (Enemy02 e in enemies02)
            {
                if (colliderModel.IsInter(e.model) == true)
                {
                    e.damage(1);
                    del = true;
                }
            }
            foreach (Enemy03 e in enemies03)
            {
                if (colliderModel.IsInter(e.model) == true)
                {
                    e.damage(1);
                    del = true;
                }
            }

            foreach (EnemyArrow e in Program.enemyArrows)
            {
                if (colliderModel.IsInter(e.model) == true)
                {
                    e.setDelete = true;
                    Effect_Bomb eff = new Effect_Bomb(model.Position, scene, 4);
                    eff.setMaterial(fk_Material.LavaRed);
                    eff.setDefaultPosition(0.1);
                    eff.setDelCountSpan(30);
                    eff.setSize(0.05);
                    eff.setSpeed(0.02);
                    Program.effectBombs.Add(eff);

                    del = true;
                }
            }

            foreach (Floor f in floors)
            {
                if (colliderModel.IsInter(f.model) == true)
                {
                    if (f.GetIsWoodeBox)
                    {
                        f.actDamage();
                    }
                    else
                    {
                        Effect_Bomb eff = new Effect_Bomb(pos, scene, 7);
                        eff.setModelLength(10);
                        eff.setSize(0.05);
                        eff.setMaterial(fk_Material.Orange);
                        eff.setDelCountSpan(30);
                        Program.effectBombs.Add(eff);
                    }
                    del = true;
                }
            }

            //foreach (SelectButton s in Program.selectButtons)
            //{
            //    if (colliderModel.IsInter(s.model) == true)
            //    {
            //        s.OnCollider();
            //    }
            //}

            for (int i = 0; i < Program.selectButtons.Count; i++)
            {
                if (colliderModel.IsInter(Program.selectButtons[i].model) == true)
                {
                    Program.selectButtons[i].OnCollider();
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
    }
}
