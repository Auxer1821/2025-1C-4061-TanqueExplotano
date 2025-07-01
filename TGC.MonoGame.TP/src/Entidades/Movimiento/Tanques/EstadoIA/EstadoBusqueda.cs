using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.src.Entidades;

namespace TGC.MonoGame.TP.src.EstadoIA
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class EstadoBusqueda : IEstadoIA
    {
        //--------------------------Atributos------------------------//
        private Vector2 _posicionDestino;
        private Random _random;
        private float _cooldownPosicion;
        private float _radioDeteccion;
        //----------------------------metodos------------------------//

        //---Inicializador--//
        public override void Initialize(ETanqueIA tanqueIA, EJugador tanqueJugador)
        {
            base.Initialize(tanqueIA, tanqueJugador);
            this._random = new Random();
            this._posicionDestino = new Vector2(_random.Next(-300, 300), _random.Next(-300, 300));
            this._cooldownPosicion = 3.0f;
            this._radioDeteccion = 40.0f;

        }
        //---MetodoPrincipal---//
        public override void Update(GameTime gameTime)
        {
            this._tanqueIA.MoverA(_posicionDestino, gameTime);
            this._tanqueIA.ApuntarA(_posicionDestino, gameTime);
            _cooldownPosicion -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_tanqueIA.GetPosition() == _posicionDestino || 0f >= _cooldownPosicion)
            {
                this._posicionDestino = new Vector2(_random.Next(-300, 300), _random.Next(-300, 300));
                _cooldownPosicion = 3.0f;
            }

            if (this.DetectarJugador(_radioDeteccion))
            {
//                EstadoChase estadoChase = this._tanqueIA._eChase;
//                estadoChase.Initialize(this._tanqueIA, this._tanqueJugador);
                //this._tanqueIA.CambiarEstadoIA(this._tanqueIA._eChase);
                this._tanqueIA.CambiarEstadoIA("Chase");
            }
        }
        //---CambiaEstado---//
        public override void Exit()
        {

        }
    }
}