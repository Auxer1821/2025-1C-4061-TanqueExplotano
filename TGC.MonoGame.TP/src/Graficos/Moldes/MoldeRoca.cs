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
    ///     Esta es la clase donde se guarda el  modelo; efecto y otras cosas que comparten las entidades iguale para dibujar 
    /// </summary>
    public class MoldeRoca : IMolde
    {
        private Model _modelo;
        Texture2D _rocaTexture;
        public MoldeRoca(ContentManager Content)
        {
            this._modelo = Content.Load<Model>(@"Models/Stone/Stone");
            this._efecto = Content.Load<Effect>(@"Effects/shaderRoca");
            this._rocaTexture = Content.Load<Texture2D>(@"Models/Stone/roca3");
            this._efecto.Parameters["Texture"].SetValue(_rocaTexture);

            this._efecto.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["KAmbient"]?.SetValue(0.7f);
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

        public override void Draw(Matrix mundo, GraphicsDevice Graphics)
        {
            _efecto.Parameters["World"].SetValue(mundo);
            

            foreach (var mesh in _modelo.Meshes)
            {
                Matrix MundoShader = mesh.ParentBone.Transform * mundo;
                _efecto.Parameters["World"].SetValue(MundoShader);
                _efecto.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(MundoShader)));
                mesh.Draw();
            }
        }
    



    }
}