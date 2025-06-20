using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.Modelos;



namespace TGC.MonoGame.TP.src.Moldes
{
    /// <summary>
    ///     Molde con los datos para la casa. 
    /// </summary>
    public class MoldeCasa : IMolde
    {
        private Model _modelo;
        Texture2D _paredTexture;
        Texture2D _techoTexture;
        Texture2D _chimeneaTexture;
        Texture2D _marcoTexture;
        public MoldeCasa(ContentManager Content)
        {
            _efecto = Content.Load<Effect>(@"Effects/shaderCasa");
            _modelo = Content.Load<Model>(@"Models/house/cartoon_house1");
            _chimeneaTexture = Content.Load<Texture2D>(@"Models/house/paredPiedra");
            _paredTexture = Content.Load<Texture2D>(@"Models/house/textura-roja");
            _techoTexture = Content.Load<Texture2D>(@"Models/house/techo2");
            _marcoTexture = Content.Load<Texture2D>(@"Models/house/tablasMadera");

            _efecto.Parameters["TextureChimenea"].SetValue(_chimeneaTexture);
            _efecto.Parameters["TexturePared"].SetValue(_paredTexture);
            _efecto.Parameters["TextureTecho"].SetValue(_techoTexture);
            _efecto.Parameters["TextureVentana"].SetValue(_marcoTexture);

            this._efecto.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"]?.SetValue(Color.Transparent.ToVector3());
            this._efecto.Parameters["KAmbient"]?.SetValue(0.5f);
            this._efecto.Parameters["KDiffuse"]?.SetValue(0.8f);
            this._efecto.Parameters["KSpecular"]?.SetValue(0.2f);
            this._efecto.Parameters["shininess"]?.SetValue(1.0f);

            foreach (var mesh in _modelo.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _efecto;
                }
            }
        }
        public override void Draw(Matrix mundo, GraphicsDevice graphics){
            _efecto.Parameters["World"].SetValue(mundo);
            _efecto.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(mundo)));
            foreach (var mesh in _modelo.Meshes)
            {
                if (mesh.Name.Contains("Roof"))
                {
                    _efecto.CurrentTechnique = _efecto.Techniques["Techo"];
                }
                else if (mesh.Name.Contains("Window"))
                {
                    _efecto.CurrentTechnique = _efecto.Techniques["Ventana"];
                }
                else if (mesh.Name.Contains("Cummny"))
                {
                    _efecto.CurrentTechnique = _efecto.Techniques["Chimenea"];
                }
                else if (mesh.Name.Contains("Wall"))
                {
                    _efecto.CurrentTechnique = _efecto.Techniques["Pared"];
                }
                _efecto.Parameters["World"].SetValue(mesh.ParentBone.Transform * mundo);
                mesh.Draw();
            }
        }

    }
}