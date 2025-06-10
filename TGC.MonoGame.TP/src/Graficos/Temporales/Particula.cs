using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Graficos.Temporales
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class Particula
    {
        //----------------------------------------------Variables--------------------------------------------------//
        private Vector3 _posicionInicial;
        private Vector3 _coordenadas;
        private Vector3 _velocidadInicial = Vector3.Zero; // Velocidad inicial en cero
        private Vector3 _velocidad;
        private Vector3 _desaceleracion = Vector3.Zero; // Desaceleración inicial en cero
        private float _vidaInicial; // Vida inicial de la partícula
        private float _vida;
        private float _tamanio = 1f; // Tamaño de la partícula
        Effect _efecto;

        private string _tecnica = "Default"; // Nombre de la técnica de sombreado a utilizar


        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Particula()
        {
        }
        // Inicializar recursos necesarios para el HUD
        public void Initialize(Vector3 coordenadas, Effect efecto, Random random, float vida)
        {
            _efecto = efecto;
            this._tecnica = "Particle";
            this._coordenadas = coordenadas;
            this._posicionInicial = coordenadas;

            float velocidadBase = 18.0f;  // Aumentamos la velocidad base
            float variacionVelocidad = 4.0f;
            float gravedad = 25.5f;  // Gravedad más suave
            // Velocidad aleatoria en forma esférica (explosión radial)
            Vector3 direction = new Vector3(
            (float)(random.NextDouble() * 2 - 1),  // Rango -1 a 1
            (float)(random.NextDouble() * 2 - 1),
            (float)(random.NextDouble() * 2 - 1)
            );
            direction.Normalize();


            // Velocidad inicial más fuerte con variación aleatoria
            this._velocidad = direction * (float)(random.NextDouble() * variacionVelocidad + velocidadBase) ;
            this._velocidadInicial = this._velocidad;

            // Desaceleración más suave e independiente de la velocidad inicial
            this._desaceleracion = new Vector3(
                -Math.Sign(this._velocidad.X) * 0.8f,  // Valor fijo para desaceleración
                -Math.Sign(this._velocidad.Y) * 0.5f + -gravedad,  // Gravedad adicional en Y
                -Math.Sign(this._velocidad.Z) * 0.8f
            );

            this._vidaInicial = vida; // Vida inicial de la partícula
            this._vida = _vidaInicial;
            this._tamanio = (float)(random.NextDouble() * 0.5f + 0.1f);
        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//
        public void Dibujar(GraphicsDevice Graphics, IndexBuffer indexBuffer, VertexBuffer vertexBuffer)
        {

            this._efecto.Parameters["ParticlePosition"]?.SetValue(_coordenadas);
            this._efecto.Parameters["ParticleSize"]?.SetValue(_tamanio);

            Graphics.SetVertexBuffer(vertexBuffer);
            Graphics.Indices = indexBuffer;

            _efecto.CurrentTechnique = _efecto.Techniques[_tecnica];
            foreach (var pass in this._efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount);
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; // Tiempo transcurrido desde el último frame

            // Actualizar la posición de la partícula
            _velocidad += _desaceleracion * deltaTime; // Aplicar desaceleración
            _coordenadas += _velocidad * deltaTime;

            // Reducir la vida de la partícula
            _vida -= deltaTime;
            this._tamanio -= deltaTime * 0.6f; // Reducir tamaño con el tiempo
            if (_tamanio < 0.01f) // Evitar que el tamaño sea demasiado pequeño
            {
                _tamanio = 0.01f;
            }

            // Si la vida es menor o igual a cero, se puede eliminar o reiniciar la partícula
            if (_vida <= 0)
            {
                // Reiniciar o eliminar la partícula según sea necesario
                _vida = _vidaInicial; // Reiniciar vida para este ejemplo
                _coordenadas = _posicionInicial; // Reiniciar posición para este ejemplo
                _tamanio = 1f; // Reiniciar tamaño para este ejemplo
                _velocidad = _velocidadInicial; // Reiniciar velocidad a la inicial
            }
        }

        public void ActualizarPosicionInicial(Vector3 nuevaPosicion)
        {
            _posicionInicial = nuevaPosicion;
        }


        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public void cambioDeTecnica(string nombreTecnica)
        {
            _tecnica = nombreTecnica;
        }

        internal void ActualizarPosicion(Vector3 nuevaPosicion)
        {
            _coordenadas = nuevaPosicion; // Actualizar la posición actual de la partícula
        }
    }
}