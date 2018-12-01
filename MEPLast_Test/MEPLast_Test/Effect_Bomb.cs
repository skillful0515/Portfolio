using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class Effect_Bomb
    {
        public fk_Model[] models = new fk_Model[50];
        private Random rand;
        private fk_Vector randomVec;
        private double speed;

        private int delCount;
        private int delCountSpan;
        private bool del;

        private fk_Scene scene;
        private fk_Vector pos;

        private double size;

        public Effect_Bomb()
        {

        }

        public Effect_Bomb(fk_Vector _pos, fk_Scene _scene, int _soundNum)
        {
            rand = new Random();

            size = 0.1;
            speed = 0.05;
            delCountSpan = 90;
            delCount = delCountSpan;
            del = false;

            scene = _scene;
            pos = _pos;

            for (int i = 0; i < models.Length; i++)
            {
                models[i] = new fk_Model();
                models[i].Shape = new fk_Sphere(1, size);
                models[i].Material = fk_Material.Red;
                //models[i].DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;
                models[i].GlMoveTo(pos);
                scene.EntryModel(models[i]);

                randomVec = new fk_Vector(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5, rand.NextDouble() - 0.5);
                randomVec.Normalize();

                models[i].LoFocus(randomVec);
            }

            // 爆発音設定
            if (_soundNum != -1)
            {
                double magnitude = (Program.player.GetPos - pos).Dist();
                double gain = Math.Max(0.1, 10.0 / magnitude);
                gain = Math.Min(1.0, gain);
                Program.se.SetGain(_soundNum, gain);
                Program.se.StartSE(_soundNum);
            }

            // 処理落ち防止
            if (Program.effectBombs.Count >= 3)
            {
                setModelLength(1);
            }
        }

        public void update()
        {
            move();
            delete();
        }

        public void move()
        {
            for (int i = 0; i < models.Length; i++)
            {
                models[i].LoTranslate(0.0, 0.0, speed);
            }
        }

        public void delete()
        {
            delCount--;
            for (int i = 0; i < models.Length; i++)
            {
                models[i].Shape = new fk_Sphere(1, (1.0 - ((delCountSpan - delCount) / (float)delCountSpan)) * size);
            }
            if (delCount <= 0)
            {
                del = true;
            }
        }

        public void setMaterial(fk_Material _mat)
        {
            for (int i = 0; i < models.Length; i++)
            {
                models[i].Material = _mat;
            }
        }

        public void setDefaultPosition(double _range)
        {
            for (int i = 0; i < models.Length; i++)
            {
                models[i].LoTranslate(0.0, 0.0, _range);
            }
        }

        public void setDelCountSpan(int _count)
        {
            delCountSpan = _count;
            delCount = delCountSpan;
        }

        public void setSize(double _size)
        {
            size = _size;
        }

        public void setSpeed(double _speed)
        {
            speed = _speed;
        }

        // 呼ぶのであれば一番最初に
        public void setModelLength(int _num)
        {
            // 元のモデルズをいったん削除
            for (int i = 0; i < models.Length; i++)
            {
                scene.RemoveModel(models[i]);
            }

            // 新たなモデルズを生成
            models = new fk_Model[_num];
            for (int i = 0; i < models.Length; i++)
            {
                models[i] = new fk_Model();
                models[i].Shape = new fk_Sphere(1, size);
                models[i].Material = fk_Material.Red;
                //models[i].DrawMode = fk_DrawMode.POLYMODE | fk_DrawMode.LINEMODE;
                models[i].GlMoveTo(pos);
                scene.EntryModel(models[i]);

                randomVec = new fk_Vector(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5, rand.NextDouble() - 0.5);
                randomVec.Normalize();

                models[i].LoFocus(randomVec);
            }
        }

        public bool isDelete
        {
            get
            {
                return del;
            }
        }

        public void setParent(fk_Model _parent)
        {
            for(int i = 0; i < models.Length; i++)
            {
                models[i].Parent = _parent;
            }
        }
    }
}
