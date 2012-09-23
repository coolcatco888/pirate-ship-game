using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PirateShipGame.Engine.Screens;

namespace PirateShipGame.Components.BaseComponents
{
    public abstract class Component
    {
        //Public Fields shared by all components
        #region Fields
        
        /// <summary>
        /// should be set once the component is usable and not altered after
        /// </summary>
        public bool Initialized
        {
            get { return initialized; }
            set { initialized = value; }
        }
        protected bool initialized;
     
        /// <summary>
        /// If true the object is updated during update
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        protected bool enabled;
        
        /// <summary>
        /// Parent screen holding component, affects update and draw order
        /// </summary>
        public Screen Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        Screen parent;

        #endregion

        //Note there is no default constructor
        #region Constructor

        public Component(Screen parent)
        {
            this.parent = parent;
        }

        #endregion

        // Section has the 3 main methods that should be overridden
        #region Methods

        public virtual void Initialize()
        {
            this.enabled = true;
            initialized = true;
            parent.Components.Add(this);
        }

        //Update component game logic
        public virtual void Update(GameTime gameTime) { }

        public virtual void Dispose()
        {
            parent.Components.Remove(this);
        }

        #endregion
    }
}
