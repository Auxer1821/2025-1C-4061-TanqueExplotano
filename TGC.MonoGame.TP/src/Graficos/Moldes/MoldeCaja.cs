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
    public class MoldeCaja : IMolde
    {
        private IndexBuffer _indices;
        private VertexBuffer _vertices;
        Texture2D cajaTexture;
        public MoldeCaja(ContentManager Content, GraphicsDevice Graphics){
            this._efecto = Content.Load<Effect>(@"Effects/shaderCaja");
            this.cajaTexture = Content.Load<Texture2D>(@"Models/house/tablasMadera");
            this._efecto.Parameters["Texture"].SetValue(cajaTexture);
            this._efecto.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["KAmbient"]?.SetValue(0.5f);
            this._efecto.Parameters["KDiffuse"]?.SetValue(1.0f);
            this._efecto.Parameters["KSpecular"]?.SetValue(0.8f);
            this._efecto.Parameters["shininess"]?.SetValue(16.0f);
            this.ConfigPuntos(Graphics);
        }
        public override void Draw(Matrix Mundo, GraphicsDevice Graphics){

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            _efecto.Parameters["World"].SetValue(Mundo);
            _efecto.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(Mundo)));

            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this._indices.IndexCount);
            }
        }



        private void ConfigPuntos(GraphicsDevice Graphics)
        {
            // Definición de vértices
            VertexPositionNormalTexture[] puntos = new VertexPositionNormalTexture[24]; // 6 caras * 4 vértices cada una

            // Cara Trasera (Z = 0)
            puntos[0] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), Vector3.Backward, new Vector2(0, 0));
            puntos[1] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 0f), Vector3.Backward, new Vector2(1, 0));
            puntos[2] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), Vector3.Backward, new Vector2(0, 1));
            puntos[3] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 0f), Vector3.Backward, new Vector2(1, 1));

            // Cara Frontal (Z = 1)
            puntos[4] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 1f), Vector3.Forward, new Vector2(0, 0));
            puntos[5] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f), Vector3.Forward, new Vector2(1, 0));
            puntos[6] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 1f), Vector3.Forward, new Vector2(0, 1));
            puntos[7] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 1f), Vector3.Forward, new Vector2(1, 1));

            // Cara Inferior (Y = 0)
            puntos[8] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), Vector3.Down, new Vector2(0, 0));
            puntos[9] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 0f), Vector3.Down, new Vector2(1, 0));
            puntos[10] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 1f), Vector3.Down, new Vector2(0, 1));
            puntos[11] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f), Vector3.Down, new Vector2(1, 1));

            // Cara Superior (Y = 1)
            puntos[12] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), Vector3.Up, new Vector2(0, 0));
            puntos[13] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 0f), Vector3.Up, new Vector2(1, 0));
            puntos[14] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 1f), Vector3.Up, new Vector2(0, 1));
            puntos[15] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 1f), Vector3.Up, new Vector2(1, 1));

            // Cara Derecha (X = 1)
            puntos[16] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 0f), Vector3.Right, new Vector2(0, 0));
            puntos[17] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 0f), Vector3.Right, new Vector2(1, 0));
            puntos[18] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f), Vector3.Right, new Vector2(0, 1));
            puntos[19] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 1f), Vector3.Right, new Vector2(1, 1));

            // Cara Izquierda (X = 0)
            puntos[20] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), Vector3.Left, new Vector2(0, 0));
            puntos[21] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), Vector3.Left, new Vector2(1, 0));
            puntos[22] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 1f), Vector3.Left, new Vector2(0, 1));
            puntos[23] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 1f), Vector3.Left, new Vector2(1, 1));

            ushort[] Indices = new ushort[36];

            // Índices para cada cara (2 triángulos por cara)
            for (int i = 0; i < 6; i++)
            {
                int baseIndex = i * 4;
                int indicesBase = i * 6;

                Indices[indicesBase] = (ushort)(baseIndex);
                Indices[indicesBase + 1] = (ushort)(baseIndex + 1);
                Indices[indicesBase + 2] = (ushort)(baseIndex + 2);

                Indices[indicesBase + 3] = (ushort)(baseIndex + 1);
                Indices[indicesBase + 4] = (ushort)(baseIndex + 3);
                Indices[indicesBase + 5] = (ushort)(baseIndex + 2);
            }

            _vertices = new VertexBuffer(Graphics, VertexPositionNormalTexture.VertexDeclaration, puntos.Length, BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, Indices.Length, BufferUsage.WriteOnly);
            _indices.SetData(Indices);
        }
        Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            return Vector3.Cross(ab, ac);
        }

    }
}