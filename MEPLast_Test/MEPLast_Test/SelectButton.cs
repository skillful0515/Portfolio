using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace MEPLast_Test
{
    class SelectButton
    {
        public fk_Model model;
        public fk_Model[] textureModel = new fk_Model[2];
        private fk_RectTexture texture;
        private string correspondenceScene;
        private fk_Scene scene;
        private fk_AppWindow win;

        public SelectButton(fk_Vector _pos, fk_Vector _scale, string _corresScene, fk_Scene _scene, fk_Image _image, fk_AppWindow _win)
        {
            model = new fk_Model();
            model.Shape = new fk_Block(_scale.x, _scale.y, _scale.z);
            model.Material = fk_Material.White;
            model.GlMoveTo(_pos);
            model.BMode = fk_BoundaryMode.OBB;
            model.AdjustOBB();

            
            texture = new fk_RectTexture();
            texture.Image = _image;
            texture.TextureSize = new fk_TexCoord(_scale.x, _scale.y);
            texture.RepeatParam = new fk_TexCoord(1.0, 1.0);

            for (int i = 0; i < textureModel.Length; i++)
            {
                textureModel[i] = new fk_Model();
                textureModel[i].Shape = texture;
                textureModel[i].SetParent(model);
            }

            textureModel[0].GlMoveTo(0.0, 0.0, _scale.z / 2.0);
            textureModel[1].GlMoveTo(0.0, 0.0, -_scale.z / 2.0);
            textureModel[0].GlFocus(new fk_Vector(0.0, 0.0, 0.0));
            textureModel[1].GlFocus(new fk_Vector(0.0, 0.0, 0.0));

            correspondenceScene = _corresScene;
            scene = _scene;
            win = _win;

            scene.EntryModel(textureModel[0]);
            scene.EntryModel(textureModel[1]);
        }

        public void OnCollider()
        {
            if (Program.sceneState == Program.sceneStateCTRL.STAGESELECT)
            {
                Program.setupTrigger(correspondenceScene);
            }
        }
    }
}
