using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
     
     
    //cambio de endidad full primitiva a entidad full
    public class EArbol : Entidades.EntidadFull
    {
        public EArbol(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content){
            this._modelo = new Arboles.OArbol();
            //TODO: Crear Bounding Volume
            base.Initialize(Graphics,Mundo,View,Projection,Content);
        }


    }
}