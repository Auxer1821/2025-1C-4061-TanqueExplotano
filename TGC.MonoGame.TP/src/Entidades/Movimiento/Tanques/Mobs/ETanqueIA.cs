using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.BoundingsVolumes;
using TGC.MonoGame.TP.src.EstadoIA;
using TGC.MonoGame.TP.src.Tanques;


namespace TGC.MonoGame.TP.src.Entidades
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public class ETanqueIA:Etanque
    {
        private IEstadoIA _estado;
        private Vector2 _dispercion;
        
        

        //-------------------------------------------||---Constructores-e-inicializador--------------------------------------------------//
        public ETanqueIA() { }
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content, Escenarios.Escenario escenario)
        {
            //Tal vez haga algo
            base.Initialize(Graphics, Mundo, Content, escenario);
            this._estado = new EstadoBusqueda();
            this._estado.Initialize(this, this._escenario.GetJugador());
        }

        //----------------------------------------------Metodos-Logica--------------------------------------------------//

    


        public override void Update(GameTime gameTime)
        {
            //setear los valores de movimiento y disparo
            float mseg = (float) gameTime.ElapsedGameTime.TotalSeconds;

            //1. Setear Velocidad Actual = tipo.velocidad
            //this._velocidadActual = this._tipoTanque.velocidad() * mseg * 0.5f;
            this._estado.Update(gameTime);

            //2. Setear giro = giro.angulo
            //this._anguloActual = this._tipoTanque.anguloRotacionMovimiento() * mseg /8;
            //this.Disparar();
            base.Update(gameTime);
        }



        public void MoverA(Vector2 destino, GameTime gameTime)
        {
            // Calcular dirección hacia el destino
            float dx = destino.X - this._posicion.X;
            float dy = destino.Y - this._posicion.Z;
            Vector2 normalizado = new Vector2(dx, dy);
            normalizado.Normalize();

            this._dirMovimiento = Vector2.Lerp(this._dirMovimiento, normalizado,  ((float)gameTime.ElapsedGameTime.TotalSeconds) * 0.5f);
            this._velocidadActual = this._tipoTanque.velocidad() * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //HacerMovimiento(direccion, velocidad);

            // Acá podrías actualizar la posición si querés simular
            //x += (float)Math.Cos(direccion * Math.PI / 180) * velocidad * 0.016f; // usando un deltaTime simulado (ej: 60 FPS)
            //y += (float)Math.Sin(direccion * Math.PI / 180) * velocidad * 0.016f;
        }
        public void ApuntarA(Vector2 objetivo, GameTime gameTime){
           // Calcular dirección hacia el destino
            float dx = objetivo.X - this._posicion.X;
            float dy = objetivo.Y - this._posicion.Z;
            Vector3 normalizado = new Vector3(dx,0f, dy);
            normalizado.Normalize();

            this._dirApuntado = Vector3.Lerp(this._dirApuntado, normalizado, ((float)gameTime.ElapsedGameTime.TotalSeconds) * 0.8f);

        }

        public override void logicaKill()
        {
            if (this._killCount >= 8f)
            {
                this._escenario.FinJuegoPerder();
            }
        }

        public void CambiarEstadoIA(IEstadoIA estado)
        {
            this._estado = estado;
        }

        internal void SetVelocidad(float velocidad)
        {
            this._velocidadActual = velocidad;
        }

        internal void DispararConDispercion(Vector2 incremento)
        {
            this.SetDispercion(incremento);
            this.Disparar();
        }

        public void SetDispercion(Vector2 dispercion)
        {
            this._dispercion = dispercion;
            
        }
        protected override void ActualizarBala()
        {
            Vector3 dirBalar = new Vector3(this._dirApuntado.X, this._dirApuntado.Y , this._dirApuntado.Z);


            // Ángulos actuales
           float pitch = (float)Math.Asin(dirBalar.Y); // Inclinación
            float yaw = (float)Math.Atan2(dirBalar.X, dirBalar.Z); // Rotación horizontal


            // Incrementos de ángulo (por ejemplo, controlados por input)
            float deltaPitch = MathHelper.ToRadians(this._dispercion.Y);  // subir 1 grado
            float deltaYaw = MathHelper.ToRadians(this._dispercion.X);    // girar 2 grados a la derecha

            // Actualizar ángulos
            pitch += deltaPitch;
            yaw += deltaYaw;

            // Limitar pitch para no mirar totalmente arriba/abajo (evita "gimbal lock")
            pitch = MathHelper.Clamp(pitch, -MathHelper.PiOver2 + 0.01f, MathHelper.PiOver2 - 0.01f);

            // Calcular nueva dirección
            Vector3 direccion = new Vector3(
                (float)(Math.Cos(pitch) * Math.Sin(yaw)),
                (float)(Math.Sin(pitch)),
                (float)(Math.Cos(pitch) * Math.Cos(yaw))
            );

            // Normalizar si necesitas una dirección unitaria
            direccion.Normalize();
            
            this._bala.ActualizarDatos(direccion, this._posicionSalidaBala);
        }


        //---------------------------------------------MOVIMIENTO-Y-APUNTADO---------------------------------------------------//


    }
}