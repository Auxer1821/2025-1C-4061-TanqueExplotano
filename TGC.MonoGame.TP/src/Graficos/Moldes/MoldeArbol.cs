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
        private float _timer = 0f;
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

            this._efecto.Parameters["ambientColor"].SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"].SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"].SetValue(Color.Transparent.ToVector3());
            this._efecto.Parameters["KAmbient"].SetValue(0.5f);
            this._efecto.Parameters["KDiffuse"].SetValue(0.8f);
            this._efecto.Parameters["KSpecular"].SetValue(0.2f);
            this._efecto.Parameters["shininess"].SetValue(1.0f);
            this._efecto.Parameters["lightPosition"].SetValue(new Vector3(0.0f, 300.0f, 0.0f));

            foreach (var mesh in _modelo.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _efecto;
                }
            }
        }
        public override void Draw(Matrix Mundo, GraphicsDevice Graphics)
        {
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
                _efecto.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(Mundo)));
                mesh.Draw();
            }
        }
        public override void setTime(GameTime time)
        {
            // Aquí podrías actualizar parámetros relacionados con el tiempo si es necesario
            // Por ejemplo, podrías modificar la velocidad del viento o la fuerza del viento en función del tiempo
            _efecto.Parameters["Time"].SetValue((float)time.TotalGameTime.TotalSeconds);
            
            _timer += (float)time.ElapsedGameTime.TotalSeconds;
            var lightPosition = new Vector3((float)Math.Cos(_timer) * 700f, 800f, (float)Math.Sin(_timer) * 700f);
            _efecto.Parameters["lightPosition"].SetValue(lightPosition);
        }

        //borrar despues de implementar a todos los moldes
        public override void setCamara(Vector3 posicion)
        {
            _efecto.Parameters["eyePosition"].SetValue(posicion);
            
            //_efecto.Parameters["lightPosition"].SetValue(posicion + new Vector3(0, 10, 0));
        }
        public override void SetPosSOL(Vector3 posicion){
            _efecto.Parameters["lightPosition"].SetValue(posicion);
        }


    }
}