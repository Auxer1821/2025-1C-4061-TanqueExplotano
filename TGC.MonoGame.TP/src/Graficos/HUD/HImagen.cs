using System;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.HUD
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class HImagen:IHUD
    {
        private Vector2 _coordenadas;
        Texture2D _texture;
        Effect _efecto;


        //----------------------------------------------Variables--------------------------------------------------//

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public HImagen()
        {
        }
        // Inicializar recursos necesarios para el HUD
        public void Initialize(Vector2 coordenadas, ContentManager Content, string path )
        {
            _efecto = Content.Load<Effect>(@"Effects/shaderTransparencia");
            _texture = Content.Load<Texture2D>(path);
            _efecto.Parameters["Texture"].SetValue(_texture);
            this._coordenadas = coordenadas;
        }


        //----------------------------------------------Funciones-Principales--------------------------------------------------//


        //----------------------------------------------Dibujado--------------------------------------------------//
        public void Dibujado(GraphicsDevice Graphics, Effect efecto, IndexBuffer indices, VertexBuffer vertices)
        {

            efecto.Parameters["Coordenadas"].SetValue(_coordenadas);

            Graphics.SetVertexBuffer(vertices);
            Graphics.Indices = indices;
            foreach (var pass in efecto.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indices.IndexCount);

            }

        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
        


        
    }
}