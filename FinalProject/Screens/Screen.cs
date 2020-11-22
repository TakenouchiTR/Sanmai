using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject.Screens
{
    public abstract class Screen
    {
        public abstract void Setup();

        public abstract bool Play();

        public abstract void Hide();
    }
}
