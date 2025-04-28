using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Esenario
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Terreno : Objetos.Objetos
    {
        // Variables
        //  Todas en Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            this._matrixMundo = Matrix.Identity * Matrix.CreateScale(1000) * Matrix.CreateTranslation(Vector3.UnitY * -1);
            this._matrixView = Matrix.CreateLookAt(new Vector3(0, 50, 150), Vector3.Zero, Vector3.Up);
            this._matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 2500);

            base.Initialize(Graphics);

        }

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.SandyBrown.ToVector3();
            base.Initialize(Graphics, Matrix.CreateScale(500) * Mundo, View, Projection, Content);
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//

        protected override void ConfigPuntos (GraphicsDevice Graphics){
            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-1f, 0f, 1f), Color.Red),
                new VertexPositionColor(new Vector3(-1f, 0f, -1f), Color.Red),
                new VertexPositionColor(new Vector3(1f, 0f, 1f), Color.Red),
                new VertexPositionColor(new Vector3(1f, 0f, -1f), Color.Red)
            };

            _vertices = new VertexBuffer(Graphics, VertexPositionColor.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            ushort[] Indices = new ushort[]
            {
                0,1,2,
                1,2,3
            };

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, 6 , BufferUsage.None);
            _indices.SetData(Indices);
        }
        
        //Configuración de efectos tomados desde la clase padre

        


        
    }
}