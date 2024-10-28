using System.Data;
using System.Drawing;
using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace Cadastro_de_Clientes__C__Com_Edivam_Cabral_
{
    class C_Funcoes
    {

        public static void ImprimirPDF(ReportViewer report, string nomeArquivo, ReportParameterCollection p = null)
        {

            if (p != null)
            {

                report.LocalReport.SetParameters(p);

            }

            report.Refresh();
            report.RefreshReport();

            try
            {

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = report.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                using (FileStream fs = new FileStream(nomeArquivo + ".pdf", FileMode.Create))
                {

                    fs.Write(bytes, 0, bytes.Length);

                }

                System.Diagnostics.Process.Start(nomeArquivo + ".pdf");

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "C# Cadastro de Clientes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        public static void PriMaiuscula(System.Windows.Forms.Control ctr)
        {

            string originalText = ctr.Text.ToLower();
            string formattedText = "";

            string[] words = originalText.Split(' ');

            foreach (string word in words)
            {

                if (word.Length >= 3)
                {

                    formattedText += word.Substring(0, 1).ToUpper() + word.Substring(1) + " ";

                }

                else
                {

                    formattedText += word + " ";

                }

            }

            formattedText = formattedText.Trim();

            ctr.Text = formattedText;

            if (ctr is System.Windows.Forms.TextBox txt)
            {

                txt.SelectionStart = txt.Text.Length;

            }

            else if (ctr is ComboBox cb)
            {

                cb.SelectionStart = cb.Text.Length;

            }

        }


        public static DataTable BuscaSQL(string ComandoSql)
        {

            DataTable dt = new DataTable();

            using (MySqlConnection Conexao = new MySqlConnection(@"Server = localhost; Port = 3306; Database= base_clientes; User = root; Password = "))
            {

                Conexao.Open();

                using (MySqlCommand cmd = Conexao.CreateCommand())
                {

                    cmd.CommandText = ComandoSql;

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {

                        da.Fill(dt);

                    }

                }


            }

            return dt;

        }

        public static void SalvarImagemPequena(string ArquivoOriginal, string NovaFoto, int Largura, int Altura, bool onlyResizeIfWider)
        {
            Image TamanhoImagem = Image.FromFile(ArquivoOriginal);

            TamanhoImagem.RotateFlip(RotateFlipType.Rotate180FlipNone);
            TamanhoImagem.RotateFlip(RotateFlipType.Rotate180FlipNone);

            if (onlyResizeIfWider)
            {
                if (TamanhoImagem.Width <= Largura)
                {
                    Largura = TamanhoImagem.Width;
                }
            }

            int newHeight = TamanhoImagem.Height * Largura / TamanhoImagem.Width;

            if (newHeight > Altura)
            {
                Largura = TamanhoImagem.Width * Altura / TamanhoImagem.Height;
                newHeight = Altura;
            }

            Image NovaImagem = TamanhoImagem.GetThumbnailImage(Largura, newHeight, null, IntPtr.Zero);

            TamanhoImagem.Dispose();

            NovaImagem.Save(NovaFoto);
        }

    }

}
