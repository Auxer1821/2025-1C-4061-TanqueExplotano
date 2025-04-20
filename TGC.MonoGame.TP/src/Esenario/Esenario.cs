using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.Esenario
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Esenario
    {
        
        // escenario plano (Dibujado)
        private VertexBuffer _vertices;
        private IndexBuffer _indices;
        private BasicEffect _effect;


        // objeto escenario (Configuracion)
        private Matrix _matrixMundo {get;}
        private Matrix _matrixView{get;}
        private Matrix _matrixProyection{get;}

        //var a = GraphicsDevice

        //--------------------------------------------------------Constructores--------------------------------------------------------//
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public Esenario(GraphicsDevice Graphics)
        {
            //Configuración de matrices
            _matrixMundo = Matrix.Identity;
            _matrixView = Matrix.CreateLookAt(new Vector3(0, 0, 50), Vector3.Forward, Vector3.Up);
            _matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 1000);



            //Configuración Dibujar

            // Setup our basic effect
            _effect = new BasicEffect(Graphics)
            {
                World = _matrixMundo,
                View = _matrixView,
                Projection = _matrixProyection,
                VertexColorEnabled = true
            };

            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-15f, 0f, 15f), Color.Green),
                new VertexPositionColor(new Vector3(-15f, 0f, -15f), Color.Green),
                new VertexPositionColor(new Vector3(15f, 0f, 15f), Color.Green),
                new VertexPositionColor(new Vector3(15f, 0f, -15f), Color.Green)
            };

            _vertices = new VertexBuffer(Graphics, VertexPositionColor.VertexDeclaration, 4 , BufferUsage.WriteOnly);
            _vertices.SetData(puntos);

            ushort[] Indices = new ushort[]
            {
                0,1,2,
                1,2,3
            };

            _indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, 6 , BufferUsage.None);
            _indices.SetData(Indices);

        }

        public void Dibujar(GraphicsDevice Graphics)
        {
            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            _effect.Parameters["World"].SetValue(_matrixMundo);
            _effect.Parameters["View"].SetValue(_matrixView);
//            _effect.Parameters["Projection"].SetValue(_matrixProyection);

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }

           Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
        }
        

        //metodos
        


        
    }
}