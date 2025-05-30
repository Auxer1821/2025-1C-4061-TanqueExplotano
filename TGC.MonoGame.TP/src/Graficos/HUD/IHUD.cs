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
    ///     Esta es la Interfaz para los HUD 
    /// </summary>
    public interface IHUD
    {
        void Dibujado(GraphicsDevice Graphics, Effect efecto, IndexBuffer indices, VertexBuffer vertices);
    }
}