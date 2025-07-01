using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.src.Entidades;

namespace TGC.MonoGame.TP.src.EstadoIA
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class IEstadoIA
    {
        //--------------------------Variables------------------------//
        protected ETanqueIA _tanqueIA;
        protected EJugador _tanqueJugador;
        //----------------------------metodos------------------------//
        //---Initialize---//
        public virtual void Initialize(ETanqueIA tanqueIA, EJugador tanqueJugador)
        {
            this._tanqueIA = tanqueIA;
            this._tanqueJugador = tanqueJugador;
        }
        //---MetodoPrincipal---//
        public abstract void Update(GameTime gameTime);
        //---CambiaEstado---//
        public abstract void Exit();

        protected virtual bool DetectarJugador(float distancia)
        {
            Vector2 resta = this._tanqueJugador.GetPosition() - this._tanqueIA.GetPosition();
            return (resta.X * resta.X + resta.Y + resta.Y) <= distancia * distancia;
        }
    }
}