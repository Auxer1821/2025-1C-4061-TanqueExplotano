using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Terrenos
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class Terreno : Objetos.Objeto
    {
        // Variables
        Texture2D heightMap;
        Texture2D terrenoTexture;
        float[,] heightData;
        VertexPositionNormalTexture[] vertices;
        int[] indices;
        VertexBuffer terrainVertexBuffer;
        IndexBuffer terrainIndexBuffer;
        int width;
        int height;
        //  Todas en Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            this._matrixMundo = Matrix.Identity ;
            this._matrixView = Matrix.CreateLookAt(new Vector3(0, 50, 150), Vector3.Zero, Vector3.Up);
            this._matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 2500);

            base.Initialize(Graphics);

        }

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            heightMap = Content.Load<Texture2D>("Models/heightmap/crater2");
            terrenoTexture = Content.Load<Texture2D>("Models/heightmap/pasto");
            heightData = LoadHeightData(heightMap);
            //this._Color = Color.SandyBrown.ToVector3();
            Graphics.SamplerStates[0] = SamplerState.LinearWrap;
            base.Initialize(Graphics, Matrix.CreateScale(5) * Mundo * Matrix.CreateTranslation(Vector3.Down * 10), View, Projection, Content);
        }

        protected override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>("Effects/shaderTerreno");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//

        public override void Dibujar(GraphicsDevice Graphics)
        {
            Graphics.SetVertexBuffer(terrainVertexBuffer);
            if (terrainIndexBuffer == null || terrainIndexBuffer.IsDisposed)
            {
                terrainIndexBuffer = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
                terrainIndexBuffer.SetData(indices);
            }
            Graphics.Indices = terrainIndexBuffer;

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Texture"].SetValue(terrenoTexture);

            Graphics.SamplerStates[0] = new SamplerState
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                Filter = TextureFilter.Anisotropic,
                MaxAnisotropy = 16, // Valor típico para buen equilibrio calidad/rendimiento
                MipMapLevelOfDetailBias = -0.5f // Ajusta este valor según necesites
            };

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

        public float GetHeightAt(float X, float Y)
        {
            /*
            
            int width = heightData.GetLength(0);
    int height = heightData.GetLength(1);
    
    // Estos valores deben coincidir con la escala de tu terreno en el mundo
    float terrainWidth = (width - 1) * 0.1f * 5f; // Considerando tu escala de 0.1 y luego 5
    float terrainHeight = (height - 1) * 0.1f * 5f;
    
    // Convertir coordenadas del mundo a coordenadas del heightmap
    float normalizedX = (x + terrainWidth / 2f) / terrainWidth;
    float normalizedZ = (z + terrainHeight / 2f) / terrainHeight;
    
    // Asegurarse de que estén en el rango [0, 1]
    normalizedX = MathHelper.Clamp(normalizedX, 0f, 1f);
    normalizedZ = MathHelper.Clamp(normalizedZ, 0f, 1f);
    
    // Convertir a coordenadas del heightmap
    int xIndex = (int)(normalizedX * (width - 1));
    int zIndex = (int)(normalizedZ * (height - 1));
    
    // Devolver la altura
    return heightData[xIndex, zIndex];
            */

    // 1. Obtener dimensiones del heightmap
    int heightmapWidth = heightData.GetLength(0);
    int heightmapHeight = heightData.GetLength(1);
    
    // 2. Calcular dimensiones REALES del terreno (considerando escalas 0.1f y 5f)
    float terrainWorldWidth = (heightmapWidth - 1) *5f;  // Escala total: 0.5f
    float terrainWorldHeight = (heightmapHeight - 1) *5f;
    
    // 3. Convertir coordenadas del mundo a coordenadas del heightmap
    float normalizedX = (X + terrainWorldWidth / 2f) / terrainWorldWidth;
    float normalizedZ = (Y + terrainWorldHeight / 2f) / terrainWorldHeight;
    
    // 4. Asegurarse de que están en el rango [0, 1]
    normalizedX = MathHelper.Clamp(normalizedX, 0f, 1f);
    normalizedZ = MathHelper.Clamp(normalizedZ, 0f, 1f);
    
    // 5. Convertir a índices del heightmap
    int xIndex = (int)(normalizedX * (heightmapWidth - 1));
    int zIndex = (int)(normalizedZ * (heightmapHeight - 1));
    
    // 6. Devolver la altura, RESTANDO el desplazamiento hacia abajo (10 unidades)
    return heightData[xIndex, zIndex] * 5f -10f;  // Ajuste clave aquí
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
                    //posicion 3d
                    Vector3 position = new Vector3(
                        x - width / 2,
                        heightData[x, y],
                        y - height / 2
                    );
                    // Coordenadas de textura (ajusta textureRepeatFactor según necesites)
                    float textureRepeatFactor = 0.1f;
                    Vector2 texCoord = new Vector2(
                        x * textureRepeatFactor,
                        y * textureRepeatFactor);
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

            // Calcular normales
            CalculateNormals(vertices, indices);

            // Crear el vertex buffer

            terrainVertexBuffer = new VertexBuffer(Graphics, typeof(VertexPositionNormalTexture), width * height, BufferUsage.WriteOnly);
            terrainVertexBuffer.SetData(vertices);
           
        }

        protected void CalculateNormals(VertexPositionNormalTexture[] vertices, int[] indices)
        {
            // Resetear normales
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = Vector3.Zero;
            }

            // Calcular normales por triángulo
            for (int i = 0; i < indices.Length; i += 3)
            {
                int index1 = indices[i];
                int index2 = indices[i + 1];
                int index3 = indices[i + 2];

                Vector3 side1 = vertices[index2].Position - vertices[index1].Position;
                Vector3 side2 = vertices[index3].Position - vertices[index1].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            // Normalizar
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
            }
        }
        //Configuración de efectos tomados desde la clase padre





    }
}