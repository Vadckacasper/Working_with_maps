using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using RIT.Data;
using RIT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RIT
{
    public partial class gMapForm : Form
    {
        private  GMapMarker _selectedMarker;
        private readonly List<Marker> _Markers;
        private readonly MyDbContext _db = new MyDbContext();
        public gMapForm()
        {
            InitializeComponent();
            _Markers = _db.DbMarker.GetAll().ToList();
        }

        private void _gMapForm_Load(object sender, EventArgs e)
        {
            gMapControl1.MouseUp += _gMapControl_MouseUp;
            gMapControl1.MouseDown += _gMapControl_MouseDown;
        }

        private void _gMapControl1_Load(object sender, EventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            gMapControl1.MinZoom = 2; 
            gMapControl1.MaxZoom = 14;
            gMapControl1.Zoom = 3;
            gMapControl1.Position = new GMap.NET.PointLatLng(66.4169575018027, 94.25025752215694);
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.ShowCenter = false;
            gMapControl1.ShowTileGridLines = false;
            gMapControl1.Overlays.Add(GetOverlayMarkers("GroupsMarkers"));
            gMapControl1.Overlays.Add(GetOverlayMarkers(" GroupsMarkers", GMarkerGoogleType.blue));
            gMapControl1.Overlays[0].IsVisibile = false;
            gMapControl1.Update();
        }
        private GMarkerGoogle GetMarker(Marker marker, GMarkerGoogleType gMarkerGoogleType = GMarkerGoogleType.red)
        {
            GMarkerGoogle mapMarker = new GMarkerGoogle(new GMap.NET.PointLatLng(marker.Latitude, marker.Longitude), gMarkerGoogleType);
            mapMarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(mapMarker);
            mapMarker.ToolTipText = marker.Id.ToString();
            mapMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            return mapMarker;
        }

        private GMapOverlay GetOverlayMarkers(string name, GMarkerGoogleType gMarkerGoogleType = GMarkerGoogleType.red)
        {
            GMapOverlay gMapMarkers = new GMapOverlay(name);
            foreach(Marker marker in _Markers)
            {
                gMapMarkers.Markers.Add(GetMarker(marker, gMarkerGoogleType));
            }
            return gMapMarkers;
        }

        private void _gMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            _selectedMarker = gMapControl1.Overlays
                .SelectMany(o => o.Markers)
                .FirstOrDefault(m => m.IsMouseOver == true);
        }

        private void _gMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_selectedMarker is null)
                return;
            

            var latlng = gMapControl1.FromLocalToLatLng(e.X, e.Y);
            _selectedMarker.Position = latlng;
            _db.DbMarker.Update(new Marker() { Id = Convert.ToInt32(_selectedMarker.ToolTipText), Latitude = _selectedMarker.Position.Lat, Longitude = _selectedMarker.Position.Lng });
            _selectedMarker = null;
        }
    }
}
