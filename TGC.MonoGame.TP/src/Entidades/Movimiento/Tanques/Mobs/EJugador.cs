using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Tanques;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class EJugador : Etanque
    {
        private Cameras.FreeCamera _Camara;

        private float _tiempoRestante;



        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public EJugador() { }
        // es necesario hacer un override?

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            _tiempoRestante = 80f;
            base.Initialize(Graphics, Mundo, Content, escenario);
        }

        public void setCamara(Cameras.FreeCamera Camara)
        {
            _Camara = Camara;
        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//

        public override void Update(GameTime gameTime)
        {
            //------setear los valores de movimiento y disparo-------//
            float mseg = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this._tiempoRestante -= mseg;

            if(_tiempoRestante<0f){
                this.Perder();
            }
            
            var teclado = Keyboard.GetState();
            var raton = Mouse.GetState();

            //------------Logica-Movimiento---------------------------//

            this._velocidadActual = 0; // cuantsdo se mueve segun su direccion
            if (teclado.IsKeyDown(Keys.W))
            {
                this._velocidadActual += this._tipoTanque.velocidad();
            }

            if (teclado.IsKeyDown(Keys.S))
            {
                this._velocidadActual -= this._tipoTanque.velocidad();
            }
            this._velocidadActual *= mseg;

            this._anguloActual = 0.0f; //cuanto rota segun su direcion
            if (teclado.IsKeyDown(Keys.A))
            {
                this._anguloActual -= this._tipoTanque.anguloRotacionMovimiento();
            }

            if (teclado.IsKeyDown(Keys.D))
            {
                this._anguloActual += this._tipoTanque.anguloRotacionMovimiento();
            }
            this._anguloActual *= mseg;

            //---------------Logica-Disparo---------------------//


            this._dirApuntado = _Camara.getDireccion();
            if (raton.LeftButton == ButtonState.Pressed)
            {
                this.Disparar();
            }

            //---------------------Logica-Camara-----------------------//

            _Camara.actualizarCamara(this._posicion - new Vector3(_dirApuntado.X * 10, 2.5f, _dirApuntado.Z * 10), new Vector3(_dirMovimiento.X, 1.0f, _dirMovimiento.Y) * 8, gameTime);

            //-----------------------Sonido----------------------------//
            this._managerSonido.sonidoMovimiento(_velocidadActual != 0);
            this._managerSonido.sonidoDetenido(_velocidadActual == 0);

            base.Update(gameTime);
        }

        public override void Disparar(){
            if(!this.PuedeDisparar()) return;
            base.Disparar();
            //efecto de camara
            _Camara.setSacudida(true);
            //efecto de sonido
            this._managerSonido.reproducirSonido("disparo");
        }

        internal float porcentajeRecargado() //TODO
        {
            return 1.0f - (this._tipoTanque.cooldown() - _cooldownActual) / this._tipoTanque.cooldown();
        }

        public override void SetPosicionParticulas()
        {
            this._particulasDisparo.SetNuevaPosicion(this._posicion + new Vector3(_dirApuntado.X * 12f, this._dirApuntado.Y + 4f, _dirApuntado.Z * 12f));
            this._particulasDisparo.SetPosiciones(this._posicion + new Vector3(_dirApuntado.X * 12f, this._dirApuntado.Y + 4f, _dirApuntado.Z * 12f));
        }

        public override void setPosicionSalidaBala()
        {
            this._posicionSalidaBala = _Camara.Position;
        }


        //el jugador siempre se dibuja, para evitar que se vea el escenario sin el tanque
        public override bool ExcluidoDelFrustumCulling()
        {
            return true;
        }
        public override void Destruir()
        {
            this.Perder();
        }

        private void Perder()
        {
            this._escenario.FinJuegoPerder();
        }
        private void Ganar()
        {
            this._escenario.FinJuegoGanar();
        }

        internal float tiempoRestante()
        {
            return _tiempoRestante;
        }

        public override void logicaKill(){
            if(this.GetKills() >= 3f)
            this._escenario.FinJuegoGanar();
        }


        //---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//



    }
}
