using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace BotGameDrower
{
    public partial class fogOfWar: Component
    {    
        public fogOfWar()
        {
            InitializeComponent();
        }

        public fogOfWar(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
