using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.src.Entidades;

namespace TGC.MonoGame.TP.src.EstadoIA

{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class EstadoChase : IEstadoIA
    {
        //--------------------------Atributos------------------------//
        private float _tiempoSeguimiento;
        private float _tiempoEspera;
        private float _radioDeteccion;
        //----------------------------metodos------------------------//

        //---Inicializador--//
        public override void Initialize(ETanqueIA tanqueIA, EJugador tanqueJugador)
        {
            base.Initialize(tanqueIA, tanqueJugador);
            this._tiempoSeguimiento = 10.0f;
            this._tiempoEspera = 3.0f;
            this._radioDeteccion = 10.0f;

        }
        //---MetodoPrincipal---//
        public override void Update(GameTime gameTime)
        {
            _tiempoSeguimiento -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (this.DetectarJugador(_radioDeteccion))
            {
                EstadoDisparo estado = new EstadoDisparo();
                estado.Initialize(this._tanqueIA, this._tanqueJugador);
                this._tanqueIA.CambiarEstadoIA(estado);
                return;
            }
            if (_tiempoSeguimiento >= 0)
            {
                this._tanqueIA.MoverA(_tanqueJugador.GetPosition(), gameTime);
                this._tanqueIA.ApuntarA(_tanqueJugador.GetPosition(), gameTime);
            }
            else
            {
                this._tanqueIA.SetVelocidad(0.0f);
                _tiempoEspera -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        
                if (_tiempoEspera <= 0)
                {
                    EstadoBusqueda estado = new EstadoBusqueda();
                    estado.Initialize(this._tanqueIA, this._tanqueJugador);
                    this._tanqueIA.CambiarEstadoIA(estado);
                    return;
                }
            }


        }
        //---CambiaEstado---//
        public override void Exit()
        {

        }
    }
}