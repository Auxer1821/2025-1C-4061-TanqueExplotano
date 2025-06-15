using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.Modelos;



namespace TGC.MonoGame.TP.src.Moldes
{
    /// <summary>
    ///     Esta es la clase donde se guarda el  modelo; efecto y otras cosas que comparten las entidades iguale para dibujar 
    /// </summary>
    public class MoldePasto : IMolde
    {
        private Texture2D pastoTexture;        
        private IndexBuffer _indices;
        private VertexBuffer _vertices;
        public MoldePasto(ContentManager Content, GraphicsDevice Graphics) {
            this._efecto = Content.Load<Effect>(@"Effects/shaderPasto");
            this.pastoTexture = Content.Load<Texture2D>(@"Models/heightmap/pastoAlto");
            this._efecto.Parameters["Texture"].SetValue(pastoTexture);
            this._efecto.Parameters["WindSpeed"].SetValue(1.8f);
            this._efecto.Parameters["WindStrength"].SetValue(0.5f);
            this._efecto.Parameters["GrassStiffness"].SetValue(0.3f);
            this.ConfigPuntos(Graphics);
        }
        public override void Draw(Matrix Mundo, GraphicsDevice Graphics){
            Graphics.DepthStencilState = DepthStencilState.DepthRead;
            Graphics.BlendState = BlendState.AlphaBlend;

            _efecto.Parameters["World"].SetValue(Mundo);

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this._indices.IndexCount);
            }

            // Restaurar estados
            Graphics.BlendState = BlendState.Opaque;
            Graphics.DepthStencilState = DepthStencilState.Default;
        }

        public override void setTime(GameTime time)
        {
            // Aquí podrías actualizar parámetros relacionados con el tiempo si es necesario
            // Por ejemplo, podrías modificar la velocidad del viento o la fuerza del viento en función del tiempo
            _efecto.Parameters["Time"].SetValue((float)time.TotalGameTime.TotalSeconds);
        }

        private void ConfigPuntos(GraphicsDevice Graphics)
        {
            // Vértices compartidos (8 vértices únicos para ambos planos)
            VertexPositionTexture[] puntos = new VertexPositionTexture[8];
            float tamano = 5f;

            // Plano 1 (vertical)
            puntos[0] = new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0) * tamano, new Vector2(0, 1));
            puntos[1] = new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0) * tamano, new Vector2(0, 0));
            puntos[2] = new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0) * tamano, new Vector2(1, 1));
            puntos[3] = new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0) * tamano, new Vector2(1, 0));

            // Plano 2 (horizontal)
            puntos[4] = new VertexPositionTexture(new Vector3(0, -0.5f, -0.5f) * tamano, new Vector2(0, 1));
            puntos[5] = new VertexPositionTexture(new Vector3(0, 0.5f, -0.5f) * tamano, new Vector2(0, 0));
            puntos[6] = new VertexPositionTexture(new Vector3(0, -0.5f, 0.5f) * tamano, new Vector2(1, 1));
            puntos[7] = new VertexPositionTexture(new Vector3(0, 0.5f, 0.5f) * tamano, new Vector2(1, 0));


            // Índices para los triángulos (12 triángulos × 3 véndices = 36 índices)
            short[] Indices = new short[12 * 3];

            // Primer plano (2 triángulos)
            Indices[0] = 0; Indices[1] = 1; Indices[2] = 2; // Triángulo 1
            Indices[3] = 1; Indices[4] = 3; Indices[5] = 2; // Triángulo 2

            // Segundo plano (2 triángulos)
            Indices[6] = 4; Indices[7] = 5; Indices[8] = 6;   // Triángulo 3
            Indices[9] = 5; Indices[10] = 7; Indices[11] = 6; // Triángulo 4

            _vertices = new VertexBuffer(Graphics, VertexPositionTexture.VertexDeclaration, puntos.Length, BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, Indices.Length, BufferUsage.None);
            _indices.SetData(Indices);
        }

        public override void setCamara(Vector3 position)
        {
        }
        public override void SetPosSOL(Vector3 position){

        }



    }
}