using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP.src.Escenarios
{
    /// <summary>
    ///     Esta es la clase que controla los diferentes esenarios como en esenario de juego y el menu
    /// </summary>
    public interface IEscenario
    {
        void Update(GameTime gameTime);
        void Dibujar(GraphicsDevice graphicsDevice);
    }
}
