using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Arboles
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    
    //cambio de objeto a modelo, para poder usar el modelo de arbol
    public class OArbol : Modelos.Modelo
    {
        
        // Variables
        //Texture2D troncoTexture;
        //Texture2D hojasTexture;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public OArbol(){}
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content)
        {
            //this._Color = Color.Green.ToVector3();
            base.Initialize(Graphics, Mundo, Content);
        }

        protected override void ConfigurarModelo(ContentManager Content)
        {
            /*
            this._modelo = Content.Load<Model>(@"Models/tree/tree2");
            troncoTexture = Content.Load<Texture2D>(@"Models/tree/tronco2");
            hojasTexture = Content.Load<Texture2D>(@"Models/tree/light-green-texture");

            _effect2.Parameters["TextureTronco"].SetValue(troncoTexture);
            _effect2.Parameters["TextureHojas"].SetValue(hojasTexture);
            */

        }

        protected override void AjustarModelo()
        {
            _matixBase = Matrix.CreateScale(0.008f);
        }

        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/shaderArbol");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta


        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {
            // Seteo de textura
            _effect2.Parameters["World"].SetValue(this._matrixMundo);

            foreach (var mesh in _modelo.Meshes)
            {
                if (mesh.Name.Contains("Zyl"))
                {
                    _effect2.CurrentTechnique = _effect2.Techniques["Tronco"];
                }
                else
                {
                    _effect2.CurrentTechnique = _effect2.Techniques["Hojas"];
                }
                _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                mesh.Draw();
            }
        }
 

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//


        //Configuración de efectos tomados desde la clase padre

    }
}