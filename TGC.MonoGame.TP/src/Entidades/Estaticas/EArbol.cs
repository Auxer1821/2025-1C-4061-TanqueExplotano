using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class EArbol : Entidades.EntidadFullPrimitiva
    {
        public EArbol(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            this._objeto = new Arboles.OArbol();
            this._tipo = TipoEntidad.Obstaculo;
            //TODO: Crear Bounding Volume
            base.Initialize(Graphics,Mundo,View,Projection,Content, escenario);
        }


    }
}