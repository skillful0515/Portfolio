using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class Enemy
    {
        public fk_Model model;
        public fk_Vector pos;
        public int hp;
        private bool del;

        public fk_Material damageMat;
        public double damageCount;
        public double damageCountSpan;

        public fk_Scene scene;
        public fk_AppWindow win;
        public Player player;

        public fk_Model[] models;
        public fk_Material[] defaultMats;
        public fk_Model shadow;

        public Enemy(fk_Vector _pos, int _hp, Player _player, fk_Scene _scene, fk_AppWindow _win)
        {
            model = new fk_Model();
            pos = _pos;
            player = _player;
            scene = _scene;
            win = _win;
            hp = _hp;

            shadow = new fk_Model();
            shadow.Shape = new fk_Circle(32, 0.5);
            shadow.Material = fk_Material.GlossBlack;
            shadow.GlMoveTo(model.Position);
            shadow.GlAngle(0.0, -FK.PI / 2.0, 0.0);
            scene.EntryModel(shadow);

            del = false;

            // 初期設定。モデルの設定は、子クラスで行う
            model.GlMoveTo(pos);
            scene.EntryModel(model);
        }

        public void damage(int val)
        {
            hp -= val;
            damageCount -= 1.0 / 60.0;
            if (hp <= 0)
            {
                Effect_Bomb eff = new Effect_Bomb(model.Position, scene, 3);
                eff.setMaterial(defaultMats[0]);
                Program.effectBombs.Add(eff);
                Program.addScore(1000);
                del = true;
            }
            else
            {
                double magnitude = (Program.player.GetPos - model.Position).Dist();
                double gain = Math.Max(0.1, 10.0 / magnitude);
                gain = Math.Min(1.0, gain);
                Program.se.SetGain(1, gain);
                Program.se.StartSE(1);
                Program.addScore(100);
            }
        }

        public void baseUpdate()
        {
            shadow.GlMoveTo(model.Position + new fk_Vector(0.0, -0.49, 0.0));
            setDelete();
            damageEffect();
        }

        public void setDelete()
        {
            if (isDelete)
            {
                foreach (var m in models)
                {
                    win.Remove(m);
                }
                win.Remove(shadow);
            }
        }

        // ダメージを受けたときに、マテリアルを変化させる処理
        public void damageEffect()
        {
            if (damageCount < damageCountSpan)
            {
                foreach (var m in models)
                {
                    m.Material = damageMat;
                }
                damageCount -= 1.0 / 60.0;
                if (damageCount <= 0)
                {
                    for (int i = 0; i < models.Length; i++)
                    {
                        if (i < defaultMats.Length)
                        {
                            models[i].Material = defaultMats[i];
                        }
                    }
                    damageCount = damageCountSpan;
                }
            }
        }

        public bool isDelete
        {
            get
            {
                return del;
            }
            set
            {
                isDelete = value;
            }
        }
    }

    class Enemy01 : Enemy
    {
        fk_Model body;

        fk_Model viewCollider;
        public bool isFind;
        int befHp;

        private double attackCount;
        private double attackCountSpan;

        public List<EnemyAttack> enemyAttacks;

        public double speed;

        fk_Model collider;

        public Enemy01(fk_Vector _pos, int _hp, Player _player, List<EnemyAttack> _enemyAttacks, fk_Scene _scene, fk_AppWindow _win) : base(_pos, _hp, _player, _scene, _win)
        {
            enemyAttacks = _enemyAttacks;

            // モデルの設定
            model.Shape = new fk_Cone(4, 0.5, 1.0);
            //model.DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;

            body = new fk_Model();
            body.Shape = new fk_Prism(4, 0.5, 0.5, 0.5);
            body.DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;
            body.GlMoveTo(0.0, 0.0, 0.5);
            body.Parent = model;
            scene.EntryModel(body);

            // ダメージ時のマテリアルの変更
            damageCountSpan = 0.2;
            damageCount = damageCountSpan;
            damageMat = fk_Material.TrueWhite;

            // 視覚コライダー
            viewCollider = new fk_Model();
            viewCollider.Shape = new fk_Block(15.0, 1.0, 30.0);
            viewCollider.GlMoveTo(0.0, 0.0, -15.0);
            viewCollider.BMode = fk_BoundaryMode.OBB;
            viewCollider.AdjustOBB();
            //viewCollider.BDraw = true;
            viewCollider.Parent = model;
            viewCollider.DrawMode = fk_DrawMode.LINEMODE;
            //scene.EntryModel(viewCollider);

            // あたり判定用コライダー
            collider = new fk_Model();
            collider.Shape = new fk_Block(1.0, 1.0, 1.5);
            collider.DrawMode = fk_DrawMode.LINEMODE;
            collider.BMode = fk_BoundaryMode.OBB;
            collider.AdjustOBB();
            collider.GlMoveTo(0.0, 0.0, -0.25);
            collider.Parent = model;
            collider.BDraw = false;
            //scene.EntryModel(collider);

            models = new fk_Model[] { model, body, shadow, viewCollider, collider };
            defaultMats = new fk_Material[] { fk_Material.Yellow, fk_Material.Yellow, fk_Material.GlossBlack };
            model.Material = defaultMats[0];
            body.Material = defaultMats[1];

            // 攻撃に関する設定
            isFind = false;
            attackCountSpan = 1.0;
            attackCount = attackCountSpan;

            befHp = hp;

            speed = 0.03;
        }

        public void update()
        {
            move();
            baseUpdate();

            checkFind();
        }

        public void move()
        {
            if (isFind)
            {
                model.GlFocus(player.GetPos.x, model.Position.y, player.GetPos.z);
                var distance = new fk_Vector(player.GetPos - model.Position);
                distance.y = 0; // 高さ無視
                if (distance.Dist() >= 1.5)
                {
                    //model.GlVec(player.Position - model.Position);
                    model.LoTranslate(0.0, 0.0, -speed);
                }
                else
                {
                    attack();
                }
            }
        }

        public void checkFind()
        {
            if (viewCollider.IsInter(player.GetModel) && !isFind)
            {
                isFind = true;
            }

            if (befHp != hp)
            {
                isFind = true;
            }

            befHp = hp;
        }

        public void attack()
        {
            attackCount -= 1.0 / 60.0;
            if (attackCount <= 0)
            {
                enemyAttacks.Add(new EnemyAttack(model.Position + model.Vec, model.Angle, player, scene, win));
                attackCount = attackCountSpan;
            }
        }

        public fk_Model getCollider
        {
            get
            {
                return collider;
            }
        }

        public void setSpeed(double _value)
        {
            speed = _value;
        }

        public void setVec(fk_Vector _vec)
        {
            model.LoFocus(_vec);
        }
    }
    class Enemy02 : Enemy
    {
        fk_Model nose;

        fk_Model viewCollider;
        public bool isFind;
        int befHp;

        List<EnemyArrow> enemyArrows;

        double attackCount;
        double attackCountSpan;

        public Enemy02(fk_Vector _pos, int _hp, Player _player, List<EnemyArrow> _enemyArrows, fk_Scene _scene, fk_AppWindow _win, double _blockX, double _blockY, double _blockZ, double _posX, double _posY, double _posZ) : base(_pos, _hp, _player, _scene, _win)
        {
            enemyArrows = _enemyArrows;
            isFind = false;

            // モデルの設定
            model.Shape = new fk_Sphere(1, 0.5);
            model.DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;

            nose = new fk_Model();
            nose.Shape = new fk_Block(0.1, 0.1, 0.5);
            nose.GlMoveTo(0.0, 0.0, -0.4);
            nose.Parent = model;
            scene.EntryModel(nose);

            // あたり判定設定
            model.BMode = fk_BoundaryMode.OBB;
            model.AdjustOBB();
            //model.AdjustCapsule(new fk_Vector(0.0, 0.0, 0.1), new fk_Vector(0.0, 0.0, -0.1));
            model.BDraw = false;

            // ダメージ時のマテリアルの変更
            damageCountSpan = 0.2;
            damageCount = damageCountSpan;
            damageMat = fk_Material.TrueWhite;

            // 視覚コライダー
            viewCollider = new fk_Model();
            viewCollider.Shape = new fk_Block(_blockX, _blockY, _blockZ);
            viewCollider.GlMoveTo(_posX, _posY, _posZ);
            viewCollider.DrawMode = fk_DrawMode.LINEMODE;
            viewCollider.BMode = fk_BoundaryMode.OBB;
            viewCollider.AdjustOBB();
            //scene.EntryModel(viewCollider);

            // 配列にぶち込む
            models = new fk_Model[] { model, nose, shadow, viewCollider };
            defaultMats = new fk_Material[] { fk_Material.Blue, fk_Material.Red, fk_Material.GlossBlack };
            model.Material = defaultMats[0];
            nose.Material = defaultMats[1];

            // 攻撃関連設定
            attackCountSpan = 3.0;
            attackCount = attackCountSpan;

            befHp = hp;
        }

        public void update()
        {
            move();
            baseUpdate();

            checkFind();
        }

        public void move()
        {
            //model.LoTranslate(0.0, 0.0, -0.01);
            if (isFind)
            {
                model.GlFocus(player.GetPos);

                // 攻撃処理
                attack();
            }
        }

        public void checkFind()
        {
            if (viewCollider.IsInter(player.GetModel)
                || hp != befHp)
            {
                isFind = true;
            }

            befHp = hp;
        }

        public void attack()
        {
            attackCount -= 1.0 / 60.0;
            if (attackCount <= 0)
            {
                enemyArrows.Add(new EnemyArrow(model.Position, model.Angle, player, scene, win));
                attackCount = attackCountSpan;
            }
        }
    }

    class Enemy03 : Enemy
    {
        fk_Model eyeR;
        fk_Model eyeL;

        fk_Model viewCollider;

        List<Floor> floors;

        double attackCountSpan;
        double attackCount;
        int shotFloorId;

        public Enemy03(fk_Vector _pos, fk_Vector _loFocus, int _shotFloorId, int _hp, Player _player, List<Floor> _floors, fk_Scene _scene, fk_AppWindow _win, double _blockX, double _blockY, double _blockZ, double _posX, double _posY, double _posZ) : base(_pos, _hp, _player, _scene, _win)
        {
            floors = _floors;
            shotFloorId = _shotFloorId;

            // モデルの設定
            model.Shape = new fk_Block(2.0, 2.0, 1.0);
            model.DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;
            model.LoFocus(_loFocus);

            eyeR = new fk_Model();
            eyeR.Shape = new fk_Block(0.2, 0.2, 0.1);
            eyeR.GlMoveTo(0.3, 0.2, -0.51);
            eyeR.Parent = model;
            scene.EntryModel(eyeR);

            eyeL = new fk_Model();
            eyeL.Shape = new fk_Block(0.2, 0.2, 0.1);
            eyeL.GlMoveTo(-0.3, 0.2, -0.51);
            eyeL.Parent = model;
            scene.EntryModel(eyeL);

            // あたり判定設定
            model.BMode = fk_BoundaryMode.OBB;
            model.AdjustOBB();
            model.BDraw = false;

            // ダメージ時のマテリアルの変更
            damageCountSpan = 0.2;
            damageCount = damageCountSpan;
            damageMat = fk_Material.TrueWhite;

            // 範囲コライダー設定
            viewCollider = new fk_Model();
            viewCollider.Shape = new fk_Block(_blockX, _blockY, _blockZ);
            viewCollider.DrawMode = fk_DrawMode.LINEMODE;
            viewCollider.LineColor = new fk_Color(1.0, 1.0, 1.0);
            viewCollider.GlMoveTo(_posX, _posY, _posZ);
            viewCollider.BMode = fk_BoundaryMode.OBB;
            viewCollider.AdjustOBB();
            //scene.EntryModel(viewCollider);

            models = new fk_Model[] { model, eyeR, eyeL, shadow, viewCollider };
            defaultMats = new fk_Material[] { fk_Material.Red, fk_Material.DarkRed, fk_Material.DarkRed, fk_Material.GlossBlack };
            model.Material = defaultMats[0];
            eyeR.Material = defaultMats[1];
            eyeL.Material = defaultMats[2];

            // 攻撃処理関係
            attackCountSpan = 1.0;
            attackCount = attackCountSpan;
        }

        public void update()
        {
            checkFind();
            baseUpdate();
        }

        public void move()
        {
            //model.GlFocus(player.GetPos.x, model.Position.y, player.GetPos.z);
            //model.LoTranslate(0.0, 0.0, -0.01);
        }


        public void checkFind()
        {
            if (viewCollider.IsInter(player.GetModel))
            {
                move();
                attack();
            }
        }

        public void attack()
        {
            attackCount -= 1.0 / 60.0;
            if (attackCount <= 0)
            {
                Floor f = new Floor(model.Position, new fk_Vector(1.0, 1.0, 1.0), shotFloorId, scene, win, fk_Material.LavaRed);
                floors.Add(f);
                f.setSpeed(1.0);
                attackCount = attackCountSpan;
            }
        }
    }
}
