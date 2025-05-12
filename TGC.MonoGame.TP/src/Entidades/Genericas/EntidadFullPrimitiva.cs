using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class EntidadFullPrimitiva:EntidadFull
    {
        
        // Variables
        protected Objeto _objeto;

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            this.InicializarDataMundo();

            this._modelo = null;
            this._objeto.Initialize(Graphics,Mundo,View,Projection,Content);
            this._escenario = escenario;
            this._posicion=Vector3.Transform(Vector3.Zero,Mundo);
            this._boundingVolume= new BoundingsVolumes.BVEsfera(3.0f,  this._posicion);

        }

        public override void Dibujar(GraphicsDevice graphics){
            this._objeto.Dibujar(graphics);
        }

        public override void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            this._objeto.ActualizarVistaProyeccion(Vista,Proyeccion);
        }

        public override void ActualizarMatrizMundo(){
            Matrix mundo = Matrix.Identity;
            mundo *= Matrix.CreateScale(this._escala);
            mundo *= Matrix.CreateFromYawPitchRoll(this._angulo.Z, this._angulo.Y, this._angulo.X);
            mundo *= Matrix.CreateTranslation(this._posicion);

            this._objeto.ActualizarMatrizMundo(mundo);

        } 
        
    }
}