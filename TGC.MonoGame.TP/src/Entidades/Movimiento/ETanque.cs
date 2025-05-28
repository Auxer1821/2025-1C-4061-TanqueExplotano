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
    public class Etanque : EntidadFull
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
        float time = 0.0f;
        float deltaTime = 0.0f;

        //variables de sonido
        private SoundEffectInstance _sonidoDisparoInstance;
        private SoundEffectInstance _sonidoMovimientoInstance;
        private SoundEffectInstance _sonidoDetenidoInstance;
        float _volumen = 0.0f;


        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Etanque() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            this._tipoTanque = new TanqueT90();
            this._modelo = new MTanque(_tipoTanque);
            this._tipo = TipoEntidad.Tanque;
            this._activo = true;
            this._bala = new EBala();
            this._bala.Initialize(Graphics, Mundo, Content, escenario);
            this._bala.setDanio(this._tipoTanque.danio());
            this._cooldownActual = 0.0f;
            //TODO - Crear Bounding Volume especializados // Eliminarlo de EntidadFull
            base.Initialize(Graphics, Mundo, Content, escenario);

            //this._boundingVolume = new BVCuboOBB(this.CalcularCentro(_posicion), new Vector3(4.0f, 8.0f, 8.0f) , Matrix.Identity);
            this._boundingVolume = new BoundingsVolumes.BVEsfera(5.0f, this._posicion);

            //Cargar el sonido
            InstanciarSonidosTanque(Content);
        }

        private void InstanciarSonidosTanque(ContentManager Content)
        {
            SoundEffect sonidoDetenido = Content.Load<SoundEffect>(@"Sounds/tankStop");
            this._sonidoDetenidoInstance = sonidoDetenido.CreateInstance();
            _sonidoDetenidoInstance.IsLooped = true;
            _sonidoDetenidoInstance.Volume = _volumen / 4;

            SoundEffect sonidoMovimiento = Content.Load<SoundEffect>(@"Sounds/tankMove");
            this._sonidoMovimientoInstance = sonidoMovimiento.CreateInstance();
            _sonidoMovimientoInstance.IsLooped = true;
            _sonidoMovimientoInstance.Volume = _volumen / 3;

            SoundEffect sonidoDisparo = Content.Load<SoundEffect>(@"Sounds/disparo2");
            this._sonidoDisparoInstance = sonidoDisparo.CreateInstance();
            _sonidoDisparoInstance.IsLooped = false;
            _sonidoDisparoInstance.Volume = _volumen;
            _sonidoDisparoInstance.Pitch = 0.0f;


        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//

        public override bool PuedeChocar()
        {
            return _activo;
        }
        public override bool PuedeSerChocado()
        {
            return true;
        }

        private Vector3 CalcularCentro(Vector3 pos)
        {
            return new Vector3(pos.X, pos.Y + 1.5f, pos.Z);
        }

        public override void Chocar(DataChoque dataChoque, Entidad entidadEstatica)
        {
            switch (entidadEstatica._tipo)
            {
                case TipoEntidad.Bala:
                    this.RecibirDaño(dataChoque, (EBala)entidadEstatica);
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

            float anguloRotacionY = (float)Math.Atan2(_dirMovimiento.X, _dirMovimiento.Y);
            this._boundingVolume.Transformar(this.CalcularCentro(_posicion), new Vector3(0.0f, anguloRotacionY, 0.0f), 1f);
            time = (float)gameTime.TotalGameTime.TotalSeconds;
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void ActualicarModeloTanque()
        {
            ((MTanque)(this._modelo)).ActualizarMovimeinto(this._velocidadActual, this._anguloActual);
            ((MTanque)(this._modelo)).ActualizarTorreta(this._dirMovimiento, this._dirApuntado);
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
            this._modelo.EfectoDaño(Math.Clamp(this._tipoTanque.Vida() / this._tipoTanque.VidaMaxima(), 0.2f, 1.0f));

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


            this._posicion += new Vector3(_dirMovimiento.X, 0, _dirMovimiento.Y) * _velocidadActual;//p

            // triangulo 
            float dis = 15f;
            Vector2 punto1 = new Vector2((this._posicion.X + this._dirMovimiento.X * dis), (this._posicion.Z + this._dirMovimiento.Y * dis));

            Vector2 perpendicular = new Vector2(-this._dirMovimiento.Y, this._dirMovimiento.X);
            Vector2 punto2 = new Vector2((this._posicion.X + perpendicular.X * dis), (this._posicion.Z + perpendicular.Y * dis));
            Vector2 punto3 = new Vector2((this._posicion.X - perpendicular.X * dis), (this._posicion.Z - perpendicular.Y * dis));

            // Calcular el ángulo de inclinación del tanque en función de la altura del terreno
            var distanciaZ = Vector2.Distance(punto1, new Vector2(_posicion.X, _posicion.Z));
            var AlturaZ = this._escenario.getAltura(new Vector3(punto1.X, 0f, punto1.Y)) - _posicion.Y;
            var ArcoSenoZ = (float)Math.Asin(AlturaZ / distanciaZ);

            var distanciaX = Vector2.Distance(punto2, new Vector2(_posicion.X, _posicion.Z));
            var AlturaX = this._escenario.getAltura(new Vector3(punto2.X, 0f, punto2.Y)) - _posicion.Y;
            var ArcoSenoX = (float)Math.Asin(AlturaX / distanciaX);

            //suavizado de angulo para que no se vea tan brusco
            var anguloObjetivo = new Vector3(ArcoSenoZ,ArcoSenoX , 0f);
            float suavizado = 1.0f; // Factor de suavizado
            var anguloSuavizado = Vector3.Lerp(this._angulo, anguloObjetivo, suavizado * deltaTime);
            anguloSuavizado.Z = -(float)Math.Atan2(_dirMovimiento.Y, _dirMovimiento.X); // Mantener la rotación en Z para que apunte hacia adelante
            // Actualizar el ángulo del tanque
            this._angulo = anguloSuavizado;
            
            //this._angulo = new Vector3((float)anguloSuaveZ, (float)anguloSuaveX, -(float)Math.Atan2(_dirMovimiento.Y, _dirMovimiento.X));


            this.ActualizarMatrizMundo(); // Puntual para la grafica
            this._boundingVolume.Transformar(_posicion, _angulo, 1.0f); // Puntual para la colision


            var posAux = this._posicion;
            posAux.Y = this._escenario.getAltura(punto1,punto2,punto3);
            this._posicion = Vector3.Lerp(this._posicion, posAux, 0.3f); // Suavizado de altura


            //sonido
            if (_velocidadActual != 0)
            {
                if (_sonidoMovimientoInstance.State != SoundState.Playing)
                {
                    _sonidoMovimientoInstance.Play();
                }
                if (_sonidoDetenidoInstance.State == SoundState.Playing)
                {
                    _sonidoDetenidoInstance.Stop();
                }
            }
            else
            {
                if (_sonidoMovimientoInstance.State == SoundState.Playing)
                {
                    _sonidoMovimientoInstance.Stop();
                }
                if (_sonidoDetenidoInstance.State != SoundState.Playing)
                {
                    _sonidoDetenidoInstance.Play();
                }
            }


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
                _sonidoDisparoInstance.Pitch = new Random().Next(-90, 100) / 100.0f;
                _sonidoDisparoInstance.Play();
            }
        }

        //
        public void ActualizarApuntado(Vector3 apuntado)
        {
            this._dirApuntado = apuntado;
        }



    }
}