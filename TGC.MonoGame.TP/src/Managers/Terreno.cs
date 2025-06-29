using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Graficos.Utils;
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
            base.Initialize(Graphics);
            this.inicializadorIluminacion();

        }

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content)
        {
            heightMap = Content.Load<Texture2D>("Models/heightmap/crater2");
            terrenoTexture = Content.Load<Texture2D>("Models/heightmap/pasto");
            heightData = LoadHeightData(heightMap);

            //this._Color = Color.SandyBrown.ToVector3();
            Graphics.SamplerStates[0] = SamplerState.LinearWrap;
            base.Initialize(Graphics, Matrix.CreateScale(5) * Mundo * Matrix.CreateTranslation(Vector3.Down * 10), Content);
            this.inicializadorIluminacion();
        }

        private void inicializadorIluminacion()
        {
            this._effect2.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._effect2.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._effect2.Parameters["specularColor"]?.SetValue(Color.White.ToVector3());
            this._effect2.Parameters["KAmbient"]?.SetValue(0.55f);
            this._effect2.Parameters["KDiffuse"]?.SetValue(3.0f);
            this._effect2.Parameters["KSpecular"]?.SetValue(0.5f);
            this._effect2.Parameters["shininess"]?.SetValue(16.0f);
            // configuracion de la luz ya que no molde
            this._effect2.Parameters["lightPosition"]?.SetValue(new Vector3(900, 400, -1000));
        }

        protected override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>("Effects/shaderTerreno");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//

        public override void Dibujar(GraphicsDevice Graphics, ShadowMapping shadowMap)
        {
            _effect2.CurrentTechnique = _effect2.Techniques["TextureDrawing"];
            Graphics.SetVertexBuffer(terrainVertexBuffer);
            if (terrainIndexBuffer == null || terrainIndexBuffer.IsDisposed)
            {
                terrainIndexBuffer = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
                terrainIndexBuffer.SetData(indices);
            }
            Graphics.Indices = terrainIndexBuffer;
            this.CargarShadowMapper(shadowMap);
            
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Texture"].SetValue(terrenoTexture);
            _effect2.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(this._matrixMundo)));

            foreach (EffectPass pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, (width - 1) * (height - 1) * 6 / 3);
            }
        }


        public override void DibujarShadowMap(GraphicsDevice Graphics , Matrix vista, Matrix proyeccion)
        {
            _effect2.CurrentTechnique = _effect2.Techniques["DepthPass"];
            Graphics.SetVertexBuffer(terrainVertexBuffer);
            if (terrainIndexBuffer == null || terrainIndexBuffer.IsDisposed)
            {
                terrainIndexBuffer = new IndexBuffer(Graphics, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);
                terrainIndexBuffer.SetData(indices);
            }
            Graphics.Indices = terrainIndexBuffer;
            Matrix worldViewProjection = this._matrixMundo * vista * proyeccion;

            _effect2.Parameters["WorldViewProjection"].SetValue(worldViewProjection);

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
            /*
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
                if (vertices[i].Normal != Vector3.Zero)
                    vertices[i].Normal.Normalize();
                //vertices[i].Normal = Vector3.UnitX* 100f;
                //vertices[i].Normal = vertices[i].Normal * 1.4f; // Ajustar la intensidad de la normal si es necesario
            }
            */

            // 1. Resetear normales
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = Vector3.Zero;
            }

            // 2. Calcular normales por triángulo
            for (int i = 0; i < indices.Length; i += 3)
            {
                int i1 = indices[i];
                int i2 = indices[i + 1];
                int i3 = indices[i + 2];

                Vector3 v1 = vertices[i1].Position;
                Vector3 v2 = vertices[i2].Position;
                Vector3 v3 = vertices[i3].Position;

                Vector3 edge1 = v2 - v1;
                Vector3 edge2 = v3 - v1;
                Vector3 normal = Vector3.Cross(edge1, edge2);

                // Solo agregar si la normal es válida
                if (normal != Vector3.Zero)
                {
                    vertices[i1].Normal += normal;
                    vertices[i2].Normal += normal;
                    vertices[i3].Normal += normal;
                }
            }

            // 3. Calcular normales basadas en la estructura de grid (método alternativo)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    // Solo calcular para vértices interiores
                    if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
                    {
                        float heightL = vertices[y * width + (x - 1)].Position.Y;
                        float heightR = vertices[y * width + (x + 1)].Position.Y;
                        float heightD = vertices[(y + 1) * width + x].Position.Y;
                        float heightU = vertices[(y - 1) * width + x].Position.Y;

                        /*
                        vertices[index].Normal = new Vector3(
                            heightL - heightR,  // Pendiente en X
                            0.3f,              // Factor de escala (ajustar según necesidad)
                            heightD - heightU   // Pendiente en Z
                        );
                        */
                        vertices[index].Normal = new Vector3(
                            heightD - heightU,   // Pendiente en Z
                            1.0f,              // Factor de escala (ajustar según necesidad)
                            heightL - heightR  // Pendiente en X
                        );
                    }

                    // Normalizar
                    if (vertices[index].Normal != Vector3.Zero)
                    {
                        vertices[index].Normal.Normalize();
                    }
                    else
                    {
                        vertices[index].Normal = Vector3.Up;
                    }
                }
            }

        }
        //Configuración de efectos tomados desde la clase padre


        public float GetHeightAt(float X, float Y)
        {
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

        public float getAltura(Vector2 pos1 , Vector2 pos2 , Vector2 pos3 )
        {
            float y1 = GetHeightAt(pos1.X,pos1.Y);
            float y2 = GetHeightAt(pos2.X,pos2.Y);
            float y3 = GetHeightAt(pos3.X,pos3.Y);

            return (y1+y2+y3)/3;
        }

        public Vector3 getNormal(Vector2 pos1 , Vector2 pos2 , Vector2 pos3 )
        {
            float y1 = GetHeightAt(pos1.X,pos1.Y);
            float y2 = GetHeightAt(pos2.X,pos2.Y);
            float y3 = GetHeightAt(pos3.X,pos3.Y);

            Vector2 verctor1AUX = pos1-pos2;
            Vector3 vector1 = new Vector3(verctor1AUX.X, y1-y2 , verctor1AUX.Y);
            
            Vector2 verctor2AUX = pos3-pos2;
            Vector3 vector2 = new Vector3(verctor2AUX.X, y3-y2 , verctor2AUX.Y);

            return Vector3.Cross(vector1, vector2);
        }
        public virtual void setCamara(Vector3 camaraPosition)
        {
            this._effect2.Parameters["eyePosition"]?.SetValue(camaraPosition);
        }


    }
}