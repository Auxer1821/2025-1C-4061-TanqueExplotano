using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Pastos
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    
    //cambio de objeto a modelo, para poder usar el modelo de arbol
    public class OPasto : Objetos.Objeto
    {
        
        // Variables
        Texture2D pastoTexture;
        float tamano = 5f;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public OPasto(){}
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.LightBlue.ToVector3();
            pastoTexture = Content.Load<Texture2D>("Models/heightmap/pastoAlto");
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        protected override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>("Effects/shaderPasto");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {
            Graphics.BlendState = BlendState.AlphaBlend;
            Graphics.DepthStencilState = DepthStencilState.DepthRead;
            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["Texture"].SetValue(pastoTexture);
            //_effect2.Parameters["Time"].SetValue((float)gameTime.ElapsedGameTime.TotalSeconds);

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this._indices.IndexCount);
            }

            // Restaurar estados
            Graphics.BlendState = BlendState.Opaque;
            Graphics.DepthStencilState = DepthStencilState.Default;
        }
        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//

        protected override void ConfigPuntos(GraphicsDevice Graphics)
        {
            // Vértices compartidos (8 vértices únicos para ambos planos)
            VertexPositionTexture[] puntos = new VertexPositionTexture[8];

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

        

        //Configuración de efectos tomados desde la clase padre
        
    }
}