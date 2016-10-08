using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace SharpGeo.Demo
{
    public partial class DemoForm : Form
    {
        private DotSpatial.Controls.Map _map;

        public DemoForm()
        {
            InitializeComponent();

            _map = new DotSpatial.Controls.Map();
        }

        protected override void OnLoad(EventArgs e)
        {
            DotSpatial.Data.IFeatureSet shape = DotSpatial.Data.Shapefile.Open(@"d:\temp\osm_hungary_shp\gis.osm_buildings_a_free_1.shp");
            _map.MapFrame.Add(shape);
            _map.Dock = DockStyle.Fill;
            this.Controls.Add(_map);

            var bitmap = new Bitmap(200, 200, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(Color.GreenYellow), 0, 0, bitmap.Width, bitmap.Height);
            }
            //for (var x = 0; x < bitmap.Width; x++)
            //{
            //    for (var y = 0; y < bitmap.Height; y++)
            //        bitmap.SetPixel(x, y, Color.Yellow);
            //}

            var renderer = new Renderer(bitmap);
            renderer.DrawLine(0, 0, bitmap.Width, bitmap.Height, Color.Blue);

            pictureBox1.Image = bitmap;

            base.OnLoad(e);
        }
    }
}
