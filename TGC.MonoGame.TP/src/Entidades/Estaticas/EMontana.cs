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
    public class EMontana : Entidades.EntidadFullPrimitiva
    {
        public EMontana(){}
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            this._objeto = new Montanas.OMontana();
            this._tipo = TipoEntidad.Obstaculo;
            base.Initialize(Graphics,Mundo,View,Projection,Content, escenario);
        }
    }
}