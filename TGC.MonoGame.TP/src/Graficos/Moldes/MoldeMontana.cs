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
            this._efecto.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["KAmbient"]?.SetValue(0.5f);
            this._efecto.Parameters["KDiffuse"]?.SetValue(1.0f);
            this._efecto.Parameters["KSpecular"]?.SetValue(0.8f);
            this._efecto.Parameters["shininess"]?.SetValue(16.0f);
            this.ConfigPuntos(Graphics);
        }
        public override void Draw(Matrix mundo, GraphicsDevice graphics){
            _efecto.Parameters["World"].SetValue(mundo);
            _efecto.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(mundo)));

            graphics.SetVertexBuffer(_vertices);
            graphics.Indices = _indices;

            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }
        private  void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionNormalTexture[] puntos = new VertexPositionNormalTexture[]
            {
                new VertexPositionNormalTexture(new Vector3(-1f, 0f, -1f),Vector3.Up, new Vector2(0, 0)),
                new VertexPositionNormalTexture(new Vector3(1f, 0f, -1f),Vector3.Up, new Vector2(1, 0)),
                new VertexPositionNormalTexture(new Vector3(-1f, 0f, 1f),Vector3.Up, new Vector2(0, 1)),
                new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f),Vector3.Up, new Vector2(1, 1)),
                new VertexPositionNormalTexture(new Vector3(0f, 2f, 0f),Vector3.Normalize(new Vector3(0, 0.5f, -0.5f)), new Vector2(0.5f, 0.5f))
            };

            Vector3 normalTriangulo1 = CalculateNormal(
                puntos[0].Position,
                puntos[1].Position,
                puntos[4].Position);

            Vector3 normalTriangulo2 = CalculateNormal(
                puntos[1].Position,
                puntos[3].Position,
                puntos[4].Position);

            Vector3 normalTriangulo3 = CalculateNormal(
                puntos[3].Position,
                puntos[2].Position,
                puntos[4].Position);

            Vector3 normalTriangulo4 = CalculateNormal(
                puntos[2].Position,
                puntos[0].Position,
                puntos[4].Position);

            // Asigna normales a los vértices
            puntos[0].Normal = Vector3.Normalize(normalTriangulo1 + normalTriangulo4);
            puntos[1].Normal = Vector3.Normalize(normalTriangulo1 + normalTriangulo2);
            puntos[2].Normal = Vector3.Normalize(normalTriangulo3 + normalTriangulo4);
            puntos[3].Normal = Vector3.Normalize(normalTriangulo2 + normalTriangulo3);
            puntos[4].Normal = Vector3.Normalize(
                normalTriangulo1 + normalTriangulo2 + normalTriangulo3 + normalTriangulo4);

            _vertices = new VertexBuffer(Graphics, VertexPositionNormalTexture.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
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

        
        // Método para calcular la normal de un triángulo
        Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            return Vector3.Cross(ab, ac);
        }

    }
}