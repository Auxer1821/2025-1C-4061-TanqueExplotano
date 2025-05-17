using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.src.Objetos;


namespace TGC.MonoGame.TP.src.Arboles
{
    /// <summary>
    ///     Esta es la clase del escenario donde se controla 
    /// </summary>
    
    //cambio de objeto a modelo, para poder usar el modelo de arbol
    public class OArbol : Modelos.Modelo
    {
        
        // Variables
        Texture2D troncoTexture;
        Texture2D hojasTexture;
        string[] meshes;
        //  En Clase Abstracta

        //----------------------------------------------Constructores-e-inicializador--------------------------------------------------//
        public OArbol(){}
        public override void Initialize(GraphicsDevice Graphics, Matrix Mundo, Matrix View, Matrix Projection, ContentManager Content)
        {
            this._Color = Color.Green.ToVector3();
            base.Initialize(Graphics, Mundo, View, Projection, Content);
        }

        protected override void ConfigurarModelo(ContentManager Content)
        {
            this._modelo = Content.Load<Model>("Models/tree/tree2");
            troncoTexture = Content.Load<Texture2D>("Models/tree/tronco2");
            hojasTexture = Content.Load<Texture2D>("Models/tree/light-green-texture");

            //obtenemos los meshes del modelo
            int count = 0;
            int meshCount = _modelo.Meshes.Count;
            meshes = new string[meshCount];
            foreach (var mesh in _modelo.Meshes)
            {
                if (!string.IsNullOrEmpty(mesh.Name))
                {
                    meshes[count] = mesh.Name;
                    Console.WriteLine($"Mesh {count}: {mesh.Name}");
                }
                else
                {
                    // Asignar nombre genérico si no tiene
                    mesh.Name = $"Mesh_{count}";
                    meshes[count] = mesh.Name;
                    Console.WriteLine($"Mesh {count}: {mesh.Name}");
                }
                count++;
            }
        }

        protected override void AjustarModelo()
        {
            _matixBase = Matrix.CreateScale(0.008f);
        }

        public override Effect ConfigEfectos2(GraphicsDevice Graphics, ContentManager Content)
        {
            return Content.Load<Effect>("Effects/shaderTree");
        }

        //El constructor que tiene de parametos las matrices, usamos el de la clase abstracta


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
                if (mesh.Name == meshes[4])
                {
                    _effect2.Parameters["Texture"].SetValue(troncoTexture);
                }
                else
                {
                    _effect2.Parameters["Texture"].SetValue(hojasTexture);
                }
                _effect2.Parameters["World"].SetValue(mesh.ParentBone.Transform * _matrixMundo);
                mesh.Draw();
            }
        }
 

        //----------------------------------------------Funciones-Auxiliares--------------------------------------------------//


        //Configuración de efectos tomados desde la clase padre

    }
}