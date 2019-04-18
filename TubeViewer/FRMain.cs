using CM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormsExtras;

namespace TubeViewer
{
    public partial class FRMain : FormSp
    {
        //Tube tube;
        public FRMain()
        {
            InitializeComponent();
        }

        private void LoadDump_Click(object sender, EventArgs e)
        {

        }

        private void Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "bin",
                AddExtension = true,
                Filter = "Файлы bin (*.bin)|*.bin|Все файлы (*.*)|*.*",
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Tube tube = new Tube(new TypeSize(),0);
                if (Tube.load(ref tube, ofd.FileName))
                {
                    ViewAllSensors_Click(tube, null);
                }
                else
                {
                    MessageBox.Show(string.Format("Не удалось загрузить трубу из файла {0}", ofd.FileName), "Ошибка");
                }
            }
        }

        private void ViewAllSensors_Click(object sender, EventArgs e)
        {
            FRAllSensorsViewTV view = new FRAllSensorsViewTV(this,sender as Tube);
            view.Show();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
