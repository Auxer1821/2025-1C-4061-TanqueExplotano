using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;
using TGC.MonoGame.TP.src.HUD;
using System.Linq;



namespace TGC.MonoGame.TP.src.Managers
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class ManagerInterfaz 
    {
        private Entidades.EJugador _jugador;

        //------------Htexto---------------//
        private HTexto _vida;
        private HTexto _progreso;
        private HTexto _tiempo;

        private Effect _textoEffect;

        //-------------HImagen--------------//
        private List<HImagen> _imagenesHud;

        //buffers para dibujar el HUD
        private ContentManager _Content;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private GraphicsDevice _graphicsDevice;
        /*
            IMPLEMENTACION EN ESENARIO
            IMPLEMETACION DE LISTA IMAGENES HUD
            IMPLEMENTACION DEL DRAW
            Y LO QUE HAY EN EL UPDATE
            CREAR EL EFECTO PARTICULAR DEL IMAGEN TEXTO
        */
        public ManagerInterfaz()
        {
            //_jugador = new Entidades.EJugador();
            _vida = new HTexto();
            _progreso = new HTexto();
            _tiempo = new HTexto();
            _imagenesHud = new List<HImagen>();
        }

        public void Inicializar(GraphicsDevice device,ContentManager Content, Entidades.EJugador jugador){
            _Content = Content;
            _jugador = jugador;
            _graphicsDevice = device;
            
            _textoEffect = Content.Load<Effect>(@"Effects/shaderTransparencia");//todo
            Texture2D texture = Content.Load<Texture2D>(@"Textures/ui/CaracteresNegros");
            _textoEffect.Parameters["Texture"].SetValue(texture);

            _vida.Initialize(new Vector2(0,0));
            _vida.setValor( ((int) _jugador.getVida()).ToString() );

            _vida.Initialize(new Vector2(0,0));
            _progreso.setValor("0/5");

            _vida.Initialize(new Vector2(0,0));
            _tiempo.setValor("00:00");

        }

        public void Dibujar(){

        }

        public void Update()
        {
            _vida.setValor(((int)_jugador.getVida()).ToString());
            _progreso.setValor( (int)_jugador.getKills() + "/5");//TODO
            _tiempo.setValor("00:00");//DIFERENCIA DE TIMES O ALGO
            
        }

        //------Funcion para crear donde se dibujan las texturas----//
        private void crearQuad()
        {
            // 1. Definir los vértices (4 vértices para un quad)
            var vertices = new VertexPositionNormalTexture[4];

            // Tamaño del quad (1 unidad de ancho y alto)
            float halfSize = 0.5f;

            // Coordenadas de los vértices (en sentido horario)
            vertices[0] = new VertexPositionNormalTexture(
                new Vector3(-halfSize, -halfSize, 0), // Posición (inferior izquierda)
                Vector3.UnitZ,                        // Normal (apuntando hacia la cámara)
                new Vector2(0, 1));                   // Coordenadas UV

            vertices[1] = new VertexPositionNormalTexture(
                new Vector3(-halfSize, halfSize, 0),  // Superior izquierda
                Vector3.UnitZ,
                new Vector2(0, 0));

            vertices[2] = new VertexPositionNormalTexture(
                new Vector3(halfSize, -halfSize, 0),  // Inferior derecha
                Vector3.UnitZ,
                new Vector2(1, 1));

            vertices[3] = new VertexPositionNormalTexture(
                new Vector3(halfSize, halfSize, 0),   // Superior derecha
                Vector3.UnitZ,
                new Vector2(1, 0));

            // 2. Crear VertexBuffer
            _vertexBuffer = new VertexBuffer(
                _graphicsDevice,
                VertexPositionNormalTexture.VertexDeclaration,
                vertices.Length,
                BufferUsage.WriteOnly);

            _vertexBuffer.SetData(vertices);

            // 3. Definir índices (2 triángulos = 6 índices)
            var indices = new short[6];

            // Primer triángulo (inferior izquierda, superior izquierda, inferior derecha)
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;

            // Segundo triángulo (superior izquierda, superior derecha, inferior derecha)
            indices[3] = 1;
            indices[4] = 3;
            indices[5] = 2;

            // 4. Crear IndexBuffer
            _indexBuffer = new IndexBuffer(
                _graphicsDevice,
                IndexElementSize.SixteenBits,
                indices.Length,
                BufferUsage.WriteOnly);

            _indexBuffer.SetData(indices);
        }

        

    }
}