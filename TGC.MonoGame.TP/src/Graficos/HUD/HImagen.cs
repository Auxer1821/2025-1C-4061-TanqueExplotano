using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.HUD
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class HImagen
    {
        private Vector2 _coordenadas;
        Texture2D _texture;
        Effect _efecto;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private string _tecnica;
        private float _PorcentaClaridad;


        //----------------------------------------------Variables--------------------------------------------------//

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public HImagen()
        {
        }
        // Inicializar recursos necesarios para el HUD
        public void Initialize(Vector2 coordenadas, ContentManager Content, string path)
        {
            _efecto = Content.Load<Effect>(@"Effects/shaderHUDImagen");
            _texture = Content.Load<Texture2D>(@path); 
            _efecto.Parameters["Texture"].SetValue(_texture);
            _efecto.Parameters["WorldViewProjection"]?.SetValue(Matrix.Identity);
            _PorcentaClaridad = 1.0f;
            this._tecnica = "Default";
            this._coordenadas = coordenadas;
        }

        public void setQuad(float halfSize, GraphicsDevice device){
            crearQuad(halfSize, device);
        }

        public void setClaridad(float claridad){
            _PorcentaClaridad = claridad;
        }


        //----------------------------------------------Funciones-Principales--------------------------------------------------//


        //----------------------------------------------Dibujado--------------------------------------------------//
        public void Dibujado(GraphicsDevice Graphics)
        {
            _efecto.Parameters["PorcentaClaridad"]?.SetValue(_PorcentaClaridad);
            _efecto.Parameters["Texture"].SetValue(_texture);
            this._efecto.Parameters["Coordenadas"].SetValue(this._coordenadas);

            Graphics.SetVertexBuffer(_vertexBuffer);
            Graphics.Indices = _indexBuffer;

            _efecto.CurrentTechnique = _efecto.Techniques[_tecnica];
            foreach (var pass in this._efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _indexBuffer.IndexCount);

            }

        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        public void cambioDeTecnica(string nombreTecnica)
        {
            _tecnica = nombreTecnica;
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