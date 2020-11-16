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


namespace POC_Splitter {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            SetStepToEdit( 0 );
        }

        void SetStepToEdit( int editStep ) {
            IModule module = null;
            Parameters parameters = null;
            List<Variable> variables = new List<Variable>();


            if ( editStep == 0 ) {
                if ( Program.job.HasATrigger && !Program.job.UsesScheduler ) {
                    module = (IModule)Program.modules.First( m => m.Id == Program.job.TriggerModuleId && m.Type == ModuleType.Trigger );
                    parameters = Program.job.Parameters;
                }
            }
            else {
                StepBase step = Program.job.FindStepBaseByNumber((long)editStep);
                module = (IModule)Program.modules.First( m => m.Id == step.ExecutionModuleId && m.Type == ModuleType.Executable );
                parameters = step.Parameters;
            }

            List<IModule> modules = Program.modules.Select( m => m as IModule ).ToList<IModule>();
            if ( editStep > 0 )
                SyntaxChecker.GetVariablesForStep( Program.job, modules, (int)editStep - 1, variables );

            parameterContainer1.LoadParameterData( module.ParameterDefs, parameters, variables, Program.globals );
            parameterContainer1.Focus();
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
    }
}
