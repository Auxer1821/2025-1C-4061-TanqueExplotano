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
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario)
        {
            base.Initialize(Graphics, Mundo, View, Projection, Content, escenario);
            this._modelo.ActualizarColor(Color.RoyalBlue);
        }

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
            if(this._velocidadActual != 0.0f){
                if (teclado.IsKeyDown(Keys.A))
                {
                    this._anguloActual -= this._tipoTanque.anguloRotacionMovimiento();
                }

                if (teclado.IsKeyDown(Keys.D))
                {
                    this._anguloActual += this._tipoTanque.anguloRotacionMovimiento();
                }
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
                }
            }
            else
            {
                this._cooldownActual += 1*mseg;
            }

            //---------------------Logica-Camara-----------------------//
            

            //cambios para ver el tanque
            _Camara.setPosicion(this._posicion - new Vector3(_dirMovimiento.X, 0, _dirMovimiento.Y) * 5  , new Vector3(0f, 1.0f, 0f));
            
            
            base.Update(gameTime);
        }
        
        protected override void RecibirDaño(DataChoque choque,  EBala bala){}
        



//---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//


    }
}
