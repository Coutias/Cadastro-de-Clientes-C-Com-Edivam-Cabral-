using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Cadastro_de_Clientes__C__Com_Edivam_Cabral_
{
    public partial class F_cadCliente : Form
    {
        public F_cadCliente()
        {
            InitializeComponent();
        }

        string strCon = "Server = localhost; Port = 3306; Database= base_clientes; User = root; Password = ";
        string PastaFotos = AppDomain.CurrentDomain.BaseDirectory + "/Fotos/";

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

            Thread.Sleep(800);
            Close();

        }

        private void F_cadCliente_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                SendKeys.Send("{Tab}");
                e.SuppressKeyPress = true;

            }

        }

        private void SalvarClienteMySQL()
        {

            using (MySqlConnection Conexao = new MySqlConnection(strCon))
            {

                Conexao.Open();

                using (MySqlCommand cmd = Conexao.CreateCommand())
                {

                    if (tb_codigo.Text == "")
                    {

                        cmd.CommandText = "INSERT INTO clientes(nome, documento, genero, rg, estado_civil, nasc, cep, endereco, numero, bairro, cidade, estado, celular, email, obs, situacao) VALUES(@nome, @documento, @genero, @rg, @estado_civil, @nasc, @cep, @endereco, @numero, @bairro, @cidade, @estado, @celular, @email, @obs, @situacao)";

                    }

                    else
                    {

                        cmd.CommandText = "UPDATE clientes SET nome = @nome, documento = @documento, genero = @genero, rg = @rg, estado_civil = @estado_civil, nasc = @nasc, cep = @cep, endereco = @endereco, numero = @numero, bairro = @bairro, cidade = @cidade, estado = @estado, celular = @celular, email = @email, obs = @obs, situacao = @situacao WHERE id = " + tb_codigo.Text;

                    }

                    cmd.Parameters.AddWithValue("@nome", tb_nomeCliente.Text);
                    cmd.Parameters.AddWithValue("@documento", mtb_documento.Text);

                    if (rb_masculino.Checked == true)
                    {

                        cmd.Parameters.AddWithValue("@genero", "Masculino");

                    }

                    else if (rb_feminino.Checked == true)
                    {

                        cmd.Parameters.AddWithValue("@genero", "Feminino");

                    }

                    else
                    {

                        cmd.Parameters.AddWithValue("@genero", "Outros");

                    }

                    cmd.Parameters.AddWithValue("@rg", tb_rg.Text);
                    cmd.Parameters.AddWithValue("@estado_civil", cb_estadoCiv.Text);

                    // Conversão de data do formulário para o DB do MySQL
                    if (mtb_dataNasc.Text == "  /  /")
                    {
                        cmd.Parameters.AddWithValue("@nasc", DBNull.Value);
                    }
                    else
                    {
                        if (DateTime.TryParse(mtb_dataNasc.Text, out DateTime dataNasc))
                        {
                            cmd.Parameters.AddWithValue("@nasc", dataNasc);
                        }
                        else
                        {
                            MessageBox.Show("Data de nascimento inválida. Por favor, verifique o formato.");
                            return;
                        }
                    }

                    cmd.Parameters.AddWithValue("@cep", mtb_cep.Text);
                    cmd.Parameters.AddWithValue("@endereco", tb_endereco.Text);
                    cmd.Parameters.AddWithValue("@numero", tb_numero.Text);
                    cmd.Parameters.AddWithValue("@bairro", tb_bairro.Text);
                    cmd.Parameters.AddWithValue("@cidade", tb_cidade.Text);
                    cmd.Parameters.AddWithValue("@estado", cb_estado.Text);
                    cmd.Parameters.AddWithValue("@celular", mtb_celular.Text);
                    cmd.Parameters.AddWithValue("@email", tb_email.Text);
                    cmd.Parameters.AddWithValue("@obs", tb_obs.Text);

                    if (cb_situacao.Checked == true)
                    {

                        cmd.Parameters.AddWithValue("@situacao", "Ativo");

                    }

                    else
                    {

                        cmd.Parameters.AddWithValue("@situacao", "Inativo");

                    }

                    cmd.ExecuteNonQuery();

                    if (tb_codigo.Text == "")
                    {

                        cmd.CommandText = "SELECT @@IDENTITY";
                        tb_codigo.Text = cmd.ExecuteScalar().ToString();

                    }

                }

                MessageBox.Show("Tudo feito [OK]");

                Conexao.Close();

            }

        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {

            if (Validacoes() == true)
            {

                return;

            }

            SalvarClienteMySQL();

            tb_nomeCliente.Focus();

        }

        private bool Validacoes()
        {

            // Validações

            // Validar Campo Nome
            if (tb_nomeCliente.Text == "")
            {

                MessageBox.Show("Campo nome é obrigatório!");
                tb_nomeCliente.Focus();
                return true;

            }

            // Validar Campo CPF/CNPJ
            if (rb_cpf.Checked == false && rb_cnpj.Checked == false)
            {

                MessageBox.Show("Campo de documentação é obrigatório!\rMarque o CPF ou CNPJ");
                return true;

            }

            // Validar Campo Marcação Documentação
            if (mtb_documento.Text == "")
            {

                if (rb_cpf.Checked == true)
                {

                    MessageBox.Show("Digite o CPF");

                }

                else
                {

                    MessageBox.Show("Digite o CNPJ");

                }

                mtb_documento.Focus();
                return true;

            }

            // Verifica se marcou pelo menos uma das opções do genero

            if (rb_masculino.Checked == false && rb_feminino.Checked == false && rb_outros.Checked == false)
            {

                MessageBox.Show("Selecione o Gênero");
                return true;

            }

            // Validar se data é válida

            if (mtb_dataNasc.Text != "  /  /")
            {

                try
                {

                    Convert.ToDateTime(mtb_dataNasc.Text);

                }
                catch (Exception)
                {

                    MessageBox.Show("Data de nascimento não é válida");
                    return true;

                }

            }

            return false;

        }

        private void btn_novo_Click(object sender, EventArgs e)
        {

            if (md_novo.Show() == DialogResult.Yes)
            {

                tb_codigo.Text = "";
                tb_nomeCliente.Text = "";

                rb_cpf.Checked = true;
                rb_cnpj.Checked = false;
                mtb_documento.Text = "";

                rb_masculino.Checked = false;
                rb_feminino.Checked = false;
                rb_outros.Checked = true;

                tb_rg.Text = "";
                cb_estadoCiv.Text = "";
                mtb_dataNasc.Text = "";

                mtb_cep.Text = "";
                tb_endereco.Text = "";
                tb_numero.Text = "";
                tb_bairro.Text = "";
                tb_cidade.Text = "";
                cb_estado.Text = "";
                mtb_celular.Text = "";
                tb_email.Text = "";

                tb_obs.Text = "";
                cb_situacao.Checked = true;

                tb_nomeCliente.Focus();

                btn_salvar.Text = "Salvar";

                pb_image.Image = Properties.Resources.Pessoa;

            }

        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {

            Close();

        }

        private void rb_cpf_CheckedChanged(object sender, EventArgs e)
        {

            if (rb_cpf.Checked == true)
            {

                mtb_documento.Mask = "000.000.000-00";
                mtb_documento.Focus();

            }

        }

        private void rb_cnpj_CheckedChanged(object sender, EventArgs e)
        {

            if (rb_cnpj.Checked == true)
            {

                mtb_documento.Mask = "00.000.000/0000-00";
                mtb_documento.Focus();

            }

        }

        private void rb_masculino_CheckedChanged(object sender, EventArgs e)
        {

            tb_rg.Focus();

        }

        private void rb_feminino_CheckedChanged(object sender, EventArgs e)
        {

            tb_rg.Focus();

        }

        private void rb_outros_CheckedChanged(object sender, EventArgs e)
        {

            tb_rg.Focus();

        }

        private void mtb_dataNasc_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (mtb_dataNasc.Text == "  /  /")
            {

                return;

            }

            try
            {

                mtb_dataNasc.Text = Convert.ToDateTime(mtb_dataNasc.Text).ToString();

            }
            catch (Exception)
            {

                md_datainvalida.Show();
                e.Cancel = true;

            }


        }

        private void cb_estadoCiv_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (cb_estadoCiv.Text == "")
            {

                return;

            }

            if (cb_estadoCiv.SelectedIndex == -1)
            {

                md_estadoCivil.Show();
                e.Cancel = true;

            }

        }

        private void cb_estado_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (cb_estado.Text == "")
            {

                return;

            }

            if (cb_estado.SelectedIndex == -1)
            {

                md_estado.Show();
                e.Cancel = true;

            }

        }

        private void tb_nomeCliente_TextChanged(object sender, EventArgs e)
        {



        }

        private void mtb_cep_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (mtb_cep.Text.Replace(" ", "").Length == 0)
            {

                return;

            }

            if (mtb_cep.Text.Replace(" ", "").Length < 8)
            {

                md_informacao.Show();
                e.Cancel = true;


            }

        }

        private void mtb_documento_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (mtb_documento.Text == "")
            {

                return;

            }

            if (rb_cpf.Checked == true && mtb_documento.Text.Replace(" ", "").Length < 11)
            {

                md_informacao.Show();
                e.Cancel = true;

            }

            if (rb_cnpj.Checked == true && mtb_documento.Text.Replace(" ", "").Length < 14)
            {

                md_informacao.Show();
                e.Cancel = true;

            }

        }

        private void tb_endereco_TextChanged(object sender, EventArgs e)
        {



        }

        private void tb_bairro_TextChanged(object sender, EventArgs e)
        {



        }

        private void tb_cidade_TextChanged(object sender, EventArgs e)
        {



        }

        private void tb_nomeCliente_KeyUp(object sender, KeyEventArgs e)
        {



        }

        private void tb_nomeCliente_TextChanged_1(object sender, EventArgs e)
        {



        }

        private void tb_nomeCliente_Leave(object sender, EventArgs e)
        {

            C_Funcoes.PriMaiuscula(tb_nomeCliente);

        }

        private void tb_endereco_Leave(object sender, EventArgs e)
        {

            C_Funcoes.PriMaiuscula(tb_endereco);

        }

        private void tb_bairro_Leave(object sender, EventArgs e)
        {

            C_Funcoes.PriMaiuscula(tb_bairro);

        }

        private void tb_cidade_Leave(object sender, EventArgs e)
        {

            C_Funcoes.PriMaiuscula(tb_cidade);

        }

        byte[] imgBytes;

        private void ib_add_Click(object sender, EventArgs e)
        {

            if (tb_codigo.Text == "")
            {

                MessageBox.Show("Salve os dados do cliente primeiro");
                return;

            }

            OpenFileDialog caixa = new OpenFileDialog();

            caixa.Filter = "Arquivos de Imagem | *.jpg; *.jpeg; *.png; *.gif; *.bmp";

            if (caixa.ShowDialog() == DialogResult.OK)
            {

                // Falta algo..

            }

            C_Funcoes.SalvarImagemPequena(caixa.FileName, AppDomain.CurrentDomain.BaseDirectory + "/FotoTemp.png", pb_image.Width, pb_image.Height, false);

            pb_image.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/FotoTemp.png");
            imgBytes = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/FotoTemp.png");

            using (MySqlConnection Conexao = new MySqlConnection(strCon))
            {
                
                Conexao.Open();

                using (MySqlCommand cmd = Conexao.CreateCommand())
                {

                    cmd.CommandText = "UPDATE clientes SET foto = @foto WHERE id = @id";

                    cmd.Parameters.AddWithValue("@foto", imgBytes);
                    cmd.Parameters.AddWithValue("@id", tb_codigo.Text);

                    cmd.ExecuteNonQuery();

                }
            }

        }

        private void ib_remove_Click(object sender, EventArgs e)
        {

            if (tb_codigo.Text == "")
            {

                md_falhaFoto.Show("Não tem foto para ser removida");
                return;

            }

            if (File.Exists(PastaFotos + tb_codigo.Text + ".png"))
            {

                md_falhaFoto.Show("Não foi encontrada foto para ser removida");
                return;

            }

            if (md_removerFoto.Show("Deseja realmente remover essa foto?") == DialogResult.No)
            {

                return;

            }

            pb_image.Image = Properties.Resources.Pessoa;

            File.Delete(PastaFotos + tb_codigo.Text + ".png");

        }

        private void F_cadCliente_Load(object sender, EventArgs e)
        {

            if (tb_codigo.Text == "")
            {

                return;

            }

            btn_salvar.Text = "Atualizar";

            DataTable dt = C_Funcoes.BuscaSQL("SELECT * FROM clientes WHERE id = " + tb_codigo.Text);


            tb_nomeCliente.Text = dt.Rows[0]["nome"].ToString();
            tb_rg.Text = dt.Rows[0]["rg"].ToString();
            cb_estadoCiv.Text = dt.Rows[0]["estado_civil"].ToString();
            mtb_dataNasc.Text = dt.Rows[0]["nasc"].ToString();
            mtb_cep.Text = dt.Rows[0]["cep"].ToString();
            tb_endereco.Text = dt.Rows[0]["endereco"].ToString();
            tb_numero.Text = dt.Rows[0]["numero"].ToString();
            tb_bairro.Text = dt.Rows[0]["bairro"].ToString();
            tb_cidade.Text = dt.Rows[0]["cidade"].ToString();
            cb_estado.Text = dt.Rows[0]["estado"].ToString();
            mtb_celular.Text = dt.Rows[0]["celular"].ToString();
            tb_email.Text = dt.Rows[0]["email"].ToString();
            tb_obs.Text = dt.Rows[0]["obs"].ToString();

            if (dt.Rows[0]["documento"].ToString().Length == 11)
            {

                rb_cpf.Checked = true;

            }

            else
            {

                rb_cnpj.Checked = true;

            }

            mtb_documento.Text = dt.Rows[0]["documento"].ToString();

            if (dt.Rows[0]["genero"].ToString() == "Masculino")
            {

                rb_masculino.Checked = true;

            }

            else if (dt.Rows[0]["genero"].ToString() == "Feminino")
            {

                rb_feminino.Checked = true;

            }

            else
            {

                rb_outros.Checked = true;

            }

            if (dt.Rows[0]["situacao"].ToString() == "Ativo")
            {

                cb_situacao.Checked = true;

            }

            else
            {

                cb_situacao.Checked = false;

            }

            if (File.Exists(PastaFotos + tb_codigo.Text + ".png"))
            {

                pb_image.Image = Image.FromFile(PastaFotos + tb_codigo.Text + ".png");

            }

            else
            {

                pb_image.Image = Properties.Resources.Pessoa;

            }

        }

    }

}
