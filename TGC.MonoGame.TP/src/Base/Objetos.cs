using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.Objetos
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class Objeto
    {
        
        // Variables
        protected VertexBuffer _vertices;
        protected IndexBuffer _indices;
        protected BasicEffect _effect;
        protected Effect _effect2;
        protected Matrix _matrixMundo {get; set;}
        protected Matrix _matrixView {get; set;}
        protected Matrix _matrixProyection {get; set;}

        protected Vector3 ubicacion {get; set;}
        protected Vector3 _Color {get; set;}

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public virtual void Initialize (GraphicsDevice Graphics)
        {
            //Configuración de matrices base
            //REALIZAR EN CADA HIJO

            //Seteo de efectos
            ConfigEfectos(Graphics);
            
            //Configuración Dibujar
            ConfigPuntos(Graphics);

        }

        public virtual void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection)
        {
            //Configuración de matrices
            this._matrixMundo = Mundo;
            this._matrixView = View;
            this._matrixProyection = Projection;

            //Seteo de efectos
            this.ConfigEfectos(Graphics);
                

            //Configuración Dibujar
            this.ConfigPuntos(Graphics);

        }

        public virtual void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            //Configuración de matrices
            this._matrixMundo = Mundo;
            this._matrixView = View;
            this._matrixProyection = Projection;

            //Seteo de efectos
            _effect2 = this.ConfigEfectos2(Graphics, Content);
                

            //Configuración Dibujar
            this.ConfigPuntos(Graphics);

        }



        //----------------------------------------------Dibujado--------------------------------------------------//

        public virtual void Dibujar(GraphicsDevice Graphics){
            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["DiffuseColor"].SetValue(this._Color);

            Graphics.SetVertexBuffer(_vertices);
            Graphics.Indices = _indices;

            foreach (var pass in _effect2.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this._indices.IndexCount);
            }
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//

        protected abstract void ConfigPuntos(GraphicsDevice Graphics);
        
        protected virtual void ConfigEfectos(GraphicsDevice Graphics){
            this._effect = new BasicEffect(Graphics)
            {
                World = _matrixMundo,
                View = _matrixView,
                Projection = _matrixProyection,
                VertexColorEnabled = true
            };
        }

        protected virtual Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content){
            return Content.Load<Effect>("Effects/BasicShader");
        }

        public void ActualizarVistaProyeccion(Matrix Vista, Matrix Proyeccion){
            this._matrixView = Vista;
            this._matrixProyection = Proyeccion;
        }
        public virtual void ActualizarMatrizMundo(Matrix mundo){
            this._matrixMundo= mundo;
        }
        
    }
}