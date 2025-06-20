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

            /*
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
                        */
            // Definición de vértices (sin normales iniciales)
            VertexPositionNormalTexture[] puntos = new VertexPositionNormalTexture[8];
            puntos[0] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 0f), Vector3.Zero, new Vector2(0, 0));
            puntos[1] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 0f), Vector3.Zero, new Vector2(1, 0));
            puntos[2] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 0f), Vector3.Zero, new Vector2(0, 1));
            puntos[3] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 0f), Vector3.Zero, new Vector2(1, 1));
            puntos[4] = new VertexPositionNormalTexture(new Vector3(0f, 0f, 1f), Vector3.Zero, new Vector2(0, 0));
            puntos[5] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f), Vector3.Zero, new Vector2(1, 0));
            puntos[6] = new VertexPositionNormalTexture(new Vector3(0f, 1f, 1f), Vector3.Zero, new Vector2(0, 1));
            puntos[7] = new VertexPositionNormalTexture(new Vector3(1f, 1f, 1f), Vector3.Zero, new Vector2(1, 1));

            // Inicializa listas para acumular normales
            Vector3[] normalesAcumuladas = new Vector3[8];
            int[] contadorNormales = new int[8];

            /*
                        ushort[] Indices = new ushort[]
                        {
                            0,1,2, 1,2,3, //Cara Trasera
                            4,5,6, 5,6,7, //Cara delantera
                            0,4,5, 0,1,5, //Cara abajo
                            2,6,7, 2,3,7, //Cara superior
                            7,5,1, 1,7,3, //Cara derecha
                            0,4,6, 0,6,2  //Cara izquierda
                        };
                        */
            ushort[] Indices = new ushort[]
 {
        // Cara Trasera (Z = 0)
        0, 2, 1, 1, 2, 3,
        
        // Cara Frontal (Z = 1)
        5, 6, 4, 5, 7, 6,
        
        // Cara Inferior (Y = 0)
        4, 1, 5, 4, 0, 1,
        
        // Cara Superior (Y = 1)
        2, 7, 3, 2, 6, 7,
        
        // Cara Derecha (X = 1)
        1, 7, 3, 1, 5, 7,
        
        // Cara Izquierda (X = 0)
        4, 6, 0, 0, 6, 2
 };

            // Normales por cara (fija para cada vértice de la cara)
            Vector3[] normalesPorCara = new Vector3[6];

            // Calcula normales para cada cara
            normalesPorCara[0] = Vector3.Backward;  // Trasera
            normalesPorCara[1] = Vector3.Forward;   // Frontal
            normalesPorCara[2] = Vector3.Down;      // Inferior
            normalesPorCara[3] = Vector3.Up;        // Superior
            normalesPorCara[4] = Vector3.Right;     // Derecha
            normalesPorCara[5] = Vector3.Left;      // Izquierda

            // Asigna normales a los vértices según su cara
            for (int i = 0; i < Indices.Length; i++)
            {
                int indiceCara = i / 6; // Cada cara tiene 6 índices (2 triángulos)
                puntos[Indices[i]].Normal = normalesPorCara[indiceCara];
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