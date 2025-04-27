using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.Esenario2
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public class Esenario2
    {
        
        // escenario plano (Dibujado)
        private VertexBuffer _vertices;
        private IndexBuffer _indices;
        private BasicEffect _effect;


        // objeto escenario (Configuracion)
        private Matrix _matrixMundo {get; set;}
        private Matrix _matrixView {get; set;}
        private Matrix _matrixProyection {get; set;}

        //var a = GraphicsDevice

        //--------------------------------------------------------Constructores--------------------------------------------------------//
        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public void Esenario2_Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            this._matrixMundo = Matrix.Identity;
            this._matrixView = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            this._matrixProyection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Graphics.Viewport.AspectRatio, 1, 250);

            //Seteo de efectos
            this._effect = new BasicEffect(Graphics)
            {
                World = _matrixMundo,
                View = _matrixView,
                Projection = _matrixProyection,
                VertexColorEnabled = true
            };
                

            //Configuración Dibujar
            //Version Triangulo XZ
            //TODO: Emprolijar esto. Tanto los vertices como en TGCGame.cs

            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-15f, -1f, 15f), Color.Green),
                new VertexPositionColor(new Vector3(-15f, -1f, -15f), Color.Green),
                new VertexPositionColor(new Vector3(15f, -1f, 15f), Color.Green),
                new VertexPositionColor(new Vector3(15f, -1f, -15f), Color.Green)
            };

            //Version Triangulo XY
/*
            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(-15f, 15f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(-15f, -15f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(15f, 15f, 0f), Color.Green),
                new VertexPositionColor(new Vector3(15f, -15f, 0f), Color.Green)
            };
*/

/*
            //Version Triangulo YZ
            VertexPositionColor[] puntos = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0f, -15f, 15f), Color.Green),
                new VertexPositionColor(new Vector3(0f, -15f, -15f), Color.Green),
                new VertexPositionColor(new Vector3(0f, 15f, 15f), Color.Green),
            };
*/
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

        public void Escenario2_Dibujar(GraphicsDevice Graphics)
        {
            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,2);
            }
            
        }
        

        //metodos
        


        
    }
}