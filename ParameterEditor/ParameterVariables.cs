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
using XCENT.JobServer.Model;

namespace XCENT.JobServer.Manager.App
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

            public GridRow( bool isGlobal, ModuleParameterDirection? direction, string name, string value, string description, string reference ) {

                if ( isGlobal )
                    Global = FontAwesome.Globe;
                else
                    Global = FontAwesome.Equals;

                if ( direction == null )
                    Direction = "";
                else if ( direction.Value == ModuleParameterDirection.In )
                    Direction = FontAwesome.LongArrowAltDown;
                else if ( direction.Value == ModuleParameterDirection.Out )
                    Direction = FontAwesome.LongArrowAltUp;
                else if ( direction.Value == ModuleParameterDirection.InOut )
                    Direction = FontAwesome.SortAlt;

                Name = name;
                Value = value;
                Description = description;
                Reference = reference;
            }

        }

        List<Variable> _variables;
        List<GlobalDtoForView> _globals;
        AppFonts _fonts;
        bool _isLoading = false;

        public string Reference { get; internal set; }

        public ParameterVariables( string caption, string currentValue, List<GlobalDtoForView> globals, List<Variable> variables ) {
            InitializeComponent();

            Text = $"Assign a variable for: \"{caption}\"";

            _variables = variables;
            _globals = globals;

            _fonts = new AppFonts( dgvVariables.Font.Size * 1.2f );

            LoadGrid( currentValue );
        }

        void LoadGrid( string valueSelector = null ) {
            List<GridRow> rows = new List<GridRow>();

            _isLoading = true;

            try {

                //determine first if the initial value selector is among the normally filtered out IN variables, if so then set the checkbox.
                if ( !String.IsNullOrEmpty( valueSelector ) )
                    chkIncludeInputVariables.Checked = _variables.Any( m => m.Direction == ModuleParameterDirection.In && m.Reference() == valueSelector );

                rows.AddRange(
                     _globals.Select( m => new GridRow( true, null, m.Symbol, m.Value, m.Description, $"{{{{::{m.Symbol}}}}}" ) ).ToList() );

                rows.AddRange(
                    _variables.Where( m => ( m.Direction != ModuleParameterDirection.In && !chkIncludeInputVariables.Checked ) || chkIncludeInputVariables.Checked ).Select( m => new GridRow( false, m.Direction, $"{m.ModuleName}[{m.StepNumber}].{m.Name}", "", "", $"{{{{{m.Name}}}}}" ) ).ToList()
                    );


                dgvVariables.Columns.Clear();

                dgvVariables.AllowUserToResizeColumns = true;
                dgvVariables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvVariables.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                dgvVariables.DataSource = rows;

                dgvVariables.Columns[ "Global" ].DefaultCellStyle.Font = new Font( _fonts.FARegular, FontStyle.Regular );
                dgvVariables.Columns[ "Direction" ].DefaultCellStyle.Font = new Font( _fonts.FARegular, FontStyle.Regular );
                dgvVariables.Columns[ "Global" ].HeaderCell.Value = "";
                dgvVariables.Columns[ "Direction" ].HeaderCell.Value = "";

                dgvVariables.Columns[ "Reference" ].Visible = false;

                dgvVariables.AutoResizeColumns();

                dgvVariables.AllowUserToResizeRows = false;

                if ( String.IsNullOrEmpty( valueSelector ) )
                    return;

                dgvVariables.ClearSelection();
                foreach ( DataGridViewRow r in dgvVariables.Rows )
                    if ( r.Cells[ "Reference" ].Value.ToString() == valueSelector ) {
                        dgvVariables.CurrentCell = r.Cells[ 0 ];
                        r.Selected = true;
                        break;
                    }

                ///dgvVariables.Rows[ 1 ].Selected = true;

            }
            finally {
                _isLoading = false;
            }
        }

        void SelectVariableAndClose( int rowIndex ) {
            Reference = dgvVariables.Rows[ rowIndex ].Cells[ "Reference" ].Value.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }


        private void chkIncludeInputVariables_CheckedChanged( object sender, EventArgs e ) {
            if ( _isLoading )
                return;
            LoadGrid();
        }

        private void dgvVariables_CellDoubleClick( object sender, DataGridViewCellEventArgs e ) {
            if (e.RowIndex >= 0 )
                SelectVariableAndClose( e.RowIndex );
        }

        private void btnSelect_Click( object sender, EventArgs e ) {
            SelectVariableAndClose( dgvVariables.SelectedRows[ 0 ].Cells[ 0 ].RowIndex );
        }

        private void btnClear_Click( object sender, EventArgs e ) {
            Reference = "";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void dgvVariables_KeyDown( object sender, KeyEventArgs e ) {
            if ( e.KeyCode == Keys.Enter )
                SelectVariableAndClose( dgvVariables.SelectedRows[ 0 ].Cells[ 0 ].RowIndex );
        }
    }

    static class VariablesExtension
    {
        static public string Reference( this Variable variable ) {
            return $"{{{{{variable.Name}}}}}";
        }


        static public string Reference( this IGlobal global ) {
            return $"{{{{::{global.Symbol}}}}}";
        }

    }





}
