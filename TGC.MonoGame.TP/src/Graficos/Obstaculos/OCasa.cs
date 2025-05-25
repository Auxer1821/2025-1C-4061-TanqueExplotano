using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Casas
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    public class OCasa : Modelos.Modelo
    {
        
        // Variables   
        Texture2D paredTexture;
        Texture2D techoTexture;
        Texture2D chimeneaTexture;
        Texture2D marcoTexture;
        string[] meshes;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.DarkRed.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        protected override void ConfigurarModelo(ContentManager Content){
            this._modelo = Content.Load<Model>(@"Models/house/cartoon_house1");
            chimeneaTexture = Content.Load<Texture2D>(@"Models/house/paredPiedra");
            paredTexture = Content.Load<Texture2D>(@"Models/house/textura-roja");
            techoTexture = Content.Load<Texture2D>(@"Models/house/techo2");
            marcoTexture = Content.Load<Texture2D>(@"Models/house/tablasMadera");

            //cargar texturas en los parametros del shader
            _effect2.Parameters["TextureChimenea"].SetValue(chimeneaTexture);
            _effect2.Parameters["TexturePared"].SetValue(paredTexture);
            _effect2.Parameters["TextureTecho"].SetValue(techoTexture);
            _effect2.Parameters["TextureVentana"].SetValue(marcoTexture);

            //obtenemos los meshes del modelo
            int count = 0;
            int meshCount = _modelo.Meshes.Count;
            meshes = new string[meshCount];
            foreach (var mesh in _modelo.Meshes)
            {
                if (!string.IsNullOrEmpty(mesh.Name))
                {
                    meshes[count] = mesh.Name;
                    //Console.WriteLine($"Mesh {count}: {mesh.Name}");
                }
                else
                {
                    // Asignar nombre genérico si no tiene
                    mesh.Name = $"Mesh_{count}";
                    meshes[count] = mesh.Name;
                    //Console.WriteLine($"Mesh {count}: {mesh.Name}");
                }
                count++;
            }

        }
        protected override void AjustarModelo(){
            _matixBase = Matrix.CreateScale(0.03f)* Matrix.CreateTranslation(Vector3.UnitY * 3f);
        }

        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>(@"Effects/shaderCasa");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta

        //----------------------------------------------Dibujado--------------------------------------------------//
        public override void Dibujar(GraphicsDevice Graphics)
        {
            // Seteo de textura
            _effect2.Parameters["View"].SetValue(this._matrixView);
            _effect2.Parameters["Projection"].SetValue(this._matrixProyection);
            _effect2.Parameters["World"].SetValue(this._matrixMundo);

            foreach (var mesh in _modelo.Meshes)
            {
                if (mesh.Name.Contains("Roof"))
                {
                    _effect2.CurrentTechnique = _effect2.Techniques["Techo"];
                }
                else if (mesh.Name.Contains("Window"))
                {
                    _effect2.CurrentTechnique = _effect2.Techniques["Ventana"];
                }
                else if (mesh.Name.Contains("Cummny"))
                {
                    _effect2.CurrentTechnique = _effect2.Techniques["Chimenea"];
                }
                else if (mesh.Name.Contains("Wall"))
                {
                    _effect2.CurrentTechnique = _effect2.Techniques["Pared"];
                }
                _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                mesh.Draw();
            }
        }

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//
       

        //Configuración de efectos tomados desde la clase padre
        
    }
}