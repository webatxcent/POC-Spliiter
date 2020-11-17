using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCENT.JobServer.Abstract;
using XCENT.JobServer.Api.Models;

namespace POC_Splitter
{

    public partial class ParameterVariables : Form
    {

        class GridRow
        {
            public string Global { get;  }
            public string Direction { get; }
            public string Name { get; }
            public string Value { get; }
            public string Description { get; }

            public string Reference { get; }

            public GridRow( bool isGlobal, ModuleParameterDirection? direction, string name, string value, string description ) {

                if ( isGlobal )
                    Global = "\uf0ac";
                else
                    Global = "\uf52c";

                if ( direction == null )
                    Direction = "";
                else if ( direction.Value == ModuleParameterDirection.In )
                    Direction = "\uf309";
                else if ( direction.Value == ModuleParameterDirection.Out )
                    Direction = "\uf30c";
                else if ( direction.Value == ModuleParameterDirection.InOut )
                    Direction = "\uf883"; //"\uf338";

                Name = name;
                Value = value;
                Description = description;

                //TODO: WEB-these should be generated as part of the global or variable class.
                if ( isGlobal )
                    Reference = $"{{::{name}}}";
                else
                    Reference = $"{{{name}}}";

            }
        }

        List<Variable> _variables;
        List<GlobalDtoForView> _globals;
        AppFonts _fonts;

        public string Reference { get; internal set; }

        public ParameterVariables( string caption, List<GlobalDtoForView> globals, List<Variable> variables ) {
            InitializeComponent();

            Text = $"Assign a variable for: \"{caption}\"";

            _variables = variables;
            _globals = globals;

            _fonts = new AppFonts( dgvVariables.Font.Size * 1.2f );

            LoadGrid();
        }

        void LoadGrid() {
            List<GridRow> rows = new List<GridRow>();

            rows.AddRange(
                _globals.Select( m => new GridRow( true, null, m.Symbol, m.Value, m.Description ) ).ToList());

            rows.AddRange(
                _variables.Where( m => ( m.Direction != ModuleParameterDirection.In && !chkIncludeInputVariables.Checked ) || chkIncludeInputVariables.Checked ).Select( m => new GridRow( false, m.Direction, m.Name, null, null ) ).ToList()
                );

            dgvVariables.Columns.Clear();

            dgvVariables.DataSource = rows;

            dgvVariables.Columns[ "Global" ].DefaultCellStyle.Font = new Font( _fonts.FARegular, FontStyle.Regular );
            dgvVariables.Columns[ "Direction" ].DefaultCellStyle.Font = new Font( _fonts.FARegular, FontStyle.Regular );
            dgvVariables.Columns[ "Global" ].HeaderCell.Value = "";
            dgvVariables.Columns[ "Direction" ].HeaderCell.Value = "";

            dgvVariables.Columns[ "Reference" ].Visible = false;

        }

        private void chkIncludeInputVariables_CheckedChanged( object sender, EventArgs e ) {
            LoadGrid();
        }

        private void dgvVariables_CellDoubleClick( object sender, DataGridViewCellEventArgs e ) {
            if (e.RowIndex >= 0 ) {
                Reference = dgvVariables.Rows[e.RowIndex].Cells["Reference"].Value.ToString();
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }

    class GridRow
    {
        string Global { get; }
        string Direction { get; }
        string Name { get; }
        string Value { get; }
        string Description { get; }

        public GridRow( bool isGlobal, ModuleParameterDirection? direction, string name, string value, string description ) {

            if ( isGlobal )
                Global = "G";
            else
                Global = "V";

            if ( direction == null )
                Direction = "";
            else if ( direction.Value == ModuleParameterDirection.In )
                Direction = "In";
            else if ( direction.Value == ModuleParameterDirection.Out )
                Direction = "Out";
            else if ( direction.Value == ModuleParameterDirection.InOut )
                Direction = "InOut";

            Name = name;
            Value = value;
            Description = description;
        }
    }





}
