using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Cajas
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class OCaja : Objetos.Objeto
    {
        
        // Variables
        Texture2D cajaTexture;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public OCaja(){}
        public override void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            this._matrixMundo = Matrix.Identity;
            this._matrixView = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            this._matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 250);

            base.Initialize(Graphics);

        }
        
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            //this._Color = new Vector2(0,0).ToVector3();
            cajaTexture = Content.Load<Texture2D>(@"Models/house/tablasMadera");
            base.Initialize(Graphics, Mundo, View, Projection, Content);
            //setear solo una vez la textura
            _effect2.Parameters["Texture"].SetValue(cajaTexture);

        }


        protected override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content){
            return Content.Load<Effect>(@"Effects/shaderCaja");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta
        //----------------------------------------------Dibujado--------------------------------------------------//

        public override void Dibujar(GraphicsDevice Graphics)
        {
            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this._indices.IndexCount);
            }
        }
        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        protected override void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionTexture[] puntos = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(0f, 0f, 0f), new Vector2(0,0)),
                new VertexPositionTexture(new Vector3(1f, 0f, 0f), new Vector2(1,0)),
                new VertexPositionTexture(new Vector3(0f, 1f, 0f), new Vector2(0,1)),
                new VertexPositionTexture(new Vector3(1f, 1f, 0f), new Vector2(1,1)),
                new VertexPositionTexture(new Vector3(0f, 0f, 1f), new Vector2(0,0)),
                new VertexPositionTexture(new Vector3(1f, 0f, 1f), new Vector2(1,0)),
                new VertexPositionTexture(new Vector3(0f, 1f, 1f), new Vector2(0,1)),
                new VertexPositionTexture(new Vector3(1f, 1f, 1f), new Vector2(1,1))
            };

            _vertices = new VertexBuffer(Graphics, VertexPositionTexture.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            ushort[] Indices = new ushort[]
            {
                0,1,2, 1,2,3, //Cara Trasera
                4,5,6, 5,6,7, //Cara delantera
                0,4,5, 0,1,5, //Cara abajo
                2,6,7, 2,3,7, //Cara superior
                7,5,1, 1,7,3, //Cara derecha
                0,4,6, 0,6,2  //Cara izquierda
            };

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, 36 , BufferUsage.None);
            _indices.SetData(Indices);
        }

        //Configuración de efectos tomados desde la clase padre
        
    }
}