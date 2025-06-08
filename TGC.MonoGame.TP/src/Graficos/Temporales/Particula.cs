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
        private float _vida = 0.3f;
        private float _tamanio = 0.1f; // Tamaño de la partícula
        Effect _efecto;

        private string _tecnica = "Default"; // Nombre de la técnica de sombreado a utilizar


        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Particula()
        {
        }
        // Inicializar recursos necesarios para el HUD
        public void Initialize(Vector3 coordenadas, Effect efecto, Random random)
        {
            _efecto = efecto;
            _efecto.Parameters["WorldViewProjection"]?.SetValue(Matrix.Identity);
            this._tecnica = "Particle";
            this._coordenadas = coordenadas;
            this._posicionInicial = coordenadas;

            // Velocidad aleatoria en forma esférica (explosión radial)
            Vector3 direction = new Vector3(
                (float)(random.NextDouble() * 2 - 1),
                (float)(random.NextDouble() * 2 - 1f),
                (float)(random.NextDouble() * 2 - 1)
            );
            direction.Normalize();
            
            this._velocidad = direction * (float)(random.NextDouble() * 2f + 2f); // Velocidad aleatoria entre 0.5 y 1.0
            this._velocidadInicial = this._velocidad; // Guardar la velocidad inicial
            this._desaceleracion = -this._velocidad * 1.5f; // Desaceleración basada en la velocidad inicial
            this._desaceleracion.Y += -0.4f; // Aumentar la desaceleración en Y para simular gravedad
            //this._vida = (float)(random.NextDouble() * 2.0f + 1.0f);
            this._tamanio = (float)(random.NextDouble() * 0.5f + 0.1f);
        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//
        public void Dibujar(GraphicsDevice Graphics, IndexBuffer indexBuffer, VertexBuffer vertexBuffer)
        {
            //_efecto.Parameters["PorcentaClaridad"]?.SetValue(_PorcentaClaridad);
            this._efecto.Parameters["World"]?.SetValue(Matrix.Identity * Matrix.CreateScale(10f) * Matrix.CreateTranslation(_coordenadas));
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
            // Actualizar la posición de la partícula
            //_velocidad.Y -= 0.3f * (float)gameTime.ElapsedGameTime.TotalSeconds; // Aumentar la velocidad en Y para simular gravedad
            _velocidad += _desaceleracion * (float)gameTime.ElapsedGameTime.TotalSeconds; // Aplicar desaceleración
            _coordenadas += _velocidad * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reducir la vida de la partícula
            _vida -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            this._tamanio -= (float)gameTime.ElapsedGameTime.TotalSeconds * 0.4f; // Reducir tamaño con el tiempo
            if (_tamanio < 0.01f) // Evitar que el tamaño sea demasiado pequeño
            {
                _tamanio = 0.01f;
            }

            // Si la vida es menor o igual a cero, se puede eliminar o reiniciar la partícula
            if (_vida <= 0)
            {
                // Reiniciar o eliminar la partícula según sea necesario
                _vida = 0.3f; // Reiniciar vida para este ejemplo
                _coordenadas = _posicionInicial; // Reiniciar posición para este ejemplo
                _tamanio = 0.2f; // Reiniciar tamaño para este ejemplo
                _velocidad = _velocidadInicial; // Reiniciar velocidad a la inicial
            }
        }

        public void ActualizarPosicionInicial(Vector3 nuevaPosicion)
        {
            _posicionInicial = nuevaPosicion;
        }


        public void ModificarPosicion(Vector3 nuevaPosicion)
        {
            _coordenadas += nuevaPosicion * _velocidad;
        }

        public void ModificarVelocidad(Vector3 nuevaVelocidad)
        {
            _velocidad = nuevaVelocidad;
        }




        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public void cambioDeTecnica(string nombreTecnica)
        {
            _tecnica = nombreTecnica;
        }
        
       

        
    }
}