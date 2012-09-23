using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PirateShipGame.Engine.Screens;
using PirateShipGame.Components.BaseComponents;

namespace PirateShipGame.Engine.Input
{
    public abstract class InputDevice<T> : Component
    {
        protected InputDevice(Screen parent)
            : base(parent)
        {
        }

        public abstract T State { get; }
    }
}
