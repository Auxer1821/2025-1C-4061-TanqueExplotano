using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Graficos.Utils;
using TGC.MonoGame.TP.src.Moldes;


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
        public Modelos.Modelo _modelo { get; set; }
        public BoundingsVolumes.BoundingVolume _boundingVolume { get; set; }
        public Escenarios.Escenario _escenario { get; set; }

        public TipoEntidad _tipo { get; set; }

        // DataMundo  --> La actualización está en: ""
        public Vector3 _posicion { get; set; } //(x,y,z)
        public float _escala { get; set; }     //(cantidad)
        public Vector3 _angulo { get; set; }   //(zy,xz,xy)

        protected IMolde _molde;

        //----------------------------------------------Metodos--------------------------------------------------//
        public virtual bool PuedeChocar()
        {
            throw new NotImplementedException();
        }
        public virtual bool PuedeSerChocado()
        {
            throw new NotImplementedException();
        }
        public virtual bool PuedeDibujar()
        {
            throw new NotImplementedException();
        }
        public virtual void Dibujar(GraphicsDevice graphics)
        {
            throw new NotImplementedException();
        }

        public virtual void Chocar(DataChoque dataChoque, Entidad entidadEstatica)
        {
            throw new NotImplementedException();
        }


        public virtual void InicializarDataMundo()
        {
            this._posicion = Vector3.Zero;
            this._angulo = Vector3.Zero;
            this._escala = 1.0f;
        }

        public virtual void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public virtual void EfectCamera(Matrix vista, Matrix proyeccion)
        {
            _modelo.EfectCamera(vista, proyeccion);
        }


        public virtual Matrix GetMundo()
        {
            throw new NotImplementedException();
        }

        public IMolde GetMolde()
        {
            return this._molde;
        }
        public virtual bool ExcluidoDelFrustumCulling()
        {
            return false; // Por defecto no se excluye del frustum culling
        }

        public virtual void DibujarShadowMap(GraphicsDevice graphics, Matrix vista, Matrix proyeccion)
        {
            throw new NotImplementedException(); //TODO - Actualizarlo para todos;
        }

        public virtual void Dibujar(GraphicsDevice graphicsDevice, ShadowMapping shadowMapper)
        {
            throw new NotImplementedException();
        }
    }
}