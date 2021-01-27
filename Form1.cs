using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCENT.JobServer.Abstract;
using XCENT.JobServer.Model;
using XCENT.JobServer.Manager.App;


namespace POC_Splitter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            SetStepToEdit( 1 );
        }

        void SetStepToEdit( int editStep ) {
            IModule module = null;
            Parameters parameters = null;
            List<Variable> variables = new List<Variable>();


            if ( editStep == 0 ) {
                if ( Program.job.HasATrigger && !Program.job.UsesScheduler ) {
                    module = (IModule)Program.modules.FirstOrDefault( m => m.Id == Program.job.TriggerModuleId && m.Type == ModuleType.Trigger );
                    parameters = Program.job.Parameters;
                }
            }
            else {
                StepBase step = Program.job.FindStepByNumber(editStep);
                module = (IModule)Program.modules.FirstOrDefault( m => m.Id == step.ExecutionModuleId && m.Type == ModuleType.Executable );
                parameters = step.Parameters;
            }

            if ( module != null ) {
                //build list of variables based on step you are on.
                List<IModule> modules = Program.modules.Select( m => m as IModule ).ToList<IModule>();
                if ( editStep > 0 )
                    SyntaxChecker.GetVariablesForStep( Program.job, modules, (int)editStep - 1, variables );

                //Givens:
                //1. parameter defs for the module in question. Module can be a Trigger.
                //2. any existing parameter values.
                //3. variables
                //4. globals

                parameterContainer1.LoadParameterData( module.ParameterDefs, parameters, variables, Program.globals );
            }
            else {
                parameterContainer1.LoadParameterData( new List<ParameterDef>(), new Parameters(), new List<Variable>(), Program.globals );
            }

            if ( parameterContainer1.PreferredHeight > parameterContainer1.Height ) {
                var adjust = parameterContainer1.PreferredHeight - parameterContainer1.Height;
                Height = this.Height + adjust;
            }

            txtLanding.Focus();

        }



        private void btnTrigger_Click( object sender, EventArgs e ) {
            SetStepToEdit( 0 );
        }

        private void btnStep1_Click( object sender, EventArgs e ) {
            SetStepToEdit( 1 );
        }

        private void btnStep2_Click( object sender, EventArgs e ) {
            SetStepToEdit( 2 );
        }

        private void btnStep3_Click( object sender, EventArgs e ) {
            SetStepToEdit( 3 );
        }

        private void btnShowParams_Click( object sender, EventArgs e ) {
            MessageBox.Show( parameterContainer1.Parameters.ToJson() );
        }

        private void btnIsValid_Click( object sender, EventArgs e ) {
            string valid = parameterContainer1.IsValid ? "Valid" : "Invalid";
            string changed = parameterContainer1.HasChanged ? "were" : "were not";
            MessageBox.Show( $"Data is {valid }. Changes {changed} made." );
        }

    }
}
