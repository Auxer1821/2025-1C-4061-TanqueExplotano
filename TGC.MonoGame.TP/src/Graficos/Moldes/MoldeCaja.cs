using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using System.Linq;
using TGC.MonoGame.TP.src.Modelos;



namespace TGC.MonoGame.TP.src.Moldes
{
    /// <summary>
    ///     Esta es la clase donde se guarda el  modelo; efecto y otras cosas que comparten las entidades iguale para dibujar 
    /// </summary>
    public class MoldeCaja : IMolde
    {
        private IndexBuffer _indices;
        private VertexBuffer _vertices;
        Texture2D cajaTexture;
        public MoldeCaja(ContentManager Content, GraphicsDevice Graphics){
            this._efecto = Content.Load<Effect>(@"Effects/shaderCaja");
            this.cajaTexture = Content.Load<Texture2D>(@"Models/house/tablasMadera");
            this._efecto.Parameters["Texture"].SetValue(cajaTexture);

            this.ConfigPuntos(Graphics);
        }
        public override void Draw(Matrix Mundo, GraphicsDevice Graphics){

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this._indices.IndexCount);
            }
        }


        
        private  void ConfigPuntos(GraphicsDevice Graphics){

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


    }
}