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
        private Vector3 _coordenadas;
        private Vector3 _velocidad;
        private float _vida = 3.0f;
        Effect _efecto;

        private string _tecnica = "Default"; // Nombre de la técnica de sombreado a utilizar
        private float _PorcentaClaridad;


        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Particula()
        {
        }
        // Inicializar recursos necesarios para el HUD
        public void Initialize(Vector3 coordenadas, Vector3 velocidad, Effect efecto)
        {
            _efecto = efecto;
            _efecto.Parameters["WorldViewProjection"]?.SetValue(Matrix.Identity);
            _PorcentaClaridad = 1.0f;
            this._tecnica = "Particle";
            this._coordenadas = coordenadas;
            // Variación aleatoria en velocidad (ajusta los valores según necesites)
            float variacionX = (float)(new Random().NextDouble() * 0.4f - 0.2f); // -0.2 a +0.2 en X
            float variacionY = (float)(new Random().NextDouble() * 0.5f + 0.5f); // +0.5 a +1.0 en Y (para que suban)
            float variacionZ = (float)(new Random().NextDouble() * 0.4f - 0.2f); // -0.2 a +0.2 en Z

            this._velocidad = new Vector3(
                velocidad.X + variacionX,
                velocidad.Y + variacionY, // Asegura que Y sea positivo (para que suban)
                velocidad.Z + variacionZ
            );
        }

        //----------------------------------------------Funciones-Principales--------------------------------------------------//
        public void Dibujar(GraphicsDevice Graphics, IndexBuffer indexBuffer, VertexBuffer vertexBuffer)
        {
            //_efecto.Parameters["PorcentaClaridad"]?.SetValue(_PorcentaClaridad);
            this._efecto.Parameters["ParticlePosition"]?.SetValue(_coordenadas);

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
            _coordenadas += _velocidad * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reducir la vida de la partícula
            _vida -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Si la vida es menor o igual a cero, se puede eliminar o reiniciar la partícula
            if (_vida <= 0)
            {
                // Reiniciar o eliminar la partícula según sea necesario
                _vida = 3.0f; // Reiniciar vida para este ejemplo
                _coordenadas = Vector3.Zero; // Reiniciar posición para este ejemplo
            }
        }


        public void SetClaridad(float claridad)
        {
            _PorcentaClaridad = claridad;
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