using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TGC.MonoGame.TP.src.Managers;


namespace TGC.MonoGame.TP.src.Escenarios
{
    public enum TipoEsenario
    {
        Gameplay,
        Menu,
        Victoria,
        Derrota
    }
    /// <summary>
    ///     Esta es la clase que controla los diferentes esenarios como en esenario de juego y el menu
    /// </summary>
    public class DirectorEscenarios
    {
        ///-----------Variables-------------//
        private Escenario _escenarioGameplay;
        private EscenarioMenu _escenarioMenu;

        private IEscenario _esenarioActivo;
        private GraphicsDevice _graphicsDevice; 
        private Matrix _world;
        private ContentManager _content;
        private ManagerSonido _managerSonido;
        private string tipoMusica = "menu";

        public DirectorEscenarios()
        {

        }

        public void Initialize(GraphicsDevice graphicsDevice, Matrix world, ContentManager content){
            _graphicsDevice = graphicsDevice; 
            _world = world;
            _content = content;
            _escenarioGameplay = new Escenario();
            _escenarioGameplay.Initialize(_graphicsDevice,_world,_content);
            
            _escenarioMenu = new EscenarioMenu();
            _escenarioMenu.Initialize(graphicsDevice, content);
            _esenarioActivo = _escenarioMenu; //TODO: cambiar para que sea el menu

            _managerSonido = new ManagerSonido(content);
            _managerSonido.InstanciarMusica();
        }

        public void Dibujar(){

            //logica de dibujado del escenario
            _esenarioActivo.Dibujar(this._graphicsDevice);

        }

        public void Update(GameTime gameTime)
        {
            _esenarioActivo.Update(gameTime);
            this._managerSonido.reproducirMusica(tipoMusica);

        }
        public void CambiarEsenarioActivo(TipoEsenario esenario)
        {
            switch (esenario)
            {
                case TipoEsenario.Gameplay:
                    _esenarioActivo = this._escenarioGameplay;
                    this.tipoMusica = "accion";
                    break;
                default:
                    //--Cambiar a menu--//
                    _esenarioActivo = this._escenarioGameplay;
                    this.tipoMusica = "menu";
                    break;

            }
        }

    }
}
