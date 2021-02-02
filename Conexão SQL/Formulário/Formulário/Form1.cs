using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Formulário
{
    public partial class Form1 : Form
    {
        int pos;
        SqlConnection conexao = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=SQL_Agenda;Integrated Security=True;Pooling=False");
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;
        DataTable ds = new DataTable("Dados");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conexao.Open();
            cmd.Connection = conexao;
            cmd.CommandType = CommandType.Text;
            carrega_dados();

        if (ds.Rows.Count <= 0)
        {
            MessageBox.Show("Tabela sem registros, cadstre um ","Tabela vazia");
        }
        else
        {
            navega();
            label6.Text =Convert.ToString(ds.Rows.Count);
        }
        }

        private void Form1_Closed(Object sender, EventArgs e)
        {
            conexao.Close();
        }

        private void Btn_Primeiro_Click(object sender, EventArgs e)
        {
            pos = 0;
            navega();
        }

        private void Btn_Anterior_Click(object sender, EventArgs e)
        {
            if (pos > 0) pos = pos - 1;
            else
            {
                MessageBox.Show("Primeiro registro","Mensagem");
                pos = 0;
            }
            navega();
        }

        private void Btn_Proximo_Click(object sender, EventArgs e)
        {
            if (pos <ds.Rows.Count - 1) pos = pos + 1;
            else
            {
                MessageBox.Show("Não há mais registros","Mensagem");
                pos = ds.Rows.Count - 1;
            }
            navega();
        }

        private void Btn_Ultimo_Click(object sender, EventArgs e)
        {
            pos = ds.Rows.Count - 1;
            navega();
        }

        private void Btn_Novo_Click(object sender, EventArgs e)
        {
            if (ds.Rows.Count < 0) textBox1.Text = Convert.ToString("1");
            else
            {
                codigo = Convert.ToInt32(ds.Rows[ds.Rows.Count - 1]["Código"]);
                textBox1.Text = Convert.ToString(codigo+1);
                textBox4.Clear();
                textBox3.Clear();
                textBox2.Clear();
                textBox2.Focus();
            }
            botoes(false);
        }

        private void Btn_Editar_Click(object sender, EventArgs e)
        {
            botoes(true);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "uptade Dados set nome=@nome,email=@email,endereco=@endereco where codigo=@codigo";

            cmd.Parameters.AddWithValue("@codigo",textBox1.Text);
            cmd.Parameters.AddWithValue("@nome",textBox2.Text);
            cmd.Parameters.AddWithValue("@email",textBox3.Text);
            cmd.Parameters.AddWithValue("@endereco",textBox4.Text);

            cmd.ExecuteNonQuery();
            MessageBox.Show("Dados atualizados com Sucesso","Atualização");
            carrega_dados();
            navega();

        }

        private void Btn_Gravar_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Dados (codigo,nome,email,endereco) Values (@codigo,@nome,@email,@endereco)";

            cmd.Parameters.Add(new SqlParameter("@codigo", this.textBox1.Text));
            cmd.Parameters.Add(new SqlParameter("@nome", this.textBox2.Text));
            cmd.Parameters.Add(new SqlParameter("@email", this.textBox3.Text));
            cmd.Parameters.Add(new SqlParameter("@endereco", this.textBox4.Text));

            cmd.ExecuteNonQuery();
            MessageBox.Show("Dadoscadastrados com Sucesso","Inclusão");
            carrega_dados();
            navega();
            botoes(true);

        }

        private void Btn_Excluir_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conexao;
            cmd.CommandType = CommandType.Text;

            if (MessageBox.Show("Tem certeza que deseja excluir esse registro?","Exclusão", MessageBoxButtons.YesNo) == DialogoResult.Yes)
            {
                cmd.CommandText=("delete from Dados ehere Codigo = @codigo");
                cmd.Parameters.Add(new SqlParameter("@codigo", this.textBox1.Text));
                cmd.ExecuteNonQuery();
                carrega_dados();
                navega();
                botoes(true);
            }
        }

        private void navega()
        {
            textBox1.Text=Convert.ToString(ds.Rows[pos]["Codigos"]);
            textBox2.Text=Convert.ToString(ds.Rows[pos]["Nome"]);
            textBox3.Text=Convert.ToString(ds.Rows[pos]["Email"]);
            textBox4.Text=Convert.ToString(ds.Rows[pos]["Endereco"]);
        }

        private void carrega_dados()
        {
            cmd.CommandText = "select * from Dados";
            dr = cmd.ExecuteReader();
            ds.Clear();
            ds.Load(dr);
            pos = 0;
            label6.Text = Convert.ToString(ds.Rows.Count);
            dataGridView1.DataSource = ds;
        }
        private void botoes (Boolean ativar)
        {
            Btn_Primeiro.Enabled = ativar;
            Btn_Ultimo.Enabled = ativar;
            Btn_Anterior.Enabled = ativar;
            Btn_Proximo.Enabled = ativar;
            Btn_Novo.Enabled = ativar;
            Btn_Gravar.Enabled = ativar;
            Btn_Editar.Enabled = ativar;
            Btn_Excluir.Enabled = ativar;
        }

        private void Btn_Primeiro_Click_1(object sender, EventArgs e)
        {

        }
    }
}
