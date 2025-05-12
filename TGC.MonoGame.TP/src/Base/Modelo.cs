using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TGC.MonoGame.TP.src.Modelos
{
    /// <summary>
    ///     Clase Abstracta para todos los objetos
    /// </summary>
    public abstract class Modelo
    {
        
        // Variables
        protected Model _modelo;
        protected BasicEffect _effect;
        protected Effect _effect2;
        protected Matrix _matixBase {get; set;}
        protected Matrix _matrixMundo {get; set;}
        protected Matrix _matrixView {get; set;}
        protected Matrix _matrixProyection {get; set;}
        protected Vector3 _Color {get; set;}

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//

        public Modelo (){
            //TODO
        }
        
        public virtual void Initialize (GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            //Configuración de matrices
            //this._matrixMundo = Mundo;
            this._matrixView = View;
            this._matrixProyection = Projection;

            //Seteo de efectos
            _effect2 = this.ConfigEfectos2(Graphics, Content);

            //Configuración Dibujar
            this.ConfigurarModelo(Content);
            this.AjustarModelo();

            //efecto al modelo
            foreach (var mesh in _modelo.Meshes)
            {
                // Un mesh puede tener mas de 1 mesh part (cada 1 puede tener su propio efecto).
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = _effect2;
                }
            }
            
            ActualizarMatrizMundo(Mundo);

        }



        //----------------------------------------------Dibujado--------------------------------------------------//

        public virtual void Dibujar(GraphicsDevice Graphics){

            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);
            _effect2.Parameters["DiffuseColor"].SetValue(this._Color);

            foreach (var mesh in _modelo.Meshes)
            {
                _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                mesh.Draw();
            }
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//

        protected abstract void ConfigurarModelo(ContentManager content);
        protected virtual void AjustarModelo(){
            this._matixBase= Matrix.Identity;
        }
        
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
            this._matrixMundo= this._matixBase * mundo;
        }


        
    }
}