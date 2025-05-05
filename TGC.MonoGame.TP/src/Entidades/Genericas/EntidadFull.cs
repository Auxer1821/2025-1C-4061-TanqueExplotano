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
    public abstract class EntidadFull:Entidad
    {
        
        // Variables

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public virtual void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content){
            this._modelo.Initialize(Graphics,Mundo,View,Projection,Content);
        }

        public virtual void Dibujar(GraphicsDevice graphics){
            this._modelo.Dibujar(graphics);
        }
        public virtual void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            this._modelo.ActualizarVistaProyeccion(Vista,Proyeccion);
        }

        //TODO Crear vista y proyección
        
    }
}