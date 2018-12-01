using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class Floor
    {
        public fk_Model model;
        private fk_Model[] textureModel = new fk_Model[6];
        private fk_RectTexture[] texture = new fk_RectTexture[6];

        private fk_Vector scale;
        private fk_Vector vel;
        private int velId;
        private int count = 0;
        private bool isMove;
        private bool isWoodenBox;
        private bool del;
        private double delTime;
        private double speed = 1.0;
        private int countLimit = 120;

        private int boxHp;
        private int boxHpMax;


        private fk_Scene scene;
        private fk_AppWindow win;

        public Floor()
        {

        }
        public Floor(fk_Vector _pos, fk_Vector _scale, fk_Scene _scene, fk_AppWindow _win)
        {
            model = new fk_Model();
            model.Shape = new fk_Block(_scale.x, _scale.y, _scale.z);
            model.Material = fk_Material.White;
            //model.DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;
            model.GlMoveTo(_pos);
            model.BMode = fk_BoundaryMode.OBB;
            model.AdjustOBB();

            setUpTexture(_pos, _scale);

            scale = _scale;
            vel = new fk_Vector();
            isMove = false;
            isWoodenBox = false;
            win = _win;
            del = false;
            delTime = 5.0;

            scene = _scene;
            //scene.EntryModel(model);
            for (int i = 0; i < textureModel.Length; i++)
            {
                scene.EntryModel(textureModel[i]);
            }
        }
        public Floor(fk_Vector _pos, fk_Vector _scale, fk_Scene _scene, fk_AppWindow _win, fk_Material _mat) : this(_pos, _scale, _scene, _win)
        {
            model.Material = _mat;
        }
        public Floor(fk_Vector _pos, fk_Vector _scale, int _velId, fk_Scene _scene, fk_AppWindow _win) : this(_pos, _scale, _scene, _win)
        {
            velId = _velId;
            setVel();
            count = 0;
            isMove = true;
        }
        public Floor(fk_Vector _pos, fk_Vector _scale, int _velId, fk_Scene _scene, fk_AppWindow _win, fk_Material _mat) : this(_pos, _scale, _velId, _scene, _win)
        {
            model.Material = _mat;
        }

        void setUpTexture(fk_Vector _pos, fk_Vector _scale)
        {
            // テクスチャ設定
            for (int i = 0; i < textureModel.Length; i++)
            {
                textureModel[i] = new fk_Model();
                texture[i] = new fk_RectTexture();
                textureModel[i].Shape = texture[i];
                textureModel[i].SetParent(model);
                texture[i].Image = Program.floorImage;
                switch (i)
                {
                    case 0:
                    case 1:
                        texture[i].TextureSize = new fk_TexCoord(_scale.x, _scale.z);
                        texture[i].RepeatParam = new fk_TexCoord(_scale.x / 1.0, _scale.z / 1.0);
                        break;
                    case 2:
                    case 3:
                        texture[i].TextureSize = new fk_TexCoord(_scale.z, _scale.y);
                        texture[i].RepeatParam = new fk_TexCoord(_scale.z / 1.0, _scale.y / 1.0);
                        break;
                    case 4:
                    case 5:
                        texture[i].TextureSize = new fk_TexCoord(_scale.x, _scale.y);
                        texture[i].RepeatParam = new fk_TexCoord(_scale.x / 1.0, _scale.y / 1.0);
                        break;
                }
                texture[i].RepeatMode = true;
            }

            textureModel[0].GlMoveTo(0.0, _scale.y / 2.0, 0.0);
            textureModel[1].GlMoveTo(0.0, -(_scale.y / 2.0), 0.0);
            textureModel[2].GlMoveTo(_scale.x / 2.0, 0.0, 0.0);
            textureModel[3].GlMoveTo(-(_scale.x / 2.0), 0.0, 0.0);
            textureModel[4].GlMoveTo(0.0, 0.0, _scale.z / 2.0);
            textureModel[5].GlMoveTo(0.0, 0.0, -(_scale.z / 2.0));

            textureModel[0].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
            textureModel[1].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
            textureModel[2].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
            textureModel[3].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
            textureModel[4].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
            textureModel[5].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
        }

        public void setTextuteImage(fk_Image _image)
        {
            for (int i = 0; i < texture.Length; i++)
            {
                texture[i].Image = _image;
            }
        }
        public void setTextuteImage_Wooden(int _hp)
        {
            for (int i = 0; i < texture.Length; i++)
            {
                texture[i].Image = Program.woodenBoxImage[0];
                texture[i].RepeatParam = new fk_TexCoord(1.0, 1.0);
            }

            boxHpMax = _hp;
            boxHp = _hp;

            isWoodenBox = true;
        }

        public void setTextureImage_breakWooden(fk_Image _image)
        {
            for (int i = 0; i < texture.Length; i++)
            {
                texture[i].Image = _image;
                texture[i].RepeatParam = new fk_TexCoord(1.0, 1.0);
            }
        }

        void setVel()
        {
            switch (velId)
            {
                case -6:
                    vel = new fk_Vector(0.0, -0.1, 0.0);
                    break;
                case -5:
                    vel = new fk_Vector(0.0, 0.0, -0.1);
                    break;
                case -4:
                    vel = new fk_Vector(-0.1, 0.0, 0.0);
                    break;
                case -3:
                    vel = new fk_Vector(0.1, 0.0, 0.0);
                    break;
                case -2:
                    vel = new fk_Vector(0.0, 0.0, 0.1);
                    break;
                case -1:
                    vel = new fk_Vector(0.0, 0.1, 0.0);
                    break;
                case 1:
                    vel = new fk_Vector(0.1, 0.0, 0.0);
                    break;
                case 2:
                    vel = new fk_Vector(0.0, 0.1, 0.0);
                    break;
                case 3:
                    vel = new fk_Vector(0.1, 0.0, 0.0);
                    break;
                case 4:
                    vel = new fk_Vector(0.0, 0.0, -0.1);
                    break;
                default:
                    break;
            }
        }

        public void update()
        {
            if (isMove == true)
            {
                move();
            }
            if (isWoodenBox == true)
            {
                actBreak();
            }
        }

        public void move()
        {
            count++;
            model.GlTranslate(vel * speed);
            switch (velId)
            {
                case -6:
                case -5:
                case -4:
                case -3:
                case -2:
                case -1:
                    delTime -= 1.0 / 60.0;
                    if (delTime <= 0)
                    {
                        del = true;
                        for (int i = 0; i < textureModel.Length; i++)
                        {
                            win.Remove(textureModel[i]);
                        }
                    }
                    break;
                case 1:
                    if (count >= countLimit)
                    {
                        vel.x *= -1;
                        count = 0;
                    }
                    break;
                case 2:
                    if (count >= countLimit)
                    {
                        vel.y *= -1;
                        count = 0;
                    }
                    break;
                case 3:
                    vel.Set(0.1 * Math.Cos(-count * FK.PI / 180.0), 0.1 * Math.Sin(-count * FK.PI / 180.0), 0.0);
                    break;
                case 4:
                    if (count >= countLimit)
                    {
                        vel.z *= -1;
                        count = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        public void actBreak()
        {

        }

        public void actDamage()
        {
            if (isWoodenBox == true)
            {
                boxHp--;
                if (boxHp <= 0)
                {
                    Program.addScore(500);
                    DeleteTexture(scene);
                    del = true;
                    Effect_Bomb ef = new Effect_Bomb(GetPos, scene, 6);
                    Program.effectBombs.Add(ef);
                    ef.setMaterial(fk_Material.Flesh);
                    ef.setDefaultPosition(GetScale.x / 2.0);
                    return;
                }
                Program.addScore(10);

                // 効果をつける
                //Program.se.StartSE(5);
                Effect_Bomb e = new Effect_Bomb(GetPos, scene, 5);
                Program.effectBombs.Add(e);
                e.setModelLength(10);
                e.setSize(0.05);
                e.setMaterial(fk_Material.Flesh);
                e.setDelCountSpan(30);
                e.setDefaultPosition(GetScale.x / 1.0);

                // テクスチャ変更
                double rate = boxHp / (double)boxHpMax;

                for (int i = 0; i < 6; i++)
                {
                    if (rate >= i / 7.0)
                    {
                        setTextureImage_breakWooden(Program.woodenBoxImage[(6 - i)]);
                    }
                }
                if (boxHp == boxHpMax - 1)
                {
                    setTextureImage_breakWooden(Program.woodenBoxImage[1]);
                }
            }
        }

        public fk_Vector GetPos
        {
            get
            {
                return model.Position;
            }
        }

        public fk_Vector GetScale
        {
            get
            {
                return scale;
            }
        }

        public fk_Vector GetVel
        {
            get
            {
                return vel * speed;
            }
        }

        public bool isDelete
        {
            get
            {
                return del;
            }
        }

        public void setSpeed(double _speed)
        {
            speed = _speed;
        }

        public void setLimit(int _limit)
        {
            countLimit = _limit;
        }

        public void reSetCount(int _newCount)
        {
            count = _newCount;
        }

        public void DeleteTexture(fk_Scene _scene)
        {
            for (int i = 0; i < textureModel.Length; i++)
            {
                _scene.RemoveModel(textureModel[i]);
            }
        }

        public bool GetIsWoodeBox
        {
            get
            {
                return isWoodenBox;
            }
        }
    }
}
