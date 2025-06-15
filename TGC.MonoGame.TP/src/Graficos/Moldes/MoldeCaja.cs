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
            this._efecto.Parameters["ambientColor"].SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"].SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"].SetValue(Color.White.ToVector3());
            this._efecto.Parameters["KAmbient"].SetValue(0.5f);
            this._efecto.Parameters["KDiffuse"].SetValue(1.0f);
            this._efecto.Parameters["KSpecular"].SetValue(0.8f);
            this._efecto.Parameters["shininess"].SetValue(16.0f);
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


        
        private  void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionNormalTexture[] puntos = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f),Vector3.Zero, new Vector2(0,0)),
                new VertexPositionNormalTexture(new Vector3(1f, 0f, 0f),Vector3.Zero, new Vector2(1,0)),
                new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f),Vector3.Zero, new Vector2(0,1)),
                new VertexPositionNormalTexture(new Vector3(1f, 1f, 0f),Vector3.Zero, new Vector2(1,1)),
                new VertexPositionNormalTexture(new Vector3(0f, 0f, 1f),Vector3.Zero, new Vector2(0,0)),
                new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f),Vector3.Zero, new Vector2(1,0)),
                new VertexPositionNormalTexture(new Vector3(0f, 1f, 1f),Vector3.Zero, new Vector2(0,1)),
                new VertexPositionNormalTexture(new Vector3(1f, 1f, 1f),Vector3.Zero, new Vector2(1,1))
            };

            // Inicializa listas para acumular normales
            Vector3[] normalesAcumuladas = new Vector3[8];
            int[] contadorNormales = new int[8];

            ushort[] Indices = new ushort[]
            {
                0,1,2, 1,2,3, //Cara Trasera
                4,5,6, 5,6,7, //Cara delantera
                0,4,5, 0,1,5, //Cara abajo
                2,6,7, 2,3,7, //Cara superior
                7,5,1, 1,7,3, //Cara derecha
                0,4,6, 0,6,2  //Cara izquierda
            };

            // Calcula normales por triángulo y acumula
            for (int i = 0; i < Indices.Length; i += 3)
            {
                ushort i0 = Indices[i];
                ushort i1 = Indices[i + 1];
                ushort i2 = Indices[i + 2];

                Vector3 normal = CalculateNormal(
                    puntos[i0].Position,
                    puntos[i1].Position,
                    puntos[i2].Position);

                // Acumula normales para cada vértice del triángulo
                normalesAcumuladas[i0] += normal;
                normalesAcumuladas[i1] += normal;
                normalesAcumuladas[i2] += normal;

                contadorNormales[i0]++;
                contadorNormales[i1]++;
                contadorNormales[i2]++;
            }

            // Promedia y normaliza
            for (int i = 0; i < puntos.Length; i++)
            {
                if (contadorNormales[i] > 0)
                {
                    puntos[i].Normal = Vector3.Normalize(normalesAcumuladas[i] / contadorNormales[i]);
                }
            }

            _vertices = new VertexBuffer(Graphics, VertexPositionNormalTexture.VertexDeclaration, puntos.Length, BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, 36, BufferUsage.None);
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