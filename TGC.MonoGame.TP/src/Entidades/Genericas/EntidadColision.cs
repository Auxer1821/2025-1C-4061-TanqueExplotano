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
    public abstract class EntidadColision:Entidad
    {
        
        // Variables

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public virtual void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario){
            this.InicializarDataMundo();

            this._modelo = null;
            this._escenario = escenario;
            this._posicion=Vector3.Transform(Vector3.Zero,Mundo);
            this._boundingVolume= new BoundingsVolumes.BVEsfera(3.0f,  this._posicion);
        }

        public override bool PuedeChocar(){
            return false;
        }
        public override bool PuedeSerChocado(){
            return true;
        }

        
    }
}