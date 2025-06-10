using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using TGC.MonoGame.TP.src.Graficos.Temporales;

namespace TGC.MonoGame.TP.src.Graficos.Temporales
{

    public class EmisorParticula
    {
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private Effect _efecto;
        Texture2D _texture;
        //lista de partículas
        private List<Particula> _particulas = new List<Particula>();
        private GraphicsDevice _graphicsDevice;
        float _deltaTime;
        bool _puedeDibujar = false; // Bandera para controlar si se pueden dibujar las partículas
        private float _tiempoVida; // Tiempo de vida de las partículas, se puede ajustar según sea necesario
        private float _tiempoVidaInicial = 0.6f; // Tiempo de vida inicial de las partículas
        private Vector4 _colorParticula = new Vector4(Color.Red.ToVector3(), 0.8f); // Color de la partícula
        private Vector4 _colorParticulaInicial = new Vector4(Color.Red.ToVector3(), 0.8f); // Color de la partícula
        public EmisorParticula()
        {
        }
        public void Initialize(ContentManager Content, GraphicsDevice graphics, int cantidadParticulas, Vector3 posicionInicial)
        {
            // Crear el quad que se usará para las partículas
            this.CrearQuad(0.5f, graphics);

            _graphicsDevice = graphics;
            _efecto = Content.Load<Effect>(@"Effects/shaderParticula");
            _texture = Content.Load<Texture2D>(@"Textures/particula/particula");
            _efecto.Parameters["World"]?.SetValue(Matrix.Identity);
            _efecto.Parameters["Texture"].SetValue(_texture);
            _efecto.Parameters["ParticleSize"]?.SetValue(1f);
            _efecto.Parameters["ParticleColor"]?.SetValue(new Vector4(Color.Red.ToVector3(), 0.5f));

            this._tiempoVida = _tiempoVidaInicial; // Inicializar el tiempo de vida de las partículas

            // Inicializar las partículas
            this.InicializarParticulas(posicionInicial, cantidadParticulas, _tiempoVida);


        }

        public void Dibujar()
        {
            if (!_puedeDibujar)
            {
                return; // Si no se puede dibujar, salir del método
            }
            // Configurar el estado del gráfico
            _graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _graphicsDevice.BlendState = BlendState.AlphaBlend;

            // Establecer el VertexBuffer y IndexBuffer
            _graphicsDevice.SetVertexBuffer(_vertexBuffer);
            _graphicsDevice.Indices = _indexBuffer;

            // Dibujar cada partícula
            foreach (var particula in _particulas)
            {
                _efecto.Parameters["ParticleColor"]?.SetValue(_colorParticula);
                particula.Dibujar(_graphicsDevice, _indexBuffer, _vertexBuffer);
            }

            // Restaurar estados
            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Update(GameTime gameTime)
        {
            // Calcular el deltaTime
            _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Actualizar el tiempo de vida del emisor
            if (_puedeDibujar)
            {
                foreach (var particula in _particulas)
                {
                    particula.Update(gameTime);
                }
                _tiempoVida -= _deltaTime;
                _colorParticula = Vector4.Lerp(_colorParticulaInicial, new Vector4(Color.Yellow.ToVector3(), 0.4f), 1 - (_tiempoVida / _tiempoVidaInicial));
            }

            if (_tiempoVida <= 0)
            {
                _puedeDibujar = false; // Desactivar el dibujado si el tiempo de vida es 0 o menor
                _tiempoVida = _tiempoVidaInicial; // Reiniciar el tiempo de vida si es necesario
                _colorParticula = _colorParticulaInicial; // Reiniciar el color de la partícula
            }
        }

        public void SetNuevaPosicion(Vector3 nuevaPosicion)
        {
            // Actualizar la posición inicial de todas las partículas
            foreach (var particula in _particulas)
            {
                particula.ActualizarPosicionInicial(nuevaPosicion);
            }
        }
        public void SetPosiciones(Vector3 nuevaPosicion)
        {
            // Actualizar la posición inicial de todas las partículas
            foreach (var particula in _particulas)
            {
                particula.ActualizarPosicion(nuevaPosicion);
            }
        }
        public void InicializarParticulas(Vector3 posicionInicial, int cantidadParticulas, float tiempoVida)
        {
            // Limpiar la lista de partículas
            _particulas.Clear();

            // Crear un generador de números aleatorios
            Random random = new Random();

            // Crear nuevas partículas
            for (int i = 0; i < cantidadParticulas; i++)
            {
                var particula = new Particula();
                particula.Initialize(posicionInicial, _efecto, random, tiempoVida);
                _particulas.Add(particula);
            }
        }

        public void SetPuedeDibujar(bool puedeDibujar)
        {
            _puedeDibujar = puedeDibujar; // Actualizar la bandera para controlar el dibujado
        }

        public void SetVistaProyeccion(Matrix Vista, Matrix Proyeccion)
        {
            _efecto.Parameters["View"]?.SetValue(Vista);
            _efecto.Parameters["Projection"]?.SetValue(Proyeccion);
        }
        public void SetQuad(float halfSize, GraphicsDevice device)
        {
            CrearQuad(halfSize, device);
        }
        private void CrearQuad(float halfSize, GraphicsDevice _graphicsDevice)
        {
            // 1. Definir los vértices (4 vértices para un quad)
            var vertices = new VertexPositionNormalTexture[4];

            // Tamaño del quad (1 unidad de ancho y alto)
            //float halfSize = 0.025f;

            // Coordenadas de los vértices (en sentido horario)
            vertices[0] = new VertexPositionNormalTexture(
                new Vector3(-halfSize, -halfSize, 0), // Posición (inferior izquierda)
                Vector3.UnitZ,                        // Normal (apuntando hacia la cámara)
                new Vector2(0, 1));                   // Coordenadas UV

            vertices[1] = new VertexPositionNormalTexture(
                new Vector3(-halfSize, halfSize, 0),  // Superior izquierda
                Vector3.UnitZ,
                new Vector2(0, 0));

            vertices[2] = new VertexPositionNormalTexture(
                new Vector3(halfSize, -halfSize, 0),  // Inferior derecha
                Vector3.UnitZ,
                new Vector2(1, 1));

            vertices[3] = new VertexPositionNormalTexture(
                new Vector3(halfSize, halfSize, 0),   // Superior derecha
                Vector3.UnitZ,
                new Vector2(1, 0));

            // 2. Crear VertexBuffer
            _vertexBuffer = new VertexBuffer(
                _graphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly);

            _vertexBuffer.SetData(vertices);

            // 3. Definir índices (2 triángulos = 6 índices)
            var indices = new short[6];

            // Primer triángulo (inferior izquierda, superior izquierda, inferior derecha)
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            // Segundo triángulo (superior izquierda, superior derecha, inferior derecha)
            indices[3] = 1;
            indices[4] = 3;
            indices[5] = 2;

            // 4. Crear IndexBuffer
            _indexBuffer = new IndexBuffer(
                _graphicsDevice,
                IndexElementSize.SixteenBits,
                indices.Length,
                BufferUsage.WriteOnly);

            _indexBuffer.SetData(indices);
        }

    }

}