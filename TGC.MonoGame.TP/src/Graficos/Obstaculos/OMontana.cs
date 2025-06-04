using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Montanas
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class OMontana : Objetos.Objeto
    {

        // Variables
        //Texture2D montanaTexture;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public OMontana(){}
        public override void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices
            //this._Color = Color.Yellow.ToVector3();
            this._matrixMundo = Matrix.Identity;

            base.Initialize(Graphics);

        }

        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, ContentManager Content)
        {
            //this._Color = Color.DarkGray.ToVector3();
            //montanaTexture = Content.Load<Texture2D>(@"Models/heightmap/montana");
            base.Initialize(Graphics, Matrix.CreateScale(new Vector3(100,200,100)) * Mundo, Content);
            //setear solo una vez la textura
            //_effect2.Parameters["Texture"].SetValue(montanaTexture);
        }

        protected override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/shaderMontana");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//

        public override void Dibujar(GraphicsDevice Graphics)
        {


            //actualizar solo valores necesarios
            _effect2.Parameters["World"].SetValue(this._matrixMundo);

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }

        }
        

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        protected override void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionTexture[] puntos = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-1f, 0f, -1f), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(1f, 0f, -1f), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(-1f, 0f, 1f), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(1f, 0f, 1f), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(0f, 2f, 0f), new Vector2(0.5f, 0.5f))
            };

            _vertices = new VertexBuffer(Graphics, VertexPositionTexture.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
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