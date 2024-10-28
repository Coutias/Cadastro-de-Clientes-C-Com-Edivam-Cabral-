using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data;
using Org.BouncyCastle.Tls;

namespace Cadastro_de_Clientes__C__Com_Edivam_Cabral_
{
    public partial class F_MenuClientes : Form
    {

        public F_MenuClientes()
        {

            InitializeComponent();

        }

        string PastaFotos = AppDomain.CurrentDomain.BaseDirectory + "/Fotos/";

        private string GerarCriterios()
        {

            string c = "";

            if (tb_pesquisaCodigo.Text != string.Empty)
            {

                c += " AND id = " + tb_pesquisaCodigo.Text;

            }

            if (tb_pesquisaNome.Text != string.Empty)
            {

                c += $" AND (nome LIKE '%{tb_pesquisaNome.Text}%' OR documento LIKE '%{tb_pesquisaNome.Text}%'  OR documento LIKE '%{tb_pesquisaNome.Text}%')";

            }

            if (cb_pesquisaGenero.Text != string.Empty)
            {

                c += $" AND genero = '{cb_pesquisaGenero.Text}'";

            }

            if (cb_pesquisaEstadoCivil.Text != string.Empty)
            {

                c += $" AND estado_civil = '{cb_pesquisaEstadoCivil.Text}'";

            }

            if (tb_pesquisaEndereco.Text != string.Empty)
            {

                string e = tb_pesquisaEndereco.Text;

                c += $" AND (cep LIKE '%{e}%' OR endereco LIKE '%{e}%' OR numero LIKE '%{e}%' OR bairro LIKE '%{e}%' OR estado LIKE '%{e}%')";

            }

            try
            {

                DateTime data = Convert.ToDateTime(tb_pesquisaNasc.Text);
                c += $"AND nasc = '{data.ToString("yyyy-MM-dd")}'";

            }
            catch (Exception)
            {

                

            }

            if (rb_pesquisaAtivo.Checked == true)
            {

                c += " AND situacao = 'Ativo' ";

            }

            else if (rb_pesquisaCancelado.Checked == true)
            {

                c += " AND situacao = 'Cancelado' ";

            }

            return c;

        }

        private void ReorganizarDataGridView()
        {

            foreach (DataGridViewRow lin in dgv_lista.Rows)
            {

                if (lin.Cells["situacao"].Value.ToString() == "Cancelado")
                {

                    lin.DefaultCellStyle.ForeColor = Color.Crimson;

                }

                if (File.Exists(PastaFotos + lin.Cells["id"].Value.ToString() + ".png"))
                {

                    lin.Cells["foto"].Value = Image.FromFile(PastaFotos + lin.Cells["id"].Value.ToString() + ".png");

                }

                else
                {

                    lin.Cells["foto"].Value = Properties.Resources.Pessoa;

                }

            }

            btn_edit.Enabled = false;
            btn_ficha.Enabled = false;

        }

        private void BuscarClientes()
        {

            dgv_lista.DataSource = C_Funcoes.BuscaSQL("SELECT * FROM clientes WHERE 1 " + GerarCriterios());

            if ((dgv_lista.RowCount * 40) + 50 > 572)
            {

                dgv_lista.Height = 572;

            }

            else
            {

                dgv_lista.Height = (dgv_lista.RowCount * 40) + 50;

            }

            ReorganizarDataGridView();

            Rodape();

            if (dgv_lista.RowCount == 0)
            {

                lb_aviso.Visible = true;

            }

            else
            {

                lb_aviso.Visible = false;

            }

        }

        private void Rodape()
        {

            lb_totalLocalizado.Text = "Total Localizados: " + dgv_lista.RowCount;

            int contador = 0;

            foreach (DataGridViewRow lin in dgv_lista.Rows)
            {

                if (lin.Cells["situacao"].Value.ToString() == "Cancelado")
                {

                    contador++;

                }

            }

            lb_totalCancelados.Text = "Total Cancelados: " + contador.ToString();
            lb_totalAtivos.Text = "Total Ativos: " + (dgv_lista.RowCount - contador).ToString();

        }

        private void ibtn_close_Click(object sender, EventArgs e)
        {

            Thread.Sleep(500);
            Application.Exit();

        }





        private void btn_add_Click(object sender, EventArgs e)
        {

            F_cadCliente frm = new F_cadCliente();
            frm.ShowDialog();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string caminhoArquivo = @"D:\CURSOS DE C#\PROJETOS DAS AULAS DE C#\Cadastro de Clientes (C# Com Edivam Cabral)\Cadastro de Clientes (C# Com Edivam Cabral)\bin\Debug\dadosClientes.json";

            string conteudoJson = System.IO.File.ReadAllText(caminhoArquivo);

            dynamic objetoJson = Newtonsoft.Json.JsonConvert.DeserializeObject(conteudoJson);

            string comandoSql = "INSERT INTO clientes (nome, documento, genero, rg, estado_civil, nasc, cep, endereco, bairro, cidade, estado, celular, email, obs, situacao, numero) " +
                                            "VALUES (@nome, @documento, @genero, @rg, @estado_civil, @nasc, @cep, @endereco, @bairro, @cidade, @estado, @celular, @email, @obs, @situacao, @numero)";

            using (MySqlConnection Conexao = new MySqlConnection(@"Server = localhost; Port = 3306; Database= base_clientes; User = root; Password = "))
            {
                Conexao.Open();

                foreach (var item in objetoJson)
                {
                    using (MySqlCommand cmd = Conexao.CreateCommand())
                    {
                        cmd.CommandText = comandoSql;

                        cmd.Parameters.AddWithValue("@nome", item.nome.ToString());
                        cmd.Parameters.AddWithValue("@documento", item.documento.ToString());
                        cmd.Parameters.AddWithValue("@genero", item.genero.ToString());
                        cmd.Parameters.AddWithValue("@rg", item.rg.ToString());
                        cmd.Parameters.AddWithValue("@estado_civil", item.estado_civil.ToString());
                        cmd.Parameters.AddWithValue("@nasc", Convert.ToDateTime(item.nasc.ToString()));
                        cmd.Parameters.AddWithValue("@cep", item.cep.ToString());
                        cmd.Parameters.AddWithValue("@endereco", item.endereco.ToString());
                        cmd.Parameters.AddWithValue("@bairro", item.bairro.ToString());
                        cmd.Parameters.AddWithValue("@cidade", item.cidade.ToString());
                        cmd.Parameters.AddWithValue("@estado", item.estado.ToString());
                        cmd.Parameters.AddWithValue("@celular", item.celular.ToString());
                        cmd.Parameters.AddWithValue("@email", item.email.ToString());
                        cmd.Parameters.AddWithValue("@obs", item.obs.ToString());
                        cmd.Parameters.AddWithValue("@situacao", item.situacao.ToString());
                        cmd.Parameters.AddWithValue("@numero", item.numero.ToString());

                        cmd.ExecuteNonQuery();

                    }
                }
            }
        }

        private void F_MenuClientes_Load(object sender, EventArgs e)
        {

            BuscarClientes();

            ReorganizarDataGridView();

        }

        private void dgv_lista_Sorted(object sender, EventArgs e)
        {

            ReorganizarDataGridView();

        }

        private void dgv_lista_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            btn_edit.Enabled = true;
            btn_ficha.Enabled = true;

        }

        private void btn_edit_Click(object sender, EventArgs e)
        {

            F_cadCliente form1 = new F_cadCliente();

            form1.tb_codigo.Text = dgv_lista.CurrentRow.Cells["id"].Value.ToString();

            form1.ShowDialog();

            ReorganizarDataGridView();

            BuscarClientes();

        }

        private void tb_pesquisaCodigo_TextChanged(object sender, EventArgs e)
        {

            dgv_lista.DataSource = C_Funcoes.BuscaSQL("SELECT * FROM clientes");
            BuscarClientes();
            ReorganizarDataGridView();

        }

        private void tb_pesquisaNome_TextChanged(object sender, EventArgs e)
        {

            BuscarClientes();

        }

        private void rb_pesquisaTodos_CheckedChanged(object sender, EventArgs e)
        {

            if (rb_pesquisaTodos.Checked == true)
            {

                BuscarClientes();

            }

        }

        private void rb_pesquisaAtivo_CheckedChanged(object sender, EventArgs e)
        {

            if (rb_pesquisaAtivo.Checked == true)
            {

                BuscarClientes();

            }

        }

        private void rb_pesquisaCancelado_CheckedChanged(object sender, EventArgs e)
        {

            if (rb_pesquisaCancelado.Checked == true)
            {

                BuscarClientes();

            }

        }

        private void dgv_lista_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)
            {

                return;

            }
            DataGridViewRow lin = dgv_lista.Rows[e.RowIndex];

            lin.DefaultCellStyle.BackColor = Color.AliceBlue;
            lin.Cells["foto"].Value = lin.Cells["foto"].Value;

        }

        private void dgv_lista_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)
            {

                return;

            }

            dgv_lista.Rows[e.RowIndex].DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0 ? Color.FromArgb(209, 196, 233) : Color.FromArgb(224, 215, 240));

        }

        private void btn_pdf_Click(object sender, EventArgs e)
        {

            DataTable dt = C_Funcoes.BuscaSQL("SELECT * FROM clientes");

            DS.DadosClienteDataTable dtCli = new DS.DadosClienteDataTable();
            dtCli.Merge(dt);

            ReportDataSource rds = new ReportDataSource("DataSet1", dtCli as DataTable);
            rv_reportRelatorio.LocalReport.DataSources.Clear();
            rv_reportRelatorio.LocalReport.DataSources.Add(rds);


            C_Funcoes.ImprimirPDF(rv_reportRelatorio, "RelatórioClientes");

        }

        private void btn_ficha_Click(object sender, EventArgs e)
        {

            string id = dgv_lista.CurrentRow.Cells["id"].Value.ToString();

            DataTable dt = C_Funcoes.BuscaSQL("SELECT * FROM clientes WHERE id = " +  id);

            DS.DadosClienteDataTable dtCli = new DS.DadosClienteDataTable();
            dtCli.Merge(dt);

            if (dtCli.Rows[0]["foto"] != DBNull.Value)
            {

                dtCli.Rows[0]["foto"] = dt.Rows[0]["foto"];

            }

            else
            {

                using (MemoryStream ms = new MemoryStream())
                {

                    Properties.Resources.Pessoa.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    dtCli.Rows[0]["foto"] = ms.ToArray();

                }

            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dtCli as DataTable);
            rv_reportFicha.LocalReport.DataSources.Clear();
            rv_reportFicha.LocalReport.DataSources.Add(rds);


            C_Funcoes.ImprimirPDF(rv_reportFicha, "FichaCadastral");

        }

    }

}

/*
 if (lin["foto"] != DBNull.Value)
            {

                linha["foto"] = lin["foto"];

            }

            else
            {

                using (MemoryStream ms = new MemoryStream())
                {

                    Properties.Resources.Pessoa.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    linha["foto"] = ms.ToArray();

                }

            }
 */

/*
            DS.DadosClienteDataTable dtCli = new DS.DadosClienteDataTable();
            DataRow linha = dtCli.NewRow();
            
            dtCli.Rows.Add(linha);

            ReportDataSource rds = new ReportDataSource("DataSet1", dtCli as DataTable);
            rv_reportFicha.LocalReport.DataSources.Clear();
            rv_reportFicha.LocalReport.DataSources.Add(rds);

            ReportParameterCollection p = new ReportParameterCollection
            {

                new ReportParameter("foto", "File://"),
                new ReportParameter("nome", lin["nome"].ToString()),
                new ReportParameter("documento", lin["documento"].ToString()),
                new ReportParameter("rg", lin["rg"].ToString()),
                new ReportParameter("nasc", lin["nasc"].ToString().Replace(" 00:00:00", "")),
                new ReportParameter("celular", lin["celular"].ToString()),
                new ReportParameter("email", lin["email"].ToString()),
                new ReportParameter("genero", lin["genero"].ToString()),
                new ReportParameter("estado_civil", lin["estado_civil"].ToString()),
                new ReportParameter("endereco", lin["endereco"].ToString()),
                new ReportParameter("numero", lin["numero"].ToString()),
                new ReportParameter("bairro", lin["bairro"].ToString()),
                new ReportParameter("cidade", lin["cidade"].ToString()),
                new ReportParameter("estado", lin["estado"].ToString()),
                new ReportParameter("cep", lin["cep"].ToString()),
                new ReportParameter("obs", lin["obs"].ToString()),
                new ReportParameter("situacao", lin["situacao"].ToString())
                

            };
            */