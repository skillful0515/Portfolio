using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class GoalPoint
    {
        fk_Model model;
        fk_Scene scene;
        fk_AppWindow win;

        Player player;

        fk_Model collider;

        public GoalPoint(fk_Vector _pos, Player _player, fk_Scene _scene, fk_AppWindow _win)
        {
            player = _player;
            scene = _scene;
            win = _win;

            model = new fk_Model();
            model.Shape = new fk_Sphere(2, 1.0);
            model.Material = fk_Material.LavaRed;
            model.GlMoveTo(_pos);

            collider = new fk_Model();
            collider.Shape = new fk_Block(5.0, 1.0, 5.0);
            collider.GlMoveTo(_pos);
            collider.BMode = fk_BoundaryMode.OBB;
            collider.AdjustOBB();

            scene.EntryModel(model);
        }

        public void update()
        {
            onCollider();
        }

        void onCollider()
        {
            if (collider.IsInter(player.GetModel))
            {
                player.setEyeColor(fk_Material.LightBlue);
                player.SetClearPanel();
            }
        }
    }
}
