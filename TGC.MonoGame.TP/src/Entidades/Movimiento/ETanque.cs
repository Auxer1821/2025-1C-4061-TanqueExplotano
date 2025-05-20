using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Tanques;
using Microsoft.Xna.Framework.Audio;

namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class Etanque:EntidadFull
    {
        
        // Variables
        private bool _activo;
        protected TipoTanque _tipoTanque;
        protected Vector2 _dirMovimiento = Vector2.UnitX;
        protected Vector3 _dirApuntado = Vector3.UnitX;
        protected EBala _bala;
        protected float _cooldownActual;
        protected float _velocidadActual;
        protected float _anguloActual;
        private SoundEffect _sonidoDisparo;
        private SoundEffectInstance _sonidoDisparoInstance;
        float _volumen = 0.3f;
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Etanque() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._tipoTanque = new TanqueT90();
            this._modelo = new MTanque(_tipoTanque);
            this._tipo = TipoEntidad.Tanque;
            this._activo = true;
            this._bala = new EBala();
            this._bala.Initialize(Graphics, Mundo, View, Projection, Content, escenario);
            this._bala.setDanio(this._tipoTanque.danio());
            this._cooldownActual = 0.0f;
            //TODO - Crear Bounding Volume especializados // Eliminarlo de EntidadFull
            base.Initialize(Graphics, Mundo, View, Projection, Content, escenario);


            this._sonidoDisparo = Content.Load<SoundEffect>("Sounds/disparo2");
            this._sonidoDisparoInstance = _sonidoDisparo.CreateInstance();
            _sonidoDisparoInstance.IsLooped = false;
            _sonidoDisparoInstance.Volume = _volumen;
            _sonidoDisparoInstance.Pitch = 0.0f;
        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//

        public override bool PuedeChocar(){
            return _activo;
        }
        public override bool PuedeSerChocado(){
            return true;
        }

        public override void Chocar(DataChoque dataChoque, Entidad entidadEstatica){
            switch (entidadEstatica._tipo)
            {
                case TipoEntidad.Bala:
                    this.RecibirDaño(dataChoque, (EBala) entidadEstatica);
                    break;

                case TipoEntidad.Tanque:
                    this.AplicarColisionMovimiento(dataChoque);
                    break;
                case TipoEntidad.Obstaculo:
                    this.AplicarColisionMovimiento(dataChoque);
                    break;

                default:
                    // Quizás no hacer nada, o loguear el caso
                    break;
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            this.ActualizarDireccion();
            this.Mover();
            this.ActualicarModeloTanque();
        }

        private void ActualicarModeloTanque(){
            ((MTanque)(this._modelo)).ActualizarMovimeinto(this._velocidadActual,this._anguloActual);
            ((MTanque)(this._modelo)).ActualizarTorreta(this._dirMovimiento,this._dirApuntado);
        }


        //Metodos propios
        private void AplicarColisionMovimiento(DataChoque choque)
        {
            // Empuja al tanque hacia afuera para que no quede dentro del obstáculo
            Vector3 correccion = choque._normal * choque._penetracion;

            // Corregir posición
            this._posicion += correccion;

            // Cancelar velocidad en dirección del impacto (variable de velocidad / movimiento)
            // this.Velocidad -= Vector3.Dot(this.Velocidad, choque._normal) * choque._normal;

            // También podrías anular el movimiento completamente, si es más simple
            // Prefiero esto: El Jugador setea su velocidad a 0 todos los frames; Los Tanques pueden seguir rotando y cuando dejen de chocar se moveran nuevamente
            this._velocidadActual = 0.0f;
        }

        protected virtual void RecibirDaño(DataChoque choque, EBala bala)
        {
            if (!this._tipoTanque.EstaVivo()) return;
            // Restar vida (suponiendo que existe una propiedad 'Vida')
            this._tipoTanque.RecibirDanio(bala._danio);
            this._modelo.EfectoDaño(Math.Clamp(this._tipoTanque.Vida()/this._tipoTanque.VidaMaxima(),0.2f,1.0f));

            // Efectos visuales
            // MostrarChispa(choque._puntoContacto);
            // ReproducirSonidoImpacto();
            // Modificar la mesh del modelo para simular el impacto (Entrega 4)

            // Chequear destrucción
            if (!this._tipoTanque.EstaVivo()) this.Destruir();
        }
        

        private void Destruir()
        {
            // Lógica de destrucción:  explosionar, etc.
            this._activo = false;
            // CrearEfectoExplosion(this._modelo.Posicion);
            this._escenario.AgregarAEliminar(this);
            this._modelo.ActualizarColor(Color.DarkRed);
            this._escenario.AgregarACrear(this);
        }

        protected bool PuedeDisparar()
        {
            return this._cooldownActual > this._tipoTanque.cooldown();
        }

//---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//

        // Función que actualiza los valores de posición y ángulo
        // Para luego usarlos y crear la matriz mundo
        public void Mover()
        {
            this._posicion += new Vector3(_dirMovimiento.X, 0, _dirMovimiento.Y) * _velocidadActual;

            var angulo = this._angulo;
            angulo.Z -= _anguloActual;
            this._angulo = angulo;

            this.ActualizarMatrizMundo(); // Puntual para la grafica
            this._boundingVolume.Transformar(_posicion, _angulo, 1.0f); // Puntual para la colision


            var posAux = this._posicion;
            posAux.Y = this._escenario.getAltura(this._posicion);
            this._posicion = posAux;

            
        }

        //Función llamada gameplay para que actualice los valores de la matriz dirección.
        // Valores de 1, -1, 0
        // Luego será multiplicado por su respectiva velocidad
        public void ActualizarDireccion()
        {
            _dirMovimiento = Vector2.Rotate(_dirMovimiento, _anguloActual);
            //TODO: direccion de la mira
        }
        // 
        public void Disparar()
        {
            this._bala.ActualizarDatos(this._dirApuntado, this._posicion); //TODO - Cambiar lugar de disparo para que no se autodestruya
            this._escenario.AgregarACrear(this._bala); //temporal

            //sonido disparo
            if (_sonidoDisparoInstance.State != SoundState.Playing)
            {
                _sonidoDisparoInstance.Pitch = new Random().Next(-100, 100) / 100.0f;
                _sonidoDisparoInstance.Play();
            }
        }

        //
        public void ActualizarApuntado(Vector3 apuntado){
            this._dirApuntado = apuntado;
        }

    }
}