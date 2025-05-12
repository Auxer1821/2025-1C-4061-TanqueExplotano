using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;


namespace TGC.MonoGame.TP.src.Entidades
{

    public enum TipoEntidad
    {
        Obstaculo,
        Tanque,
        Bala,
        Otro
    }
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class Entidad
    {
        
        // Variables
        public Modelos.Modelo _modelo {get;set;}
        public BoundingsVolumes.BoundingVolume _boundingVolume {get;set;}
        public Escenarios.Escenario _escenario {get;set;}

        public TipoEntidad _tipo {get;set;}

        // DataMundo  --> La actualización está en: ""
        public Vector3 _posicion {get;set;} //(x,y,z)
        public float _escala {get;set;}     //(cantidad)
        public Vector3 _angulo {get;set;}   //(zy,xz,xy)

        //----------------------------------------------Metodos--------------------------------------------------//
        public virtual bool PuedeChocar(){
            throw new NotImplementedException();
        }
        public virtual bool PuedeSerChocado(){
            throw new NotImplementedException();
        }
        public virtual void Dibujar(GraphicsDevice graphics){
            throw new NotImplementedException();
        }

        public virtual void Chocar(DataChoque dataChoque, Entidad entidadEstatica)
        {
            throw new NotImplementedException();
        }
        public virtual void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            throw new NotImplementedException();
        }

        public virtual void InicializarDataMundo()
        {
            this._posicion = Vector3.Zero;
            this._angulo = Vector3.Zero;
            this._escala = 1.0f;
        }

        public virtual void Update(GameTime gameTime){
            throw new NotImplementedException();
        }

        
    }
}