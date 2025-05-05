using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class Etanque:EntidadFull
    {
        
        // Variables

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Etanque(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content){
            this._modelo = new Tanques.MTanque();
            //Crear Bounding Volume
            base.Initialize(Graphics,Mundo,View,Projection,Content);
        }
        
    }
}