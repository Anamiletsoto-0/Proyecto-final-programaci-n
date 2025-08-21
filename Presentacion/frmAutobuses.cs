
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ControlAutobuses.Negocio;
using ControlAutobuses.Entidades;

namespace ControlAutobuses.Presentacion
{
    public partial class frmAutobuses : Form
    {
        private AutobusBL autobusBL;
        private int? autobusEditandoId = null;

        public frmAutobuses()
        {
            InitializeComponent();
            autobusBL = new AutobusBL();
            ConfigurarInterfaz();
            CargarAutobuses();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Autobuses";
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
            lblTitulo.Text = "🚌 Gestión de Autobuses";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(0, 0);
            panel.Controls.Add(lblTitulo);

            // Campos del formulario
            string[] labels = { "Marca:", "Modelo:", "Placa:", "Color:", "Año:" };
            int topPosition = 60;

            for (int i = 0; i < labels.Length; i++)
            {
                Label lbl = new Label();
                lbl.Text = labels[i];
                lbl.Font = new Font("Segoe UI", 10);
                lbl.ForeColor = Color.FromArgb(44, 62, 80);
                lbl.AutoSize = true;
                lbl.Location = new Point(20, topPosition);
                panel.Controls.Add(lbl);

                Control inputControl;
                if (i == 4) // Año
                {
                    NumericUpDown numeric = new NumericUpDown();
                    numeric.Minimum = 1990;
                    numeric.Maximum = DateTime.Now.Year + 1;
                    numeric.Value = DateTime.Now.Year;
                    numeric.Font = new Font("Segoe UI", 10);
                    numeric.Size = new Size(300, 30);
                    numeric.Location = new Point(20, topPosition + 25);
                    numeric.Name = "numAnio";
                    inputControl = numeric;
                }
                else
                {
                    TextBox txt = new TextBox();
                    txt.Font = new Font("Segoe UI", 10);
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.Size = new Size(300, 30);
                    txt.Location = new Point(20, topPosition + 25);

                    switch (i)
                    {
                        case 0: txt.Name = "txtMarca"; break;
                        case 1: txt.Name = "txtModelo"; break;
                        case 2: txt.Name = "txtPlaca"; break;
                        case 3: txt.Name = "txtColor"; break;
                    }

                    inputControl = txt;
                }

                panel.Controls.Add(inputControl);
                topPosition += 70;
            }

            // Botones
            int buttonTop = topPosition + 20;
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
            dgv.Name = "dgvAutobuses";
            dgv.Size = new Size(panel.Width - 60, 250);
            dgv.Location = new Point(20, buttonTop);
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellClick += DgvAutobuses_CellClick;

            // Columnas
            dgv.Columns.Add("Id", "ID");
            dgv.Columns.Add("Marca", "Marca");
            dgv.Columns.Add("Modelo", "Modelo");
            dgv.Columns.Add("Placa", "Placa");
            dgv.Columns.Add("Color", "Color");
            dgv.Columns.Add("Anio", "Año");
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

        private void CargarAutobuses()
        {
            try
            {
                DataGridView dgv = (DataGridView)this.Controls.Find("dgvAutobuses", true)[0];
                dgv.Rows.Clear();

                List<Autobus> autobuses = autobusBL.ObtenerAutobuses();

                foreach (var autobus in autobuses)
                {
                    dgv.Rows.Add(
                        autobus.Id,
                        autobus.Marca,
                        autobus.Modelo,
                        autobus.Placa,
                        autobus.Color,
                        autobus.Anio,
                        "Editar | Eliminar"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar autobuses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string marca = this.Controls.Find("txtMarca", true)[0].Text;
                string modelo = this.Controls.Find("txtModelo", true)[0].Text;
                string placa = this.Controls.Find("txtPlaca", true)[0].Text;
                string color = this.Controls.Find("txtColor", true)[0].Text;
                int anio = (int)((NumericUpDown)this.Controls.Find("numAnio", true)[0]).Value;

                Autobus autobus = new Autobus
                {
                    Marca = marca,
                    Modelo = modelo,
                    Placa = placa,
                    Color = color,
                    Anio = anio
                };

                bool resultado;
                Button btnGuardar = (Button)sender;

                if (btnGuardar.Text == "Guardar")
                {
                    resultado = autobusBL.CrearAutobus(autobus);
                    if (resultado) MessageBox.Show("Autobús registrado correctamente");
                }
                else
                {
                    autobus.Id = autobusEditandoId.Value;
                    resultado = autobusBL.ActualizarAutobus(autobus);
                    if (resultado) MessageBox.Show("Autobús actualizado correctamente");
                }

                if (resultado)
                {
                    LimpiarFormulario();
                    CargarAutobuses();
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
            this.Controls.Find("txtMarca", true)[0].Text = "";
            this.Controls.Find("txtModelo", true)[0].Text = "";
            this.Controls.Find("txtPlaca", true)[0].Text = "";
            this.Controls.Find("txtColor", true)[0].Text = "";
            ((NumericUpDown)this.Controls.Find("numAnio", true)[0]).Value = DateTime.Now.Year;

            Button btnGuardar = (Button)this.Controls.Find("btnGuardar", true)[0];
            btnGuardar.Text = "Guardar";

            Button btnCancelar = (Button)this.Controls.Find("btnCancelar", true)[0];
            btnCancelar.Visible = false;

            autobusEditandoId = null;
        }

        private void DgvAutobuses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                string accion = dgv.Rows[e.RowIndex].Cells["Acciones"].Value.ToString();

                if (accion.Contains("Editar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    EditarAutobus(id);
                }
                else if (accion.Contains("Eliminar"))
                {
                    int id = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Id"].Value);
                    EliminarAutobus(id);
                }
            }
        }

        private void EditarAutobus(int id)
        {
            try
            {
                Autobus autobus = autobusBL.ObtenerAutobusPorId(id);

                if (autobus != null)
                {
                    this.Controls.Find("txtMarca", true)[0].Text = autobus.Marca;
                    this.Controls.Find("txtModelo", true)[0].Text = autobus.Modelo;
                    this.Controls.Find("txtPlaca", true)[0].Text = autobus.Placa;
                    this.Controls.Find("txtColor", true)[0].Text = autobus.Color;
                    ((NumericUpDown)this.Controls.Find("numAnio", true)[0]).Value = autobus.Anio;

                    Button btnGuardar = (Button)this.Controls.Find("btnGuardar", true)[0];
                    btnGuardar.Text = "Actualizar";

                    Button btnCancelar = (Button)this.Controls.Find("btnCancelar", true)[0];
                    btnCancelar.Visible = true;

                    autobusEditandoId = id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar autobús: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarAutobus(int id)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de eliminar este autobús?",
                    "Confirmar Eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    bool eliminado = autobusBL.EliminarAutobus(id);

                    if (eliminado)
                    {
                        MessageBox.Show("Autobús eliminado correctamente");
                        CargarAutobuses();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar autobús: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmAutobuses
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "frmAutobuses";
            this.Load += new System.EventHandler(this.frmAutobuses_Load);
            this.ResumeLayout(false);

        }

        private void frmAutobuses_Load(object sender, EventArgs e)
        {

        }
    }
}