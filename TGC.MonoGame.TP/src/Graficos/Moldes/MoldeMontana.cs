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
    public class MoldeMontana : IMolde
    {
        Texture2D _montanaTexture;
        private VertexBuffer _vertices;
        private IndexBuffer _indices;
        public MoldeMontana(ContentManager Content, GraphicsDevice Graphics)
        {
            this._efecto = Content.Load<Effect>(@"Effects/shaderMontana");
            _montanaTexture = Content.Load<Texture2D>(@"Models/heightmap/montana");
            _efecto.Parameters["Texture"].SetValue(_montanaTexture);
            this.ConfigPuntos(Graphics);
        }
        public override void Draw(Matrix mundo, GraphicsDevice graphics){
            _efecto.Parameters["World"].SetValue(mundo);

            graphics.SetVertexBuffer(_vertices);
            graphics.Indices = _indices;

            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }
        private  void ConfigPuntos(GraphicsDevice Graphics){

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



    }
}