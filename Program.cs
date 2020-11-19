using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCENT.JobServer.Api;
using XCENT.JobServer.Api.Models;

namespace POC_Splitter
{

    /*
        To protect credentials, I created a class that is not part of the repository which you must supply in
        your local copy.

        Here is a template for that code. Copy and paste it into a local file you call Credentials.cs,
        it is already part of .gitignore and will not be posted to this public repo.

        
        //BEGIN COPY -----------------------------------------------------
        using XCENT.JobServer.Api;

        namespace POC_Splitter
        {
            class Credentials
            {
                static public JobServer JobServer() {

                    JobServer jobServer = new JobServer( "jobserver instance" );
                    jobServer.Connect( @"username", "password" );
                    return jobServer;
                }
            }
        }
        //END COPY -------------------------------------------------------
    */

    static class Program
    {

        static public JobDtoForView job;
        static public List<GlobalDtoForView> globals;
        static public List<ModuleDtoForView> modules;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main() {

            JobServer jobServer = null;

            try {
                jobServer = Credentials.JobServer();
            }
            catch ( Exception ex ) {
                MessageBox.Show( $"Could not connect to JobServer. {ex}" );
                return;
            }

            job = jobServer.Jobs.FindById( 14 );

            globals = jobServer.Globals.ListAll();
            modules = jobServer.Executors.ListAll();
            modules.AddRange( jobServer.Triggers.ListAll() );


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new Form1() );
        }
    }
}
