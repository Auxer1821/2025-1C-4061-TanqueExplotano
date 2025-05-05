using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Modelos;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class EntidadGraficaPrimitiva:EntidadGrafica
    {
        
        // Variables
        protected Objeto _objeto;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//

        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content){
            this._boundingVolume = null;
            this._modelo = null;
            this._objeto.Initialize(Graphics,Mundo,View,Projection,Content);

        }

        public override void Dibujar(GraphicsDevice Graphics){
            _objeto.Dibujar(Graphics);
        }

        public override void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            this._objeto.ActualizarVistaProyeccion(Vista,Proyeccion);
        }
    }
}