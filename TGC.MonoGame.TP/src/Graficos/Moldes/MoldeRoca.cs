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
        public MoldeRoca(ContentManager Content) {
            this._modelo = Content.Load<Model>(@"Models/Stone/Stone");
            this._efecto = Content.Load<Effect>(@"Effects/shaderRoca");
            this._rocaTexture = Content.Load<Texture2D>(@"Models/Stone/roca3");
            this._efecto.Parameters["Texture"].SetValue(_rocaTexture);
        }
        public override void Draw(Matrix mundo, GraphicsDevice Graphics){
            _efecto.Parameters["World"].SetValue(mundo);

            foreach (var mesh in _modelo.Meshes)
            {
                _efecto.Parameters["World"].SetValue(mesh.ParentBone.Transform * mundo);
                mesh.Draw();
            }
        }



    }
}