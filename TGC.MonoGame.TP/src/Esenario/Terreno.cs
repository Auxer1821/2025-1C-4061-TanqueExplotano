using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Terrenos
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Terreno : Objetos.Objeto
    {
        // Variables
        Texture2D heightMap;
        float[,] heightData;
        VertexPositionNormalTexture[] vertices;
        int[] indices;
        VertexBuffer terrainVertexBuffer;
        int width;
        int height;
        //  Todas en Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            this._matrixMundo = Matrix.Identity * Matrix.CreateScale(0.1f);
            this._matrixView = Matrix.CreateLookAt(new Vector3(0, 50, 150), Vector3.Zero, Vector3.Up);
            this._matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 2500);

            base.Initialize(Graphics);

        }

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            heightMap = Content.Load<Texture2D>("Models/heightmap/crater2");
            heightData = LoadHeightData(heightMap);
            this._Color = Color.SandyBrown.ToVector3();
            base.Initialize(Graphics, Matrix.CreateScale(5) * Mundo * Matrix.CreateTranslation(Vector3.Down * 10), View, Projection, Content);
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        
        public override void Dibujar(GraphicsDevice Graphics)
        {
            Graphics.SetVertexBuffer(terrainVertexBuffer);
            Graphics.Indices = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, (width - 1) * (height - 1) * 6, BufferUsage.WriteOnly);
            Graphics.Indices.SetData(indices);

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["DiffuseColor"].SetValue(this._Color);

            foreach (EffectPass pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (width - 1) * (height - 1) * 6 / 3);
            }
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//

        private float[,] LoadHeightData(Texture2D heightMap)
        {
            width = heightMap.Width;
            height = heightMap.Height;
            float[,] data = new float[width, height];

            Color[] colorData = new Color[width * height];
            heightMap.GetData(colorData);

            //recorremos la textura y convertimos el valor de gris a altura
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Convertir el valor de gris (0-255) a altura (0.0-1.0)
                    float heightValue = colorData[y * width + x].R / 255.0f;
                    data[x, y] = heightValue * 30.0f; // Escalar la altura (ajurtar según sea necesario)
                }
            }
            return data;
        }
        

        protected override void ConfigPuntos(GraphicsDevice Graphics)
        {
            width = heightData.GetLength(0);
            height = heightData.GetLength(1);
            vertices = new VertexPositionNormalTexture[width * height];
            indices = new int[(width - 1) * (height - 1) * 6];

            // Generar vértices
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 position = new Vector3(
                        x - width / 2,
                        heightData[x, y],
                        y - height / 2
                    );
                    Vector2 texCoord = new Vector2(x / (float)width, y / (float)height);
                    vertices[y * width + x] = new VertexPositionNormalTexture(position, Vector3.Up, texCoord);
                }
            }

            // Generar índices (triángulos)
            int index = 0;
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int topLeft = y * width + x;
                    int topRight = topLeft + 1;
                    int bottomLeft = (y + 1) * width + x;
                    int bottomRight = bottomLeft + 1;

                    indices[index++] = topLeft;
                    indices[index++] = bottomLeft;
                    indices[index++] = topRight;

                    indices[index++] = topRight;
                    indices[index++] = bottomLeft;
                    indices[index++] = bottomRight;
                }
            }

            // Crear el vertex buffer

            terrainVertexBuffer = new VertexBuffer(Graphics, typeof(VertexPositionNormalTexture), width * height, BufferUsage.WriteOnly);
            terrainVertexBuffer.SetData(vertices);
           
        }
        //Configuración de efectos tomados desde la clase padre





    }
}