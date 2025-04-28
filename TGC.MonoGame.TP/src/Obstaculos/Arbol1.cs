using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Casa
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Arbol1 : Objetos.Objetos
    {
        
        // Variables
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
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
            this._Color = Color.Green.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        protected override void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0f, 0f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(1f, 0f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(0f, 3f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(1f, 3f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(0f, 0f, 1f), Color.Green),
                new VertexPositionColor(new Vector3(1f, 0f, 1f), Color.Green),
                new VertexPositionColor(new Vector3(0f, 3f, 1f), Color.Green),
                new VertexPositionColor(new Vector3(1f, 3f, 1f), Color.Green)
            };

            _vertices = new VertexBuffer(Graphics, VertexPositionColor.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
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