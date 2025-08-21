using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Negocio;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmAsignaciones : Form
    {
        private AsignacionBL asignacionBL;
        private ChoferBL choferBL;
        private AutobusBL autobusBL;
        private RutaBL rutaBL;

        public frmAsignaciones()
        {
            InitializeComponent();
            asignacionBL = new AsignacionBL();
            choferBL = new ChoferBL();
            autobusBL = new AutobusBL();
            rutaBL = new RutaBL();
            ConfigurarInterfaz();
            CargarCombos();
            CargarAsignacionesActivas();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Asignación de Rutas";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(236, 240, 241);

            Panel panel = new Panel();
            panel.BackColor = Color.White;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "🔗 Asignación de Rutas";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(0, 0);
            panel.Controls.Add(lblTitulo);

            // Panel de información
            Panel infoPanel = new Panel();
            infoPanel.BackColor = Color.FromArgb(249, 249, 249);
            infoPanel.BorderStyle = BorderStyle.FixedSingle;
            infoPanel.Size = new Size(panel.Width - 60, 60);
            infoPanel.Location = new Point(0, 40);
            infoPanel.Padding = new Padding(10);
            panel.Controls.Add(infoPanel);

            Label lblInfo = new Label();
            lblInfo.Text = "💡 Nota: Solo se muestran choferes, autobuses y rutas disponibles. Un chofer o autobús no puede tener múltiples asignaciones activas simultáneamente.";
            lblInfo.Font = new Font("Segoe UI", 9);
            lblInfo.ForeColor = Color.FromArgb(44, 62, 80);
            lblInfo.AutoSize = false;
            lblInfo.Size = new Size(infoPanel.Width - 20, 40);
            infoPanel.Controls.Add(lblInfo);

            // Combos de selección
            string[] labels = { "Chofer:", "Autobús:", "Ruta:" };
            int topPosition = 120;

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Font = new Font("Segoe UI", 10);
                lbl.ForeColor = Color.FromArgb(44, 62, 80);
                lbl.AutoSize = true;
                lbl.Location = new Point(20, topPosition);
                panel.Controls.Add(lbl);

                ComboBox combo = new ComboBox();
                combo.Font = new Font("Segoe UI", 10);
                combo.DropDownStyle = ComboBoxStyle.DropDownList;
                combo.Size = new Size(400, 30);
                combo.Location = new Point(20, topPosition + 25);

                switch (i)
                {
                    case 0:
                        combo.Name = "cmbChoferes";
                        combo.DisplayMember = "Display";
                        break;
                    case 1:
                        combo.Name = "cmbAutobuses";
                        combo.DisplayMember = "Display";
                        break;
                    case 2:
                        combo.Name = "cmbRutas";
                        combo.DisplayMember = "Display";
                        break;
                }

                panel.Controls.Add(combo);
                topPosition += 70;
            }

            // Botones
            int buttonTop = topPosition + 20;
            Button btnAsignar = CrearBoton("Asignar", Color.FromArgb(44, 62, 80), 20, buttonTop);
            btnAsignar.Click += BtnAsignar_Click;
            panel.Controls.Add(btnAsignar);

            Button btnLimpiar = CrearBoton("Limpiar", Color.FromArgb(149, 165, 166), 120, buttonTop);
            btnLimpiar.Click += BtnLimpiar_Click;
            panel.Controls.Add(btnLimpiar);

            // DataGridView
            buttonTop += 60;
            Label lblGrid = new Label();
            lblGrid.Text = "Asignaciones Activas";
            lblGrid.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblGrid.ForeColor = Color.FromArgb(44, 62, 80);
            lblGrid.AutoSize = true;
            lblGrid.Location = new Point(20, buttonTop);
            panel.Controls.Add(lblGrid);

            buttonTop += 30;
            DataGridView dgv = new DataGridView();
            dgv.Name = "dgvAsignaciones";
            dgv.Size = new Size(panel.Width - 60, 250);
            dgv.Location = new Point(20, buttonTop);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += DgvAsignaciones_CellClick;

            // Columnas
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Chofer", "Chofer");
            dgv.Columns.Add("Autobus", "Autobús");
            dgv.Columns.Add("Ruta", "Ruta");
            dgv.Columns.Add("Fecha", "Fecha Asignación");
            dgv.Columns.Add("Acciones", "Acciones");

            dgv.Columns["Id"].Visible = false;

            panel.Controls.Add(dgv);
        }

        private Button CrearBoton(string texto, Color color, int x, int y)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new Size(90, 35);
            btn.Location = new Point(x, y);
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            return btn;
        }

        private void CargarCombos()
        {
            try
            {
                // Cargar choferes disponibles
                ComboBox cmbChoferes = (ComboBox)this.Controls.Find("cmbChoferes", true)[0];
                cmbChoferes.Items.Clear();

                var choferes = choferBL.ObtenerChoferesDisponibles();
                foreach (var chofer in choferes)
                {
                    cmbChoferes.Items.Add(new
                    {
                        Id = chofer.Id,
                        Display = $"{chofer.Nombre} {chofer.Apellido} - {chofer.Cedula}"
                    });
                }

                // Cargar autobuses disponibles
                ComboBox cmbAutobuses = (ComboBox)this.Controls.Find("cmbAutobuses", true)[0];
                cmbAutobuses.Items.Clear();

                var autobuses = autobusBL.ObtenerAutobusesDisponibles();
                foreach (var autobus in autobuses)
                {
                    cmbAutobuses.Items.Add(new
                    {
                        Id = autobus.Id,
                        Display = $"{autobus.Marca} {autobus.Modelo} - {autobus.Placa}"
                    });
                }

                // Cargar rutas disponibles
                ComboBox cmbRutas = (ComboBox)this.Controls.Find("cmbRutas", true)[0];
                cmbRutas.Items.Clear();

                var rutas = rutaBL.ObtenerRutasDisponibles();
                foreach (var ruta in rutas)
                {
                    cmbRutas.Items.Add(new
                    {
                        Id = ruta.Id,
                        Display = ruta.Nombre
                    });
                }

                // Seleccionar primeros items
                if (cmbChoferes.Items.Count > 0) cmbChoferes.SelectedIndex = 0;
                if (cmbAutobuses.Items.Count > 0) cmbAutobuses.SelectedIndex = 0;
                if (cmbRutas.Items.Count > 0) cmbRutas.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar combos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarAsignacionesActivas()
        {
            try
            {
                DataGridView dgv = (DataGridView)this.Controls.Find("dgvAsignaciones", true)[0];
                dgv.Rows.Clear();

                List<Asignacion> asignaciones = asignacionBL.ObtenerAsignacionesActivas();

                foreach (var asignacion in asignaciones)
                {
                    dgv.Rows.Add(
                        asignacion.Id,
                        asignacion.NombreChofer,
                        asignacion.PlacaAutobus,
                        asignacion.NombreRuta,
                        asignacion.FechaAsignacion.ToString("dd/MM/yyyy HH:mm"),
                        "Finalizar"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar asignaciones: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                ComboBox cmbChoferes = (ComboBox)this.Controls.Find("cmbChoferes", true)[0];
                ComboBox cmbAutobuses = (ComboBox)this.Controls.Find("cmbAutobuses", true)[0];
                ComboBox cmbRutas = (ComboBox)this.Controls.Find("cmbRutas", true)[0];

                if (cmbChoferes.SelectedItem == null || cmbAutobuses.SelectedItem == null || cmbRutas.SelectedItem == null)
                {
                    MessageBox.Show("Debe seleccionar chofer, autobús y ruta", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int choferId = (int)cmbChoferes.SelectedItem.GetType().GetProperty("Id").GetValue(cmbChoferes.SelectedItem);
                int autobusId = (int)cmbAutobuses.SelectedItem.GetType().GetProperty("Id").GetValue(cmbAutobuses.SelectedItem);
                int rutaId = (int)cmbRutas.SelectedItem.GetType().GetProperty("Id").GetValue(cmbRutas.SelectedItem);

                Asignacion asignacion = new Asignacion
                {
                    ChoferId = choferId,
                    AutobusId = autobusId,
                    RutaId = rutaId
                };

                bool resultado = asignacionBL.CrearAsignacion(asignacion);

                if (resultado)
                {
                    MessageBox.Show("Asignación creada correctamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarFormulario();
                    CargarCombos();
                    CargarAsignacionesActivas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            CargarCombos();
        }

        private void DgvAsignaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string accion = dgv.Rows[e.RowIndex].Cells["Acciones"].Value.ToString();

                if (accion.Contains("Finalizar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    FinalizarAsignacion(id);
                }
            }
        }

        private void FinalizarAsignacion(int id)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de finalizar esta asignación?",
                    "Confirmar Finalización",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool finalizada = asignacionBL.FinalizarAsignacion(id);

                    if (finalizada)
                    {
                        MessageBox.Show("Asignación finalizada correctamente", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCombos();
                        CargarAsignacionesActivas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al finalizar asignación: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmAsignaciones
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmAsignaciones";
            this.Load += new System.EventHandler(this.frmAsignaciones_Load);
            this.ResumeLayout(false);

        }

        private void frmAsignaciones_Load(object sender, EventArgs e)
        {

        }
    }
}
