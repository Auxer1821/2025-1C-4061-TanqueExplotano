using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.Tanques;
using Microsoft.Xna.Framework.Audio;
using TGC.MonoGame.TP.src.Graficos.Temporales;
using System.Runtime.InteropServices.Marshalling;

namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class Etanque : EntidadFull
    {

        // Variables
        private float tiempoUltimoImpacto = 0f;
        private bool _activo;
        protected TipoTanque _tipoTanque;
        protected Vector2 _dirMovimiento = Vector2.UnitX;
        protected Vector3 _dirApuntado = Vector3.UnitX;
        protected Vector3 _posicionSalidaBala;
        protected EBala _bala;
        protected float _cooldownActual;
        protected float _velocidadActual;
        protected float _anguloActual;
        protected float time = 0.0f;
        protected float deltaTime = 0.0f;
        protected float _killCount = 0.0f;

        //variables de sonido
        public Managers.ManagerSonido _managerSonido;
        public EmisorParticula _particulasDisparo;


        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Etanque() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            if (this._tipoTanque == null)
            {
                this._tipoTanque = new TanqueT90();
            }
            this._modelo = new MTanque(_tipoTanque);
            this._tipo = TipoEntidad.Tanque;
            this._activo = true;
            this._bala = new EBala();
            this._bala.Initialize(Graphics, Mundo, Content, escenario);
            this._bala.setDanio(this._tipoTanque.danio());
            this._cooldownActual = 0.0f;
            this._bala.setTanque(this);
            //TODO - Crear Bounding Volume especializados // Eliminarlo de EntidadFull
            base.Initialize(Graphics, Mundo, Content, escenario);

            this._boundingVolume = new BoundingsVolumes.BVEsfera(5.0f, this._posicion);
            //this._boundingVolume = new BoundingsVolumes.BVCilindroOBB(Vector3.Zero, 5.0f, 7.0f, new Vector3(_dirMovimiento.X, 0, _dirMovimiento.Y));
            //this._boundingVolume.Transformar(this._posicion, this._angulo, 1f);

            this._particulasDisparo = new EmisorParticula();
            this._particulasDisparo.Initialize(Content, Graphics, 10, new Vector3(0, 2, 0));
            this._particulasDisparo.SetNuevaPosicion(new Vector3(this._posicion.X/6, 0, this._posicion.Z/6) + new Vector3(_dirApuntado.X/6, 0.05f, _dirApuntado.Z/6));

            //Cargar el sonido
            this._managerSonido = new Managers.ManagerSonido(Content);
            this._managerSonido.InstanciarSonidosTanque();


            
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
                    //this._managerSonido.reproducirSonido("colision");
                    break;
                case TipoEntidad.Obstaculo:
                    this.AplicarColisionMovimiento(dataChoque);
                    //this._managerSonido.reproducirSonido("colision");
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

            if (!this.PuedeDisparar())
            {
                this._cooldownActual += 1 * deltaTime;
            }
            this.tiempoUltimoImpacto += deltaTime;

            // Actualizar partículas de disparo
            if (this._particulasDisparo != null)
            {
                this._particulasDisparo.Update(gameTime);
                this._particulasDisparo.SetNuevaPosicion(new Vector3(this._posicion.X/6, 0, this._posicion.Z/6) + new Vector3(_dirApuntado.X/6, 0.05f, _dirApuntado.Z/6));
            }

            //actualizar BV
            this._boundingVolume.Transformar(this._posicion, this._angulo, 1f);
        }

        public override void Dibujar(GraphicsDevice Graphics)
        {
            this._particulasDisparo.Dibujar();
            base.Dibujar(Graphics);
        }

        public override void EfectCamera(Matrix vista, Matrix proyeccion)
        {
            this._particulasDisparo.SetVistaProyeccion(vista, proyeccion);
            base.EfectCamera(vista, proyeccion);
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
            if (this.tiempoUltimoImpacto < 0.5 || bala == this._bala || !this._tipoTanque.EstaVivo())
            return;
            
            // Restar vida (suponiendo que existe una propiedad 'Vida')
            this._tipoTanque.RecibirDanio(bala._danio);
            this._modelo.EfectoDaño(Math.Clamp(this._tipoTanque.Vida() / this._tipoTanque.VidaMaxima(), 0.2f, 1.0f));
            this.tiempoUltimoImpacto = 0.0f;

            // Efectos visuales
            // MostrarChispa(choque._puntoContacto);
            // ReproducirSonidoImpacto();
            // Modificar la mesh del modelo para simular el impacto (Entrega 4)

            this._managerSonido.reproducirSonido("impactoBala");
            // Chequear destrucción
            if (!this._tipoTanque.EstaVivo()){   
                this.Destruir();
                bala.Kill();
            }
        }


        public virtual void Destruir()
        {
            // Lógica de destrucción:  explosionar, etc.
            this._activo = false;
            // CrearEfectoExplosion(this._modelo.Posicion);
            this._escenario.AgregarAEliminar(this);
            this._escenario.AgregarACrear(this);

            this._particulasDisparo.SetPuedeDibujar(false);
            this._managerSonido.reproducirSonido("muerte");
        }

        protected bool PuedeDisparar()
        {
            return this._cooldownActual > this._tipoTanque.cooldown();
        }

        public float getVida()
        {
            return this._tipoTanque.Vida();
        }

        public void SetTipoTanque(string tipoTanque)
        {
            // Cambia el tipo de tanque y actualiza el modelo y las propiedades
            switch (tipoTanque)
            {
                case "T90":
                    this._tipoTanque = new TanqueT90();
                    break;
                case "Panzer":
                    this._tipoTanque = new TanquePanzer();
                    break;
                default:
                    throw new ArgumentException("Tipo de tanque no reconocido");
            }

            this._modelo = new MTanque(this._tipoTanque);
        }

        public void SetSkinTanque(string skin)
        {
            // Cambia la skin del tanque
            ((MTanque)this._modelo).CambiarTexturaT90(skin);
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

            //altura entre los puntos 2 y 3
            var distanciaX = Vector2.Distance(punto2, punto3);
            var AlturaX = this._escenario.getAltura(new Vector3(punto2.X, 0f, punto2.Y)) - this._escenario.getAltura(new Vector3(punto3.X, 0f, punto3.Y));

            var ArcoSenoX = (float)Math.Asin(AlturaX / distanciaX);

            //suavizado de angulo para que no se vea tan brusco
            var anguloObjetivo = new Vector3(ArcoSenoZ, -ArcoSenoX, 0f);
            float suavizado = 1.0f; // Factor de suavizado
            var anguloSuavizado = Vector3.Lerp(this._angulo, anguloObjetivo, suavizado * deltaTime);
            anguloSuavizado.Z = -(float)Math.Atan2(_dirMovimiento.Y, _dirMovimiento.X); // Mantener la rotación en Z para que apunte hacia adelante
            // Actualizar el ángulo del tanque
            this._angulo = anguloSuavizado;

            //this._angulo = new Vector3((float)anguloSuaveZ, (float)anguloSuaveX, -(float)Math.Atan2(_dirMovimiento.Y, _dirMovimiento.X));


            this.ActualizarMatrizMundo(); // Puntual para la grafica
            this._boundingVolume.Transformar(_posicion, _angulo, 1.0f); // Puntual para la colision


            var posAux = this._posicion;
            posAux.Y = this._escenario.getAltura(punto1, punto2, punto3);
            this._posicion = Vector3.Lerp(this._posicion, posAux, 0.3f); // Suavizado de altura



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
            this.setPosicionSalidaBala();
            this._bala.ActualizarDatos(this._dirApuntado, this._posicionSalidaBala); //TODO - Cambiar lugar de disparo para que no se autodestruya
            this._escenario.AgregarACrear(this._bala); //temporal

            this._particulasDisparo.SetNuevaPosicion(this._posicion/6 + new Vector3(_dirApuntado.X/6 *7f, 0.6f , _dirApuntado.Z/6 *7f));
            this._particulasDisparo.SetPuedeDibujar(true);

            //sonido disparo
            //this._managerSonido.reproducirSonido("disparo");
        }

        public virtual void setPosicionSalidaBala()
        {
            this._posicionSalidaBala=this._posicion;
        }

        protected void ActualizarApuntado(Vector3 apuntado)
        {
            this._dirApuntado = apuntado;
        }

        internal void Kill()
        {
            this._killCount++;
            this.logicaKill();
        }

        public virtual void logicaKill()
        {
            throw new NotImplementedException();
        }

        public float GetKills(){
            return this._killCount;
        }

        
    }
}