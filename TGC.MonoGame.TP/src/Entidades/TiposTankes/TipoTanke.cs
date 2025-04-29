using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.src.TipoTanke
{
    /// <summary>
    ///     Esta es la clase del esenario donde se controla 
    /// </summary>
    public abstract class TipoTanke 
    {
        public abstract string file();
        public abstract Matrix AjusteMatrix();

    }
}