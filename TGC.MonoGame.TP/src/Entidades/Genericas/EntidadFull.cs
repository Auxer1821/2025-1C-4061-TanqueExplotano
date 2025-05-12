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
        public virtual void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            this.InicializarDataMundo();

            this._posicion=Vector3.Transform(Vector3.Zero,Mundo);
            this._boundingVolume= new BoundingsVolumes.BVEsfera(3.0f,  this._posicion);
            this._modelo.Initialize(Graphics,Mundo,View,Projection,Content);
            this._escenario = escenario;
        }

        public override void Dibujar(GraphicsDevice graphics){
            this._modelo.Dibujar(graphics);
        }
        public override void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            this._modelo.ActualizarVistaProyeccion(Vista,Proyeccion);
        }
        
        public override bool PuedeChocar(){
            return false;
        }
        public override bool PuedeSerChocado(){
            return true;
        }

        public virtual void ActualizarMatrizMundo(){
            Matrix mundo = Matrix.Identity;
            mundo *= Matrix.CreateScale(this._escala);
            mundo *= Matrix.CreateFromYawPitchRoll(this._angulo.Z, this._angulo.Y, this._angulo.X);
            mundo *= Matrix.CreateTranslation(this._posicion);

            this._modelo.ActualizarMatrizMundo(mundo);

        }



        //TODO Crear vista y proyección
        
    }
}