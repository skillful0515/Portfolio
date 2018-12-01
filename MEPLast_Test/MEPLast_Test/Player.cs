using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class Player
    {
        public fk_Model model;
        private fk_Vector pos;
        private fk_Angle angle;
        public double speed;
        public int hp;
        public int zanki;

        private fk_Vector moveDirection;

        // 影
        private fk_Model shadow;

        // リスト諸々用変数
        public List<Enemy01> enemies01;
        public List<Enemy02> enemies02;
        public List<Enemy03> enemies03;
        public List<Shot> shots;
        public List<Floor> floors;

        fk_Model camera;
        fk_AppWindow win;

        public Slinger slinger;

        private fk_Vector offset;

        private double jumpPower;
        private bool isJumping;
        private fk_Vector jumpInertia;
        private double inertiaTime;

        // 下側のあたり判定用変数
        private fk_Model footCollider;
        private double wide = 2.0;
        private double footWide = 0.5;
        private fk_Vector modelDistance;

        // 前後左右あたり判定
        private fk_Model frontCollider;
        private fk_Model backCollider;
        private fk_Model rightCollider;
        private fk_Model leftCollider;

        // 下側にふれた床取得用変数
        private Floor other;
        private Floor otherForShadow;

        // 攻撃時に処理負荷の関係から連続して攻撃することを防ぐための変数
        private double attackCount;
        private double attackCountSpan = 0.1;

        // チャージ攻撃用変数
        private double chargePower;
        private double chargePower_max = 5.0;
        private double chargeSpeed = 1.0;

        // 弾の軌跡用変数
        fk_Model trajectoryModel;
        fk_Line line;

        // クリアパネル変数
        fk_Model clearPanel;

        // ゲームオーバーパネル
        fk_Model gameOverPanel;

        // シーン変数
        fk_Scene scene;

        // モデル一括管理用配列
        public fk_Model[] models;

        // ダメージ処理用変数
        public double damageCount;
        public double damageCountSpan;

        public Player(fk_Vector _pos, List<Enemy01> _enemies01, List<Enemy02> _enemies02, List<Enemy03> _enemies03, List<Shot> _shots, List<Floor> _floors, fk_Model _camera, fk_Model _clearPanel, fk_Model _gameOverPanel, fk_Scene _scene, fk_AppWindow _win)
        {
            // プレイヤーのメインのあたり判定を含む、model設定
            model = new fk_Model();
            model.Shape = new fk_Block(1.0, wide, 1.0);
            //model.Material = fk_Material.Red;
            model.DrawMode = fk_DrawMode.LINEMODE;
            model.LineColor = new fk_Color(1.0, 1.0, 1.0);
            model.GlMoveTo(_pos);
            model.BMode = fk_BoundaryMode.OBB;
            //model.BDraw = true;
            model.AdjustOBB();

            pos = _pos;
            angle = new fk_Angle(0, 0, 0);
            speed = 0.1;
            moveDirection = new fk_Vector();
            enemies01 = _enemies01;
            enemies02 = _enemies02;
            enemies03 = _enemies03;
            shots = _shots;
            floors = _floors;
            //_win.Entry(model);
            camera = _camera;
            clearPanel = _clearPanel;
            gameOverPanel = _gameOverPanel;
            scene = _scene;
            win = _win;

            // 影の設定
            shadow = new fk_Model();
            shadow.Shape = new fk_Circle(32, 0.4);
            shadow.Material = fk_Material.GlossBlack;
            shadow.GlMoveTo(model.Position);
            shadow.GlAngle(0.0, -FK.PI / 2.0, 0.0);
            scene.EntryModel(shadow);

            slinger = new Slinger(new fk_Vector(0.0, -1.6, -2.0), scene, win);
            slinger.model.Parent = camera;

            offset = new fk_Vector();

            isJumping = true;
            jumpInertia = new fk_Vector();

            // 下側あたり判定
            footCollider = new fk_Model();
            footCollider.Shape = new fk_Block(1.5, footWide, 1.5);
            footCollider.Material = fk_Material.Blue;
            footCollider.DrawMode = fk_DrawMode.LINEMODE;
            footCollider.LineColor = new fk_Color(1.0, 1.0, 0.0);
            footCollider.GlMoveTo(_pos.x, _pos.y - (wide / 2.0 + footWide / 2.0), _pos.z);
            footCollider.BMode = fk_BoundaryMode.OBB;
            footCollider.AdjustOBB();
            //win.Entry(footCollider);

            modelDistance = new fk_Vector();
            modelDistance = _pos - footCollider.Position;

            // 前後左右のあたり判定設定
            frontCollider = new fk_Model();
            backCollider = new fk_Model();
            rightCollider = new fk_Model();
            leftCollider = new fk_Model();
            setCollider(frontCollider, new fk_Vector(1.5, 1.0, footWide), new fk_Vector(0.0, 1.0, -1.0), new fk_Color(0.0, 1.0, 0.0));
            setCollider(backCollider, new fk_Vector(1.5, 1.0, footWide), new fk_Vector(0.0, 1.0, 1.0), new fk_Color(0.0, 0.0, 1.0));
            setCollider(rightCollider, new fk_Vector(footWide, 1.0, 1.5), new fk_Vector(1.0, 1.0, 0.0), new fk_Color(1.0, 0.0, 0.0));
            setCollider(leftCollider, new fk_Vector(footWide, 1.0, 1.5), new fk_Vector(-1.0, 1.0, 0.0), new fk_Color(1.0, 0.6, 1.0));

            other = new Floor();
            otherForShadow = new Floor();

            // 連続攻撃防止変数設定
            attackCount = attackCountSpan;

            // チャージ用変数設定
            chargePower = 0;

            // HP設定
            hp = 3;
            zanki = 3;

            // 軌跡モデル初期設定
            trajectoryModel = new fk_Model();
            line = new fk_Line();
            trajectoryModel.Shape = line;
            trajectoryModel.LineColor = new fk_Color(0.8, 0.8, 0.5, 0.1);
            trajectoryModel.LineWidth = 3.0;

            // 一括配列に格納
            models = new fk_Model[] { model, footCollider, frontCollider, backCollider, rightCollider, leftCollider, slinger.model };

            damageCountSpan = 1.0;
            damageCount = damageCountSpan;
        }

        public void update()
        {
            if (Program.isGameOver == false) actMove();
            actRotate();
            actAttack();
            actMoveFootonCollider();

            slinger.update();

            if (otherForShadow != null)
            {
                shadow.GlMoveTo(new fk_Vector(GetPos.x, otherForShadow.GetPos.y + otherForShadow.GetScale.y / 2.0 + 0.01, GetPos.z));
            }

            actHpAndZanki();
            damageEffect();
            if (Program.isGameOver == true)
            {
                model.GlTranslate(0.0, 0.1, 0.0);
            }
        }

        public void actMove()
        {
            moveDirection = new fk_Vector();
            bool w = win.GetKeyStatus('w', fk_SwitchStatus.PRESS);
            bool s = win.GetKeyStatus('s', fk_SwitchStatus.PRESS);
            bool a = win.GetKeyStatus('a', fk_SwitchStatus.PRESS);
            bool d = win.GetKeyStatus('d', fk_SwitchStatus.PRESS);

            if (w)
            {
                moveDirection.z = -1.0;
                slinger.move();
            }
            if (s)
            {
                moveDirection.z = 1.0;
                if (w) moveDirection.z = 0.0;
                slinger.move();
            }
            if (a)
            {
                moveDirection.x = -1.0;
                slinger.move();
            }
            if (d)
            {
                moveDirection.x = 1.0;
                if (a) moveDirection.x = 0.0;
                slinger.move();
            }

            actJump();

            // 斜め移動で移動距離が増え無いように
            moveDirection.Normalize();
            model.LoTranslate(moveDirection * speed);

            pos = model.Position;

            // 奈落へ落下処理
            if (GetPos.y <= -50.0)
            {
                model.GlMoveTo(0.0, 30.0, 0.0);
                //jumpPower = 0.0;
                if (Program.isClear == false)
                {
                    zanki--;
                }
                hp = 3;

                if (zanki < 0)
                {
                    // ゲームオーバー処理
                    SetGameOverPanel();
                    slinger.removeEye();
                }

                foreach (Enemy01 e in enemies01)
                {
                    e.isFind = false;
                }
                foreach (Enemy02 e in enemies02)
                {
                    e.isFind = false;
                }
            }
        }

        public void actJump()
        {
            if (win.GetKeyStatus(' ', fk_SwitchStatus.PRESS) && isJumping == false)
            {
                jumpPower = 0.2;
                isJumping = true;
            }
            if (win.GetKeyStatus(' ', fk_SwitchStatus.PRESS) && !Program.isHide)
            {
                jumpPower = 0.2;
                isJumping = true;
            }
            //Console.WriteLine("isJumping = {0}, jumpPower = {1}", isJumping, jumpPower);

            if (isJumping == true)
            {
                model.GlTranslate(new fk_Vector(0.0, jumpPower, 0.0));
                jumpPower -= 9.8 * 0.001;
                //Console.WriteLine("jumpPower = {0}", jumpPower);
                // 抜け防止の下限設定
                if (jumpPower < -1.0)
                {
                    jumpPower = -1.0;
                }
            }

            // 床判定
            other = null;

            foreach (Floor f in floors)
            {
                if (footCollider.IsInter(f.model))
                {
                    if (isJumping == true && jumpPower < 0)
                    {
                        jumpPower = 0.0;
                        isJumping = false;
                        var p = new fk_Vector(GetPos);
                        model.GlMoveTo(p.x, f.GetPos.y + f.GetScale.y / 2.0 + footWide + wide / 2.0, p.z);
                    }
                    other = f;
                    otherForShadow = f;
                }
            }

            // 何も触れていなければ、落下。慣性処理
            if (other == null)
            {
                isJumping = true;

                // 慣性をゼロに近づける
                if (inertiaTime >= 0 && inertiaTime <= 1)
                {
                    // じわじわと慣性の力を低減していく。分母で調整
                    inertiaTime += 1.0 / 1000.0;
                }

                // 慣性の力を軽減しない場合、jumpInertia = 1.0 * jumpInertia;でも代用可能
                jumpInertia = (1.0 - inertiaTime) * jumpInertia;
                //Console.WriteLine("Inertia = {0}", jumpInertia);
            }
            else
            {
                // 触れている床が動く床ならプレイヤーも動かす
                // 慣性に床の動きを代入
                jumpInertia = other.GetVel;
                inertiaTime = 0;
            }
            // 床に合わせて動く
            actMoveFloor(jumpInertia);
        }

        // 回転処理
        public void actRotate()
        {
            angle.h = camera.Angle.h;
            model.GlAngle(angle);
        }

        // 攻撃処理
        public void actAttack()
        {
            offset = model.Position - offset;

            var vec = camera.Vec + new fk_Vector(offset.x, 0.2 + offset.y * 0.2, offset.z);
            vec.Normalize();

            // 左クリックを押している間、チャージ
            line.AllClear();
            if (win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.PRESS, true))
            {
                chargePower += chargeSpeed / 60.0;
                // 最大値を超えないように制御
                if (chargePower > chargePower_max)
                {
                    chargePower = chargePower_max;
                }
                slinger.biggerEye(chargePower);

                // 軌跡表示
                var trajeVec = new fk_Vector[2];
                trajeVec[0] = new fk_Vector(slinger.point.InhPosition);
                trajeVec[1] = new fk_Vector(slinger.point.InhPosition + 0.3 * (vec * (1.0 + chargePower)));

                var vec02 = new fk_Vector[2];
                vec02[0] = new fk_Vector(vec * (1.0 + chargePower));
                vec02[1] = new fk_Vector(vec * (1.0 + chargePower)) - new fk_Vector(0.0, 0.005, 0.0);

                // 右ベクトル（外積から計算）
                var rightVec = 0.1 * (camera.InhVec ^ camera.InhUpvec);

                for (int i = 0; i < 70; i++)
                {
                    //失敗例
                    //{
                    //    真ん中の軌跡
                    //   trajeVec[0] = new fk_Vector(i * 0.3 * (vec * (1.0 + chargePower) - new fk_Vector(0.0, 0.005, 0.0) * i)) + slinger.point.InhPosition;
                    //    trajeVec[1] = new fk_Vector((i + 1) * 0.3 * (vec * (1.0 + chargePower) - new fk_Vector(0.0, 0.005, 0.0) * (i + 1))) + slinger.point.InhPosition;
                    //    line.PushLine(trajeVec);

                    //    // 右の軌跡
                    //    line.PushLine(trajeVec[0] + rightVec, trajeVec[1] - rightVec);

                    //    // 左の軌跡
                    //    line.PushLine(trajeVec[0] - rightVec, trajeVec[1] + rightVec);
                    //    line.PushLine(trajeVec[0] + rightVec, trajeVec[1] + rightVec);
                    //    line.PushLine(trajeVec[0] - rightVec, trajeVec[1] - rightVec);

                    //    line.PushLine(trajeVec);
                    //}

                    // 真ん中の軌跡
                    //line.PushLine(trajeVec[0] + rightVec, trajeVec[1] + rightVec);
                    //line.PushLine(trajeVec[0] - rightVec, trajeVec[1] - rightVec);

                    // クロス状軌跡
                    line.PushLine(trajeVec[0] + rightVec, trajeVec[1] - rightVec);
                    line.PushLine(trajeVec[0] - rightVec, trajeVec[1] + rightVec);

                    vec02[0].y -= 0.005;
                    vec02[1].y -= 0.005;

                    trajeVec[0] += 0.3 * vec02[0];
                    trajeVec[1] += 0.3 * vec02[1];

                    scene.EntryModel(trajectoryModel);
                }
            }

            // 左クリックを離したときに、弾発射
            if (win.GetSpecialKeyStatus(fk_SpecialKey.CTRL_L, fk_SwitchStatus.PRESS) || win.GetMouseStatus(fk_MouseButton.MOUSE1, fk_SwitchStatus.UP, true))
            {
                if (attackCount == attackCountSpan) shots.Add(new Shot(slinger.point.InhPosition, vec * (1.0 + chargePower), enemies01, enemies02, enemies03, floors, scene, win));
                attackCount -= 0.0001;
                chargePower = 0;
                slinger.biggerEye(chargePower);
            }
            if (attackCount < attackCountSpan)
            {
                attackCount -= 1.0 / 60.0;
                if (attackCount <= 0)
                {
                    attackCount = attackCountSpan;
                }
            }
            //Console.WriteLine(offset);

            offset = model.Position;
        }

        // 移動床によって移動処理
        public void actMoveFloor(fk_Vector _v)
        {
            model.GlTranslate(_v);
        }

        // 基準となる、足元のあたり判定の位置補正処理
        public void actMoveFootonCollider()
        {
            footCollider.GlMoveTo(GetPos - modelDistance);
            actonCollider();
        }

        // 前後左右のあたり判定
        public void actonCollider()
        {
            foreach (Floor f in floors)
            {
                var p = new fk_Vector(GetPos);
                // 前面
                if (frontCollider.IsInter(f.model))
                {
                    model.GlMoveTo(p.x, p.y, f.GetPos.z + f.GetScale.z / 2.0 + footWide / 2.0 + 1.0);
                }
                // 背面
                if (backCollider.IsInter(f.model))
                {
                    model.GlMoveTo(p.x, p.y, f.GetPos.z - (f.GetScale.z / 2.0 + footWide / 2.0 + 1.0));
                }
                // 右側
                if (rightCollider.IsInter(f.model))
                {
                    model.GlMoveTo(f.GetPos.x - (f.GetScale.x / 2.0 + footWide / 2.0 + 1.0), p.y, p.z);
                }
                // 左側
                if (leftCollider.IsInter(f.model))
                {
                    model.GlMoveTo(f.GetPos.x + f.GetScale.x / 2.0 + footWide / 2.0 + 1.0, p.y, p.z);
                }
            }
        }

        // HP関係処理
        public void actHpAndZanki()
        {
            if (hp <= 0)
            {
                zanki--;
                if (zanki < 0)
                {
                    // ゲームオーバー処理
                    SetGameOverPanel();
                    slinger.removeEye();
                }
                else
                {
                    hp = 3;
                    model.GlMoveTo(0.0, 30.0, 0.0);
                }

                foreach (Enemy01 e in enemies01)
                {
                    e.isFind = false;
                }
                foreach (Enemy02 e in enemies02)
                {
                    e.isFind = false;
                }
            }
        }

        // ダメージ処理
        public void damage(int _val)
        {
            if (Program.isClear == false && damageCount == damageCountSpan)
            {
                hp--;
                //Console.WriteLine("hp = {0}", hp);
                damageCount -= 0.0001;
                Program.se.StartSE(2);
            }
        }

        // ダメージを受けたときに、マテリアルを変化させる処理
        public void damageEffect()
        {
            if (damageCount < damageCountSpan)
            {
                setDamageColor(fk_Material.Red, fk_Material.DarkRed);
                damageCount -= 1.0 / 60.0;
                if (damageCount <= 0)
                {
                    setDefaultColor();
                    damageCount = damageCountSpan;
                }
            }
        }

        // あたり判定設定用メソッド
        void setCollider(fk_Model _model, fk_Vector _block, fk_Vector _glMoveTo, fk_Color _col)
        {
            _model.Shape = new fk_Block(_block.x, _block.y, _block.z);
            _model.Material = fk_Material.Blue;
            _model.DrawMode = fk_DrawMode.LINEMODE;
            _model.LineColor = _col;
            _model.GlMoveTo(_glMoveTo);
            _model.Parent = footCollider;
            _model.BMode = fk_BoundaryMode.OBB;
            _model.AdjustOBB();
            //scene.EntryModel(_model);
        }

        // ポジション取得用プロパティ
        public fk_Vector GetPos
        {
            get
            {
                return model.Position;
            }
        }

        // モデル取得用プロパティ
        public fk_Model GetModel
        {
            get
            {
                return model;
            }
        }

        // カメラモデル取得用プロパティ
        public fk_Model GetCamera
        {
            get
            {
                return camera;
            }
        }

        // カメラのポジション取得用プロパティ
        public fk_Vector GetCameraPos
        {
            get
            {
                return camera.InhPosition;
            }
        }

        public void setEyeColor(fk_Material _mat)
        {
            slinger.blackEyeL.Material = _mat;
            slinger.blackEyeR.Material = _mat;
        }

        public void setDamageColor(fk_Material _mat01, fk_Material _mat02)
        {
            slinger.eyeL.Material = _mat01;
            slinger.eyeR.Material = _mat01;
            slinger.blackEyeL.Material = _mat02;
            slinger.blackEyeR.Material = _mat02;
        }

        public void setDefaultColor()
        {
            slinger.eyeL.Material = fk_Material.White;
            slinger.eyeR.Material = fk_Material.White;
            slinger.blackEyeL.Material = fk_Material.MatBlack;
            slinger.blackEyeR.Material = fk_Material.MatBlack;
        }

        public void SetClearPanel()
        {
            //scene.EntryModel(clearPanel);
            scene.EntryOverlayModel(clearPanel);
            Program.isClear = true;
        }

        public void SetGameOverPanel()
        {
            scene.EntryOverlayModel(gameOverPanel);
            Program.isGameOver = true;
            hp = 0;
        }

        public void DeleteShadow(fk_Scene _scene)
        {
            _scene.RemoveModel(shadow);
        }

        public bool setCheatJump
        {
            set
            {
                isJumping = value;
            }
        }

        public fk_Vector GetVec
        {
            get
            {
                return camera.Vec;
            }
        }

        public void DeleteTrajectoryModel()
        {
            scene.RemoveModel(trajectoryModel);
        }
    }

    class Slinger
    {
        public fk_Model model;

        public fk_Model eyeR;
        public fk_Model eyeL;
        public fk_Model blackEyeR;
        public fk_Model blackEyeL;
        private fk_Vector blackEyeV;
        private double blackEyeCount;

        public fk_Vector pos;

        public fk_Model point;

        fk_Scene scene;
        fk_AppWindow win;

        public fk_Model[] models;

        public Slinger(fk_Vector _pos, fk_Scene _scene, fk_AppWindow _win)
        {
            pos = _pos;
            scene = _scene;
            win = _win;

            model = new fk_Model();
            model.Shape = new fk_Prism(32, 0.1, 0.1, 1.0);
            model.Material = fk_Material.LightGreen;
            model.GlFocus(0, 1, 0);
            model.GlMoveTo(pos);
            scene.EntryModel(model);

            var rightArm = new fk_Model();
            rightArm.Shape = new fk_Prism(16, 0.06, 0.08, 0.3);
            rightArm.Material = fk_Material.IridescentGreen;
            rightArm.GlFocus(1.0, 0.0, -0.7);
            rightArm.GlMoveTo(0.05, 0.0, -0.8);
            scene.EntryModel(rightArm);

            var rightArmTop = new fk_Model();
            rightArmTop.Shape = new fk_Prism(16, 0.06, 0.06, 0.5);
            rightArmTop.Material = fk_Material.IridescentGreen;
            rightArmTop.GlFocus(0.1, 0.0, -1.0);
            rightArmTop.GlMoveTo(0.27, 0.0, -0.93);
            scene.EntryModel(rightArmTop);

            var rightHand = new fk_Model();
            rightHand.Shape = new fk_Sphere(8, 0.08);
            rightHand.Material = fk_Material.LightGreen;
            rightHand.GlMoveTo(0.32, 0.0, -1.4);
            scene.EntryModel(rightHand);

            var leftHand = new fk_Model();
            leftHand.Shape = new fk_Sphere(8, 0.08);
            leftHand.Material = fk_Material.LightGreen;
            leftHand.GlMoveTo(-0.32, 0.0, -1.4);
            scene.EntryModel(leftHand);

            var leftArm = new fk_Model();
            leftArm.Shape = new fk_Prism(16, 0.06, 0.08, 0.3);
            leftArm.Material = fk_Material.IridescentGreen;
            leftArm.GlFocus(-1.0, 0.0, -0.7);
            leftArm.GlMoveTo(-0.05, 0.0, -0.8);
            scene.EntryModel(leftArm);

            var leftArmTop = new fk_Model();
            leftArmTop.Shape = new fk_Prism(16, 0.06, 0.06, 0.5);
            leftArmTop.Material = fk_Material.IridescentGreen;
            leftArmTop.GlFocus(-0.1, 0.0, -1.0);
            leftArmTop.GlMoveTo(-0.27, 0.0, -0.93);
            scene.EntryModel(leftArmTop);

            var head = new fk_Model();
            head.Shape = new fk_Sphere(32, 0.15);
            head.Material = fk_Material.Green;
            head.GlMoveTo(0.0, 0.0, -1.0);
            scene.EntryModel(head);

            var nose = new fk_Model();
            nose.Shape = new fk_Prism(8, 0.01, 0.01, 0.1);
            nose.Material = fk_Material.Red;
            nose.GlFocus(0.0, 1.0, 0.0);
            nose.GlMoveTo(0.0, 0.1, -1.0);
            scene.EntryModel(nose);

            eyeR = new fk_Model();
            eyeR.Shape = new fk_Prism(16, 0.03, 0.03, 0.01);
            eyeR.Material = fk_Material.White;
            eyeR.GlFocus(0.0, 1.0, 0.0);
            eyeR.GlMoveTo(-0.05, 0.15, -1.05);
            scene.EntryModel(eyeR);

            eyeL = new fk_Model();
            eyeL.Shape = new fk_Prism(16, 0.03, 0.03, 0.01);
            eyeL.Material = fk_Material.White;
            eyeL.GlFocus(0.0, 1.0, 0.0);
            eyeL.GlMoveTo(0.05, 0.15, -1.05);
            scene.EntryModel(eyeL);

            blackEyeR = new fk_Model();
            blackEyeR.Shape = new fk_Prism(16, 0.01, 0.01, 0.02);
            blackEyeR.Material = fk_Material.MatBlack;
            blackEyeR.GlMoveTo(-0.01, -0.015, -0.01);

            blackEyeL = new fk_Model();
            blackEyeL.Shape = new fk_Prism(16, 0.01, 0.01, 0.02);
            blackEyeL.Material = fk_Material.MatBlack;
            blackEyeL.GlMoveTo(-0.01, -0.015, -0.01);

            blackEyeV = new fk_Vector(0.0003, 0.0, 0.0);

            point = new fk_Model();
            point.Shape = new fk_Sphere(4, 0.1);
            var mat = new fk_Material();
            mat.Alpha = 0.5f;
            point.Material = mat;
            point.GlMoveTo(0.0, 0.0, -1.3);
            //scene.EntryModel(point);

            rightArm.Parent = model;
            rightArmTop.Parent = model;
            rightHand.Parent = model;
            leftArm.Parent = model;
            leftArmTop.Parent = model;
            leftHand.Parent = model;
            head.Parent = model;
            nose.Parent = model;
            eyeR.Parent = model;
            eyeL.Parent = model;
            blackEyeR.Parent = eyeR;
            blackEyeL.Parent = eyeL;
            point.Parent = model;

            scene.EntryModel(blackEyeL);
            scene.EntryModel(blackEyeR);

            models = new fk_Model[] { model, nose, head, rightArm, rightArmTop, rightHand, leftArm, leftArmTop, leftHand, eyeL, eyeR, blackEyeL, blackEyeR };
        }

        public void update()
        {
            //move();
        }

        public void move()
        {
            blackEyeR.GlTranslate(blackEyeV);
            blackEyeL.GlTranslate(blackEyeV);
            blackEyeCount += 1.0 / 60.0;
            if (blackEyeCount >= 1.0)
            {
                blackEyeCount = 0;
                blackEyeV.x *= -1;
            }
        }

        public void biggerEye(double _magni)
        {
            _magni += blackEyeL.Scale;
            eyeL.SetScale(_magni, _magni, _magni);
            eyeR.SetScale(_magni, _magni, _magni);
        }

        public void removeEye()
        {
            win.Remove(blackEyeL);
            win.Remove(blackEyeR);
        }
    }
}
