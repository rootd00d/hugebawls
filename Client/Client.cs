using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace HugeBawls
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Client : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private ContentManager content;

        /// <summary>Matrix used to transform 3D to screen coordinates</summary>
        private Matrix projection;
        /// <summary>Matrix that defines the viewer's location in the scene</summary>
        private Matrix view;
        /// <summary>Matrix for controlling the location of rendered polygons</summary>
        private Matrix world;
        /// <summary>Vertex type that stores a position and color in each vertex</summary>
        private VertexDeclaration positionColorVertexDeclaration;
        /// <summary>Default Vertex- & PixelShader for the graphics card</summary>
        private Effect defaultEffect;

        private Model bawl;

        public Client()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Create a new default effect containing universal
            // vertex and pixel shaders that we can use to draw our triangle
            this.defaultEffect = new BasicEffect(graphics.GraphicsDevice, null);

            // Declare the vertex format we will use for the rotating triangle's vertices
            this.positionColorVertexDeclaration = new VertexDeclaration(
              this.graphics.GraphicsDevice, VertexPositionColor.VertexElements
            );

            // Create a new perspective projection matrix
            //this.projection = Matrix.CreatePerspectiveFieldOfView(
            //  MathHelper.PiOver4, // field of view
            //  (float)Window.ClientBounds.Width / (float)Window.ClientBounds.Height, // aspect ratio
            //  0.01f, 1000.0f // near and far clipping plane
            //);

            this.projection = Matrix.CreateOrthographic(10000f, 10000f, 20f, 100f);

            this.world = Matrix.Identity;

            this.view = Matrix.CreateLookAt(
              new Vector3(50f, 0.0f, 10.0f), Vector3.Zero, new Vector3(0.0f, 1.0f, 0.0f)
            );

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            bawl = Content.Load<Model>("Bawl");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            this.graphics.GraphicsDevice.VertexDeclaration = this.positionColorVertexDeclaration;

            RenderState renderState = graphics.GraphicsDevice.RenderState;

            renderState.AlphaBlendEnable = false;
            renderState.AlphaTestEnable = false;
            renderState.DepthBufferEnable = true;
            renderState.PointSize = 2;
            renderState.CullMode = CullMode.None;

            this.defaultEffect.Begin();

            foreach (EffectPass pass in this.defaultEffect.CurrentTechnique.Passes)
            {
                pass.Begin();

                foreach (ModelMesh mesh in bawl.Meshes)
                {
                    mesh.Draw();
                }
                
                pass.End();
            }
            this.defaultEffect.End();

            base.Draw(gameTime);
        }
    }
}
