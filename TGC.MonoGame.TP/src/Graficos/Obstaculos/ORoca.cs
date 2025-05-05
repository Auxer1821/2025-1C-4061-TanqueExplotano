using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Rocas
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class ORoca : Objetos.Objeto
    {
        
        // Variables
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public ORoca(){}
        public override void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            this._Color = Color.Yellow.ToVector3();
            this._matrixMundo = Matrix.Identity;
            this._matrixView = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            this._matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 250);

            base.Initialize(Graphics);

        }

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.Yellow.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//

        public override void Dibujar(GraphicsDevice Graphics)
        {
            

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["DiffuseColor"].SetValue(this._Color);

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,12);
            }
            
        }
        

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        protected override void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-1f, 0f, -1f), Color.Yellow),
                new VertexPositionColor(new Vector3(1f, 0f, -1f), Color.Yellow),
                new VertexPositionColor(new Vector3(-1f, 0f, 1f), Color.Yellow),
                new VertexPositionColor(new Vector3(1f, 0f, 1f), Color.Yellow),
                new VertexPositionColor(new Vector3(0f, 2f, 0f), Color.Yellow)
            };

            _vertices = new VertexBuffer(Graphics, VertexPositionColor.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            ushort[] Indices = new ushort[]
            {
                0,1,2, 1,2,3, //Cara Piso
                0,4,2, //Cara izq
                0,4,1, //Cara trasera
                1,4,3, //Cara der
                2,4,3, //Cara delantera
            };

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, 18 , BufferUsage.None);
            _indices.SetData(Indices);
        }

        //Configuración de efectos tomados desde la clase padre
        
    }
}