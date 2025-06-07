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
        public EmisorParticula()
        {
        }
        public void Initialize(ContentManager Content, GraphicsDevice graphics, int cantidadParticulas, Vector3 posicionInicial, Vector3 velocidadInicial)
        {
            // Crear el quad que se usará para las partículas
            crearQuad(0.2f, graphics);

            _graphicsDevice = graphics;
            _efecto = Content.Load<Effect>(@"Effects/shaderParticula");
            _texture = Content.Load<Texture2D>(@"Textures/particula/particula");
            _efecto.Parameters["WorldViewProjection"]?.SetValue(Matrix.Identity);
            _efecto.Parameters["Texture"].SetValue(_texture);
            _efecto.Parameters["ParticleSize"]?.SetValue(0.2f);
            _efecto.Parameters["ParticleColor"]?.SetValue(Color.DimGray.ToVector4());

            // Inicializar las partículas
            this.InicializarParticulas(posicionInicial, velocidadInicial, cantidadParticulas);
        }

        public void Dibujar()
        {
            // Configurar el estado del gráfico
            _graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            _graphicsDevice.BlendState = BlendState.AlphaBlend;

            // Establecer el VertexBuffer y IndexBuffer
            _graphicsDevice.SetVertexBuffer(_vertexBuffer);
            _graphicsDevice.Indices = _indexBuffer;

            // Dibujar cada partícula
            foreach (var particula in _particulas)
            {
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
            // Actualizar cada partícula
            foreach (var particula in _particulas)
            {
                particula.Update(gameTime);
            }
        }

        public void InicializarParticulas(Vector3 posicionInicial, Vector3 velocidadInicial, int cantidadParticulas)
        {
            // Limpiar la lista de partículas
            _particulas.Clear();

            // Crear nuevas partículas
            for (int i = 0; i < cantidadParticulas; i++)
            {
                var particula = new Particula();
                particula.Initialize(posicionInicial, velocidadInicial, _efecto);
                _particulas.Add(particula);
            }
        }

        public void SetVistaProyeccion(Matrix Vista, Matrix Proyeccion)
        {
            _efecto.Parameters["View"]?.SetValue(Vista);
            _efecto.Parameters["Projection"]?.SetValue(Proyeccion);
        }

        private void crearQuad(float halfSize, GraphicsDevice _graphicsDevice)
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