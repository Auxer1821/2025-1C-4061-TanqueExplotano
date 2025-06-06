using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.HUD;


namespace TGC.MonoGame.TP.src.Escenarios
{
    /// <summary>
    ///     Esta es la clase que controla los diferentes esenarios como en esenario de juego y el menu
    /// </summary>
    public class EscenarioMenu : IEscenario
    {

        //----------------Variables---------------------//
        private GraphicsDevice _graphicsDevice;
        private HUD.HImagen _botonJugar;
        private HUD.HImagen _TituloJuego;
        private HUD.HImagen _botonOpciones;
        private HUD.HImagen _botonSalir;
        private HUD.HImagen _botonTanqueSig;
        private HUD.HImagen _botonTanqueAnt;


        //---------------Metodos--------------------------//

        public EscenarioMenu()
        {
            
        }

        public void Initialize(GraphicsDevice device, ContentManager Content )
        {
            _graphicsDevice = device;

            this._TituloJuego = new HImagen();
            this._TituloJuego.Initialize(new Vector2(0.0f, 0.6f), Content, "Textures/ui/cooltext483425277740560");
            this._TituloJuego.setQuad(0.4f,device);
            
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Dibujar(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);
            //_textoEffect.Parameters["View"].SetValue(Matrix.Identity);
            //_textoEffect.Parameters["Projection"].SetValue(Matrix.Identity);
            _TituloJuego.Dibujado(_graphicsDevice);/*
            _botonJugar.Dibujado(_graphicsDevice);
            _botonOpciones.Dibujado(_graphicsDevice);
            _botonSalir.Dibujado(_graphicsDevice);
            _botonTanqueSig.Dibujado(_graphicsDevice);
            _botonTanqueAnt.Dibujado(_graphicsDevice);*/
        }
        
       
        
    }
}




