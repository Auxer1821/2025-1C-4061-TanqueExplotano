using System;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.src.Entidades;

namespace TGC.MonoGame.TP.src.EstadoIA
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class EstadoDisparo : IEstadoIA
    {
        //----------------------------atributos------------------------//
        private int _dispercion;
        private float _tiempoApuntado;
        private Random _random;
        //----------------------------metodos------------------------//

        //---Inicializador--//
        public override void Initialize(ETanqueIA tanqueIA, EJugador tanqueJugador)
        {
            base.Initialize(tanqueIA, tanqueJugador);
            this._tiempoApuntado = 1.0f;
            this._dispercion = 5;
            _random = new Random();

        }
        //---MetodoPrincipal---//
        public override void Update(GameTime gameTime)
        {
            this._tiempoApuntado -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            this._tanqueIA.ApuntarA(this._tanqueJugador.GetPosition(), gameTime);
            this._tanqueIA.SetVelocidad(0.0f);
            if (this._tiempoApuntado <= 0)
            {
                //TODO SOBRE LA DISPERCION
                var incremento = new Vector2((float)this._random.Next(-this._dispercion, this._dispercion) , (float)this._random.Next(-this._dispercion, this._dispercion) );// angulu (vertival, orizontal)
                //var incremento = new Vector2(180f,0f);
                this._tanqueIA.DispararConDispercion(incremento);//en grados el incremento

                //cambio de estado despues de disparar
                this._tanqueIA.CambiarEstadoIA("Busqueda");
                this.ResetState();


            }
        }
        //---CambiaEstado---//
        public override void Exit()
        {

        }
        private void ResetState(){
            this._tiempoApuntado = 1.0f;
        }

    }
}