using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baguio_Pad
{
    public partial class Notepad : Form
    {
        public Notepad()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Open";
            op.Filter = "Text Document(*.txt)|*.txt| All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
                richTextBox1.LoadFile(op.FileName, RichTextBoxStreamType.PlainText);
            this.Text = op.FileName;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.Title = "Save";
            op.Filter = "Text Document(*.txt)|*.txt| All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
                richTextBox1.SaveFile(op.FileName, RichTextBoxStreamType.PlainText);
            this.Text = op.FileName;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Do you want to close this window?";
            string title = "Notepad";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                string firstLine = richTextBox1.Lines.Length > 0 ? richTextBox1.Lines[0] : string.Empty;

                if (!string.IsNullOrEmpty(firstLine))
                {
                    string message2 = $"Do you want to save changes to {firstLine}.txt?";
                    string title2 = "Notepad";
                    MessageBoxButtons buttons2 = MessageBoxButtons.YesNoCancel;
                    DialogResult result2 = MessageBox.Show(message2, title2, buttons2);

                    if (result2 == DialogResult.Yes)
                    {
                        SaveFileDialog op = new SaveFileDialog();
                        op.Title = "Save";
                        op.Filter = "Text Document(*.txt)|*.txt| All Files(*.*)|*.*";
                        op.FileName = firstLine;
                        if (op.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(op.FileName, RichTextBoxStreamType.PlainText);
                            this.Text = op.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (result2 == DialogResult.No)
                    {
                        // If user chooses No, close the form without saving
                        this.Close();
                    }
                    else if (result2 == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    this.Close(); //When the No in Close Window is pressed
                }
            }
            else
            {
                return;
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = System.DateTime.Now.ToString();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog op = new FontDialog();
            if (op.ShowDialog() == DialogResult.OK)
                richTextBox1.Font = op.Font;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog op = new ColorDialog();
            if (op.ShowDialog() == DialogResult.OK)
                richTextBox1.ForeColor = op.Color;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load Shortcuts
            undoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
            redoToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
            copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            cutToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.X;
            selectAllToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            dateTimeToolStripMenuItem.ShortcutKeys = Keys.F5;
            
            // Initialize strip label with "Line, Col, and Characters"
            toolStripStatusLabel1.Text = $"Line 1, Col 1  |";
            toolStripStatusLabel2.Text = $"0 characters";

            // Wire up events for richTextBox1
            richTextBox1.TextChanged += richTextBox1_TextChanged;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int index = richTextBox1.SelectionStart;
            int line = richTextBox1.GetLineFromCharIndex(index);
            int col = index - richTextBox1.GetFirstCharIndexOfCurrentLine();
            int charCount = richTextBox1.Text.Length;

            toolStripStatusLabel1.Text = $"Line {line + 1}, Col {col + 1}  |";
            toolStripStatusLabel2.Text = $"{charCount} characters";
        }

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            string firstLine = richTextBox1.Lines.Length > 0 ? richTextBox1.Lines[0] : string.Empty;
            string message = $"Do you want to save changes to {firstLine}.txt?";
            string title = "Notepad";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;

            if (!string.IsNullOrEmpty(firstLine))
            {
                DialogResult result = MessageBox.Show(message, title, buttons);

                if (result == DialogResult.Yes)
                {
                    SaveFileDialog op = new SaveFileDialog();
                    op.Title = "Save";
                    op.Filter = "Text Document(*.txt)|*.txt| All Files(*.*)|*.*";
                    op.FileName = firstLine;
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox1.SaveFile(op.FileName, RichTextBoxStreamType.PlainText);
                        this.Text = op.FileName;
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
    }
}
