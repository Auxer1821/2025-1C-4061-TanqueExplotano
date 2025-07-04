using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Rocas
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class ORoca : Modelos.Modelo
    {
        
        // Variables
        //Texture2D rocaTexture;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public ORoca(){}


        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content)
        {
            //this._Color = Color.Yellow.ToVector3();
            base.Initialize(Graphics, Mundo, Content);
        }

        protected override void ConfigurarModelo(ContentManager Content){
            //this._modelo = Content.Load<Model>(@"Models/Stone/Stone");
            //rocaTexture = Content.Load<Texture2D>(@"Models/Stone/roca3");
            // Setear la textura de la roca
            //_effect2.Parameters["Texture"].SetValue(rocaTexture);
        }
        protected override void AjustarModelo(){
            _matixBase = Matrix.CreateScale(0.01f);
        }

        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/shaderRoca");
        }
               //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {
            // Seteo de textura
            _effect2.Parameters["World"].SetValue(this._matrixMundo);

            foreach (var mesh in _modelo.Meshes)
            {
                _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                mesh.Draw();
            }
        }
        
        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//

        //Configuración de efectos tomados desde la clase padre
        
    }
}