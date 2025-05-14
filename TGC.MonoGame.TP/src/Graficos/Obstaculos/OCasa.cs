using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Casas
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class OCasa : Modelos.Modelo
    {
        
        // Variables
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.DarkRed.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        protected override void ConfigurarModelo(ContentManager Content){
            this._modelo = Content.Load<Model>("Models/house/cottage_fbx");
        }
        protected override void AjustarModelo(){
            _matrixMundo = Matrix.CreateScale(0.005f) * _matrixMundo;
        }


        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
       

        //Configuración de efectos tomados desde la clase padre
        
    }
}