using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class Entidad
    {
        
        // Variables
        public Modelos.Modelo _modelo {get;set;}
        public BoundingsVolumes.BoundingVolume _boundingVolume {get;set;}
        

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

        public void Chocar(DataChoque dataChoque, Entidad entidadEstatica)
        {
            throw new NotImplementedException();
        }
        public virtual void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            throw new NotImplementedException();
        }

        
    }
}