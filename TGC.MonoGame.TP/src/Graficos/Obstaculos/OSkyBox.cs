using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.SkyBox
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    
    //cambio de objeto a modelo, para poder usar el modelo de arbol
    public class OSkyBox : Modelos.Modelo
    {
        
        // Variables
        Texture2D skyBoxTexture;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public OSkyBox(){}
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content)
        {
            this._Color = Color.LightBlue.ToVector3();
            base.Initialize(Graphics, Mundo, Content);
        }

        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/shaderSkyBox");
        }

        protected override void ConfigurarModelo(ContentManager Content){
            this._modelo = Content.Load<Model>(@"Models/skybox/skybox");
            skyBoxTexture = Content.Load<Texture2D>(@"Models/skybox/skyTexture");
            // Setear la textura del skybox
            _effect2.Parameters["Texture"].SetValue(skyBoxTexture);
        }
        protected override void AjustarModelo(){
            _matixBase = Matrix.CreateScale(1500.0f) * Matrix.CreateRotationX(MathHelper.Pi + MathHelper.PiOver2) * Matrix.CreateTranslation(Vector3.UnitY * 10);
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//

        public override void Dibujar(GraphicsDevice Graphics)
        {
            // Guarda la configuración actual del depth buffer
            var oldDepthStencilState = Graphics.DepthStencilState;

            // Configura para dibujar el skybox detrás de todo
            Graphics.DepthStencilState = DepthStencilState.None;

            _effect2.Parameters["World"].SetValue(_matrixMundo);

            //Dibujar el modelo
            foreach (var mesh in _modelo.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _effect2;
                }
                mesh.Draw();
            }
            // Restaura el depth buffer
            Graphics.DepthStencilState = oldDepthStencilState;
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        

        //Configuración de efectos tomados desde la clase padre
        
    }
}