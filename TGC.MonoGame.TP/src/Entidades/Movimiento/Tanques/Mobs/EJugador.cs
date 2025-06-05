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



        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public EJugador() { }
        // es necesario hacer un override?
        /*
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            base.Initialize(Graphics, Mundo, Content, escenario);
        }
        */

        public void setCamara(Cameras.FreeCamera Camara){
            _Camara=Camara;
        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//




        public override void Update(GameTime gameTime)
        {
            //------setear los valores de movimiento y disparo-------//
            float mseg = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            if (this.PuedeDisparar())
            {
                if (raton.LeftButton == ButtonState.Pressed)
                {
                    this.Disparar();
                    this._cooldownActual = 0;
                    //efecto de camara
                    _Camara.setSacudida(true);
                    //efecto de sonido
                    this._managerSonido.reproducirSonido("disparo");
                }
            }

            //---------------------Logica-Camara-----------------------//
            
            _Camara.actualizarCamara(this._posicion - new Vector3(_dirApuntado.X * 9, 2.5f, _dirApuntado.Z * 9), new Vector3(_dirMovimiento.X, 1.0f, _dirMovimiento.Y) * 8, gameTime);
            
            //-----------------------Sonido----------------------------//
            this._managerSonido.sonidoMovimiento(_velocidadActual != 0);
            this._managerSonido.sonidoDetenido(_velocidadActual == 0);
            
            base.Update(gameTime);
        }

        internal int getKills() //TODO
        {
            throw new NotImplementedException();
        }

        internal float porcentajeRecargado() //TODO
        {
            return 1.0f - (this._tipoTanque.cooldown() - _cooldownActual) / this._tipoTanque.cooldown();
        }

        public override void setPosicionSalidaBala()
        {
        this._posicionSalidaBala = _Camara.Position;
        }


        //---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//


    }
}
