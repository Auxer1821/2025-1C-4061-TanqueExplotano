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
            _efecto.Parameters["Texture"]?.SetValue(_montanaTexture);
            this._efecto.Parameters["ambientColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["diffuseColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["specularColor"]?.SetValue(Color.White.ToVector3());
            this._efecto.Parameters["KAmbient"]?.SetValue(0.5f);
            this._efecto.Parameters["KDiffuse"]?.SetValue(1.0f);
            this._efecto.Parameters["KSpecular"]?.SetValue(0.5f);
            this._efecto.Parameters["shininess"]?.SetValue(16.0f);
            this.ConfigPuntos(Graphics);
        }
        public override void Draw(Matrix mundo, GraphicsDevice graphics){
            _efecto.CurrentTechnique = _efecto.Techniques["TextureDrawing"];
            graphics.SetVertexBuffer(_vertices);
            graphics.Indices = _indices;

            _efecto.Parameters["World"].SetValue(mundo);
            _efecto.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(mundo)));

            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this._indices.IndexCount);
            }
        }

        public override void DibujarShadowMap(Matrix MVP, GraphicsDevice graphics)
        {

            _efecto.CurrentTechnique = _efecto.Techniques["DepthPass"];
            graphics.SetVertexBuffer(_vertices);
            graphics.Indices = _indices;

            _efecto.Parameters["WorldViewProjection"].SetValue(MVP);
            foreach (var pass in _efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this._indices.IndexCount);
            }
        }

        
        private  void ConfigPuntos(GraphicsDevice Graphics){

            VertexPositionNormalTexture[] puntos = new VertexPositionNormalTexture[16];

            //cara piso
            puntos[0] = new VertexPositionNormalTexture(new Vector3(-1f, 0f, -1f), Vector3.Forward, new Vector2(0, 0));
            puntos[1] =   new VertexPositionNormalTexture(new Vector3(1f, 0f, -1f),Vector3.Forward, new Vector2(1, 0));
            puntos[2] =   new VertexPositionNormalTexture(new Vector3(-1f, 0f, 1f),Vector3.Forward, new Vector2(0, 1));
            puntos[3] =   new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f),Vector3.Forward, new Vector2(1, 1));

            //cara izquierda
            puntos[4] = new VertexPositionNormalTexture(new Vector3(-1f, 0f, -1f), Vector3.Left, new Vector2(0, 0));
            puntos[5] = new VertexPositionNormalTexture(new Vector3(-1f, 0f, 1f), Vector3.Left, new Vector2(1, 0));
            puntos[6] = new VertexPositionNormalTexture(new Vector3(0f, 2f, 0f), Vector3.Left, new Vector2(0, 1));

            //cara trasera
            puntos[7] = new VertexPositionNormalTexture(new Vector3(-1f, 0f, -1f), Vector3.Forward, new Vector2(0, 0));
            puntos[8] = new VertexPositionNormalTexture(new Vector3(1f, 0f, -1f), Vector3.Forward, new Vector2(1, 0));
            puntos[9] = new VertexPositionNormalTexture(new Vector3(0f, 2f, 0f), Vector3.Forward, new Vector2(0.5f, 1));

            //cara derecha
            puntos[10] = new VertexPositionNormalTexture(new Vector3(1f, 0f, -1f), Vector3.Right, new Vector2(0, 0));
            puntos[11] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f), Vector3.Right, new Vector2(1, 0));
            puntos[12] = new VertexPositionNormalTexture(new Vector3(0f, 2f, 0f), Vector3.Right, new Vector2(0.5f, 1));

            //cara delantera
            puntos[13] = new VertexPositionNormalTexture(new Vector3(-1f, 0f, 1f), Vector3.Backward, new Vector2(0, 0));
            puntos[14] = new VertexPositionNormalTexture(new Vector3(1f, 0f, 1f), Vector3.Backward, new Vector2(1, 0));
            puntos[15] = new VertexPositionNormalTexture(new Vector3(0f, 2f, 0f), Vector3.Backward, new Vector2(0.5f, 1));

            ushort[] Indices = new ushort[18]
            {
                0,1,2, 1,2,3, //Cara Piso
                4,5,6, //Cara izq
                7,8,9, //Cara trasera
                10,11,12, //Cara der
                13,14,15, //Cara delantera
            };

            this._vertices = new VertexBuffer(Graphics, VertexPositionNormalTexture.VertexDeclaration, puntos.Length , BufferUsage.WriteOnly);
            this._vertices.SetData(puntos);
            this._indices = new IndexBuffer(Graphics, IndexElementSize.SixteenBits, Indices.Length , BufferUsage.WriteOnly);
            this._indices.SetData(Indices);
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