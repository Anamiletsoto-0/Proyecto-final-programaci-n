using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Negocio;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmRutas : Form
    {
        private RutaBL rutaBL;
        private int? rutaEditandoId = null;

        public frmRutas()
        {
            InitializeComponent();
            rutaBL = new RutaBL();
            ConfigurarInterfaz();
            CargarRutas();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Rutas";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(236, 240, 241);

            Panel panel = new Panel();
            panel.BackColor = Color.White;
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);
            this.Controls.Add(panel);

            // Título
            Label lblTitulo = new Label();
            lblTitulo.Text = "🗺️ Gestión de Rutas";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(0, 0);
            panel.Controls.Add(lblTitulo);

            // Campo de nombre
            Label lblNombre = new Label();
            lblNombre.Text = "Nombre de la Ruta:";
            lblNombre.Font = new Font("Segoe UI", 10);
            lblNombre.ForeColor = Color.FromArgb(44, 62, 80);
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(20, 60);
            panel.Controls.Add(lblNombre);

            TextBox txtNombre = new TextBox();
            txtNombre.Name = "txtNombre";
            txtNombre.Font = new Font("Segoe UI", 10);
            txtNombre.BorderStyle = BorderStyle.FixedSingle;
            txtNombre.Size = new Size(300, 30);
            txtNombre.Location = new Point(20, 85);
            panel.Controls.Add(txtNombre);

            // Botones
            int buttonTop = 130;
            Button btnGuardar = CrearBoton("Guardar", Color.FromArgb(44, 62, 80), 20, buttonTop);
            btnGuardar.Click += BtnGuardar_Click;
            btnGuardar.Name = "btnGuardar";
            panel.Controls.Add(btnGuardar);

            Button btnLimpiar = CrearBoton("Limpiar", Color.FromArgb(149, 165, 166), 120, buttonTop);
            btnLimpiar.Click += BtnLimpiar_Click;
            panel.Controls.Add(btnLimpiar);

            Button btnCancelar = CrearBoton("Cancelar", Color.FromArgb(231, 76, 60), 220, buttonTop);
            btnCancelar.Click += BtnCancelar_Click;
            btnCancelar.Visible = false;
            btnCancelar.Name = "btnCancelar";
            panel.Controls.Add(btnCancelar);

            // DataGridView
            buttonTop += 60;
            DataGridView dgv = new DataGridView();
            dgv.Name = "dgvRutas";
            dgv.Size = new Size(panel.Width - 60, 250);
            dgv.Location = new Point(20, buttonTop);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += DgvRutas_CellClick;

            // Columnas
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Nombre", "Nombre de la Ruta");
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

        private void CargarRutas()
        {
            try
            {
                DataGridView dgv = (DataGridView)this.Controls.Find("dgvRutas", true)[0];
                dgv.Rows.Clear();

                List<Ruta> rutas = rutaBL.ObtenerRutas();

                foreach (var ruta in rutas)
                {
                    dgv.Rows.Add(
                        ruta.Id,
                        ruta.Nombre,
                        "Editar | Eliminar"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar rutas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = this.Controls.Find("txtNombre", true)[0].Text;

                Ruta ruta = new Ruta
                {
                    Nombre = nombre
                };

                bool resultado;
                Button btnGuardar = (Button)sender;

                if (btnGuardar.Text == "Guardar")
                {
                    resultado = rutaBL.CrearRuta(ruta);
                    if (resultado) MessageBox.Show("Ruta registrada correctamente");
                }
                else
                {
                    ruta.Id = rutaEditandoId.Value;
                    resultado = rutaBL.ActualizarRuta(ruta);
                    if (resultado) MessageBox.Show("Ruta actualizada correctamente");
                }

                if (resultado)
                {
                    LimpiarFormulario();
                    CargarRutas();
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

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            this.Controls.Find("txtNombre", true)[0].Text = "";

            Button btnGuardar = (Button)this.Controls.Find("btnGuardar", true)[0];
            btnGuardar.Text = "Guardar";

            Button btnCancelar = (Button)this.Controls.Find("btnCancelar", true)[0];
            btnCancelar.Visible = false;

            rutaEditandoId = null;
        }

        private void DgvRutas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string accion = dgv.Rows[e.RowIndex].Cells["Acciones"].Value.ToString();

                if (accion.Contains("Editar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    EditarRuta(id);
                }
                else if (accion.Contains("Eliminar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    EliminarRuta(id);
                }
            }
        }

        private void EditarRuta(int id)
        {
            try
            {
                Ruta ruta = rutaBL.ObtenerRutaPorId(id);

                if (ruta != null)
                {
                    this.Controls.Find("txtNombre", true)[0].Text = ruta.Nombre;

                    Button btnGuardar = (Button)this.Controls.Find("btnGuardar", true)[0];
                    btnGuardar.Text = "Actualizar";

                    Button btnCancelar = (Button)this.Controls.Find("btnCancelar", true)[0];
                    btnCancelar.Visible = true;

                    rutaEditandoId = id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ruta: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarRuta(int id)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de eliminar esta ruta?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool eliminada = rutaBL.EliminarRuta(id);

                    if (eliminada)
                    {
                        MessageBox.Show("Ruta eliminada correctamente");
                        CargarRutas();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar ruta: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmRutas
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmRutas";
            this.Load += new System.EventHandler(this.frmRutas_Load);
            this.ResumeLayout(false);

        }

        private void frmRutas_Load(object sender, EventArgs e)
        {

        }
    }
}