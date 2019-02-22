using FPS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM
{
    public partial class FRAllSensorsView : FormSp
    {
        Tube tube;
        FRMain fRMain;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="_tube">труба для отображения</param>
        public FRAllSensorsView(FRMain _parent,Tube _tube)
        {
            MdiParent = _parent;
            fRMain = _parent;
            tube = _tube;
            InitializeComponent();
            ucTubeView.Init(tube);
        }

        private void FRAllSensorsView_Resize(object sender, EventArgs e)
        {
            ucTubeView.Invalidate();
        }
    }
}
