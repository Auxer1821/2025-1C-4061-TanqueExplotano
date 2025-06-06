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
    public class MoldeArbol : IMolde
    {
        private Model _modelo;
        Texture2D troncoTexture;
        Texture2D hojasTexture;
        public MoldeArbol(ContentManager Content)
        {
            this._modelo = Content.Load<Model>(@"Models/tree/tree2");
            this._efecto = Content.Load<Effect>(@"Effects/shaderArbol");
            this.troncoTexture = Content.Load<Texture2D>(@"Models/tree/tronco2");
            this.hojasTexture = Content.Load<Texture2D>(@"Models/tree/light-green-texture");
            this._efecto.Parameters["TextureTronco"].SetValue(troncoTexture);
            this._efecto.Parameters["TextureHojas"].SetValue(hojasTexture);
            this._efecto.Parameters["WindStrength"].SetValue(0.3f);
            this._efecto.Parameters["WindSpeed"].SetValue(1.0f);
            this._efecto.Parameters["LeafFlexibility"].SetValue(0.3f);

            foreach (var mesh in _modelo.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _efecto;
                }
            }
        }
        public override void Draw(Matrix Mundo, GraphicsDevice Graphics){
            foreach (var mesh in _modelo.Meshes)
            {
                if (mesh.Name.Contains("Zyl")) //asi se llama la mesh de tronco
                {
                    _efecto.CurrentTechnique = _efecto.Techniques["Tronco"];
                }
                else
                {
                    _efecto.CurrentTechnique = _efecto.Techniques["Hojas"];
                }
                _efecto.Parameters["World"].SetValue(mesh.ParentBone.Transform * Mundo);
                mesh.Draw();
            }
        }
        public override void setTime(GameTime time)
        {
            // Aquí podrías actualizar parámetros relacionados con el tiempo si es necesario
            // Por ejemplo, podrías modificar la velocidad del viento o la fuerza del viento en función del tiempo
            _efecto.Parameters["Time"].SetValue((float)time.TotalGameTime.TotalSeconds);
        }

    }
}