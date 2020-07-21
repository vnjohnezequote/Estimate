// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientDataBase.cs" company="John Nguyen">
//   John Nguyen
// </copyright>
// <summary>
//   Defines the ClientDataBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using AppModels.Enums;
using AppModels.PocoDataModel;
using AppModels.ResponsiveData;

namespace AppDataBase.DataBase
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using AppModels;
    using CsvHelper;
    using LiteDB;

    /// <summary>
    /// The clientPoco data base.
    /// </summary>
    public class ClientDataBase
    {
        #region Private Member
        /// <summary>
        /// The db.
        /// </summary>
        //private readonly LiteDatabase _db;



        /// <summary>
        /// The clients.
        /// </summary>
        private readonly ILiteCollection<ClientPoco> _clients;


        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDataBase"/> class.
        /// </summary>
        public ClientDataBase()
        {
            using (var db = new LiteDatabase("filename=Clients.Db;upgrade=true"))
            {
                this._clients = db.GetCollection<ClientPoco>("Clients");
                this.Clients = this.GetClients();
                this.DataBaseUpdated += this.ClientDataUpdated;
            }
            //this._db = new LiteDatabase("Clients.Db");

           //LiteDatabase db;
            //this._db = new LiteDatabase("filename=Clients.Db;upgrade=true");
            
        }

        #endregion



        /// <summary>
        /// The data base updated.
        /// </summary>
        public event EventHandler DataBaseUpdated;

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        public List<ClientPoco> Clients { get; set; }

        /// <summary>
        /// The get clients.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ClientPoco> GetClients()
        {
            this.CreateTestDataBase();
            return this._clients.FindAll().ToList();
        }

        /// <summary>
        /// The get clientPoco.
        /// </summary>
        /// <param name="clientName">
        /// The clientPoco name.
        /// </param>
        /// <returns>
        /// The <see cref="ClientPoco"/>.
        /// </returns>
        public ClientPoco GetClient(string clientName)
        {
            if (string.IsNullOrEmpty(clientName))
            {
                return null;
            }

            if (this.Clients.Exists(x => x.Name == clientName))
            {
                //var result = this.Clients.FindOne(x => x.Name == clientName);

                return Clients.FirstOrDefault(client => client.Name == clientName);

                //return result;
            }

            this.CreateClient(clientName);
            return Clients.FirstOrDefault(client => client.Name == clientName);
            //var clientPoco = this.Clients.FindOne(x => x.Name == clientName);
            //return clientPoco;
        }

        /// <summary>
        /// The create clientPoco.
        /// </summary>
        /// <param name="clientName">
        /// The clientPoco name.
        /// </param>
        /// <param name="builders">
        /// The builders.
        /// </param>
        /// <param name="clientIcon">
        /// The clientPoco icon.
        /// </param>
        public void CreateClient(string clientName, List<string> builders = null, string clientIcon = "")
        {
            var client = new ClientPoco() { Name = clientName, Builders = new List<string>(), ClientIcon = clientIcon };
            this._clients.Insert(client);
            this._clients.EnsureIndex(x => x.Name);
            this.DataBaseUpdated?.Invoke(this, null);
        }


        /// <summary>
        /// The update clientPoco.
        /// </summary>
        /// <param name="clientPoco">
        /// The clientPoco.
        /// </param>
        public void UpdateClient(ClientPoco clientPoco)
        {
            if (this._clients.Exists(x => x.Name == clientPoco.Name))
            {
                this._clients.Update(clientPoco);
                this.DataBaseUpdated?.Invoke(this, null);

            }
        }

        /// <summary>
        /// The clientPoco data updated.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ClientDataUpdated(object sender, EventArgs e)
        {
            this.Clients = this.GetClients();
        }

        /// <summary>
        /// this will create test data for database, can delete after run application first
        /// </summary>
        private void CreateTestDataBase()
        {
            var client = new ClientPoco()
            {
                Name = ClientNames.Warnervale.ToString(),
                ClientIcon = "AlphabetW",
                WinRates = new List<string>()
                {
                    "N1","N2","N3"
                },
                Treatments = new List<string>()
                {
                    "Untreated", "H2","H2S"
                },
                RoofMaterials = new List<string>()
                {
                    "Sheets","Tiles"
                },
                Builders = new List<string>() { "Jason Home", "Kenvin Home", "Happy Home", "Crazy Home" },
                WallTypes = new List<WallTypePoco>()
                                {
                                    new WallTypePoco() { Id = 0, IsLoadBearingWall = true, IsRaked = false, AliasName = "EXT WALL" },
                                    new WallTypePoco() { Id = 1, IsLoadBearingWall = false, IsRaked = false, AliasName = "INT WALL" },
                                    new WallTypePoco() { Id = 2, IsLoadBearingWall = true, IsRaked = true, AliasName = "EXT RAKED WALL" },
                                    new WallTypePoco() { Id = 3, IsLoadBearingWall = false, IsRaked = true, AliasName = "INT RAKED WALL" }
                                }
            };


            if (this._clients.Exists(x => x.Name == ClientNames.Warnervale.ToString()))
            {
                this._clients.Update(client);
            }
            else
            {
                this._clients.Insert(client);
                this._clients.EnsureIndex(x => x.Name);
            }


            client = new ClientPoco()
            {
                Name = ClientNames.Prenail.ToString(),
                ClientIcon = "AlphabetP",
                WinRates = new List<string>()
                {
                    "N1","N2","N3","N4","N5","N6","C1","C2","C3","C4"
                },
                Treatments = new List<string>()
                {
                    "Untreated", "H2","H2S Treated","T2 Blue","T2 Red","T3 Green"
                },
                RoofMaterials = new List<string>()
                {
                    "Sheets","Tiles"
                },
                Builders = new List<string>() { "Funny Home", "Teddy Home", "America Home", "Dalat Home" },
                Studs = this.CreatePrenailStuds(),
                RibbonPlates = this.CreatePrenailRibbonPlates(),
                TopPlates = this.CreatPrenailTopPlates(),
                BottomPlates = this.CreatePrenailBottomPlates(),
                Beams = this.CreatePrenailBeamList(),
                TimberBracingBases = this.CreatePrenailBracingList(),
                WallTypes = new List<WallTypePoco>()
                                {
                                    new WallTypePoco() { Id = 0, IsLoadBearingWall = true, IsRaked = false, AliasName = "LB_External" },
                                    new WallTypePoco() { Id = 1, IsLoadBearingWall = true, IsRaked = true, AliasName = "LB_ExtRaking" },
                                    new WallTypePoco() { Id = 2, IsLoadBearingWall = true, IsRaked = false, AliasName = "LB_Internal" },
                                    new WallTypePoco() { Id = 3, IsLoadBearingWall = true, IsRaked = false, AliasName = "LB_PartiWall" },
                                    new WallTypePoco() { Id = 4, IsLoadBearingWall = true, IsRaked = true, AliasName = "LB_Raked_PartiWall" },
                                    new WallTypePoco() { Id = 5, IsLoadBearingWall = false, IsRaked = false, AliasName = "NLB_Internal" },
                                    new WallTypePoco() { Id = 6, IsLoadBearingWall = false, IsRaked = true, AliasName = "NLB_IntRaking" },
                                    new WallTypePoco() { Id = 7, IsLoadBearingWall = false, IsRaked = false, AliasName = "NLB_PartiWall" },
                                    new WallTypePoco() { Id = 8, IsLoadBearingWall = false, IsRaked = true, AliasName = "NLB_Raked_PartiWall" }
            }
            };

            if (this._clients.Exists(x => x.Name == ClientNames.Prenail.ToString()))
            {
                this._clients.Update(client);
            }
            else
            {
                this._clients.Insert(client);
                this._clients.EnsureIndex(x => x.Name);
            }

            client = new ClientPoco()
            {
                Name = ClientNames.Rivo.ToString(),
                ClientIcon = "AlphabetR",
                WinRates = new List<string>()
                {
                    "N1","N2","N3"
                },
                Treatments = new List<string>()
                {
                    "Untreated", "H2","H2S Treated"
                },
                RoofMaterials = new List<string>()
                {
                    "Sheets","Tiles"
                },
                Builders = new List<string>() { "nha 1", "nha 2", "nha 3", "nha vang vang", "Nha Kiem Tra xem thu da duoc chua" },

                WallTypes = new List<WallTypePoco>
                            {
                new WallTypePoco(){Id = 0, IsLoadBearingWall = true, IsRaked = false, AliasName = "EXTERNAL LOAD BEARING"},
                new WallTypePoco(){Id = 1, IsLoadBearingWall = true, IsRaked = false, AliasName = "INTERNAL LOAD BEARING"},
                new WallTypePoco(){Id = 2, IsLoadBearingWall = false, IsRaked = false, AliasName = "EXTERNAL NON LOAD BEARING"},
                new WallTypePoco(){Id = 3, IsLoadBearingWall = false, IsRaked = false, AliasName = "INTERNAL NON LOAD BEARING"},
                new WallTypePoco(){Id = 4, IsLoadBearingWall = false, IsRaked = true, AliasName = "EXT RAKING WALL"},
                new WallTypePoco(){Id = 5, IsLoadBearingWall = false, IsRaked = true, AliasName = "INT RAKING WALL"},
                new WallTypePoco(){Id = 6, IsLoadBearingWall = false, IsRaked = false, AliasName = "PARAPET WALL"},
            }

            };

            if (this._clients.Exists(x => x.Name == ClientNames.Rivo.ToString()))
            {
                this._clients.Update(client);
            }
            else
            {
                this._clients.Insert(client);
                this._clients.EnsureIndex(x => x.Name);
            }

            client = new ClientPoco()
            {
                Name = ClientNames.StickFrame.ToString(),
                ClientIcon = "AlphabetS",
                WinRates = new List<string>()
                {
                    "N1","N2","N3","N4","N5","N6","C1","C2","C3","C4"
                },
                Treatments = new List<string>()
                {
                    "Untreated", "H2","H2S Treated", "H2F Treated","H3 Treated","T2 Treated","T2 Blue Treated", "T2 Red Treated","T3 Green Treated","LOSP"
                },
                RoofMaterials = new List<string>()
                {
                    "Sheets","Tiles"
                },
                Builders = new List<string>() { "Privium", "Privium Homes" },

                WallTypes = new List<WallTypePoco>
                {
                    new WallTypePoco(){Id = 0, IsLoadBearingWall = true, IsRaked = false, AliasName = "LOAD BEARING WALL"},
                    new WallTypePoco(){Id = 1, IsLoadBearingWall = true, IsRaked = false, AliasName = "RAKED LOAD BEARING WALL"},
                    new WallTypePoco(){Id = 2, IsLoadBearingWall = true, IsRaked = false, AliasName = "NONE LOAD BEARING WALL"},
                    new WallTypePoco(){Id = 3, IsLoadBearingWall = true, IsRaked = false, AliasName = "RAKED NONE LOAD BEARING WALL"}
                }

            };

            if (this._clients.Exists(x => x.Name == ClientNames.StickFrame.ToString()))
            {
                this._clients.Update(client);
            }
            else
            {
                this._clients.Insert(client);
                this._clients.EnsureIndex(x => x.Name);
            }

            client = new ClientPoco()
            {
                Name = ClientNames.ITMTumu.ToString(),
                ClientIcon = "AlphabetI",
                WinRates = new List<string>()
                {
                    "Extra High","Very High","High Wind","Medium Wind","Low Wind"
                },
                Treatments = new List<string>()
                {
                    "Untreated", "H2","H2S Treated", "H2F Treated","H3 Treated","T2 Treated","T2 Blue Treated", "T2 Red Treated","T3 Green Treated","LOSP"
                },
                RoofMaterials = new List<string>()
                {
                    "LongRun","Tiles"
                },
                Builders = new List<string>() { "nha 1", "nha 2", "nha 3", "nha vang vang", "Nha Kiem Tra xem thu da duoc chua" },

                WallTypes = new List<WallTypePoco>
                {
                    new WallTypePoco(){Id = 0, IsLoadBearingWall = true, IsRaked = false, AliasName = "LOAD BEARING WALL"},
                    new WallTypePoco(){Id = 1, IsLoadBearingWall = true, IsRaked = false, AliasName = "RAKED LOAD BEARING WALL"},
                    new WallTypePoco(){Id = 2, IsLoadBearingWall = true, IsRaked = false, AliasName = "NONE LOAD BEARING WALL"},
                    new WallTypePoco(){Id = 3, IsLoadBearingWall = true, IsRaked = false, AliasName = "RAKED NONE LOAD BEARING WALL"}
                }

            };

            if (this._clients.Exists(x => x.Name == ClientNames.ITMTumu.ToString()))
            {
                this._clients.Update(client);
            }
            else
            {
                this._clients.Insert(client);
                this._clients.EnsureIndex(x => x.Name);
            }





        }

        /// <summary>
        /// The create prenail dict stud.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        private Dictionary<string, List<TimberBase>> CreatePrenailStuds()
        {
            var lbwFile = "LBWStudList.csv";
            var nonlbwFile = "NonLBWStudList.csv";
            var nonlbw = this.LoadTimberListOnCvsFile<TimberBase>(nonlbwFile);
            var lbw = this.LoadTimberListOnCvsFile<TimberBase>(lbwFile);
            Dictionary<string, List<TimberBase>> studs = new Dictionary<string, List<TimberBase>>();
            studs.Add("LBW", lbw);
            studs.Add("NONLBW", nonlbw);
            return studs;
        }

        /// <summary>
        /// The create Prenail top plate.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private Dictionary<string, List<TimberBase>> CreatePrenailRibbonPlates()
        {
            var thick90File = "90RibonPlateList.csv";
            var thick70File = "70RibonPlateList.csv";
            var wallThick90 = this.LoadTimberListOnCvsFile<TimberBase>(thick90File);
            var wallThick70 = this.LoadTimberListOnCvsFile<TimberBase>(thick70File);
            Dictionary<string, List<TimberBase>> ribbonPlates = new Dictionary<string, List<TimberBase>>();
            ribbonPlates.Add("90", wallThick90);
            ribbonPlates.Add("70", wallThick70);
            return ribbonPlates;
        }

        /// <summary>
        /// The create Prenail top plate.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private Dictionary<string, List<TimberBase>> CreatPrenailTopPlates()
        {
            var thick90File = "90TopPlateList.csv";
            var thick70File = "70TopPlateList.csv";
            var wallThick90 = this.LoadTimberListOnCvsFile<TimberBase>(thick90File);
            var wallThick70 = this.LoadTimberListOnCvsFile<TimberBase>(thick70File);
            Dictionary<string, List<TimberBase>> topPlates = new Dictionary<string, List<TimberBase>>();
            topPlates.Add("90", wallThick90);
            topPlates.Add("70", wallThick70);
            return topPlates;
        }

        /// <summary>
        /// The create prenail beam list.
        /// </summary>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        private Dictionary<string, List<TimberBase>> CreatePrenailBeamList()
        {
            var mgp12File = "MGP12.csv";
            var gl17CFile = "GL17C.csv";
            var gl18CFile = "GL18C.csv";
            var f27File = "F27.csv";
            var lvl13File = "LVl13.csv";
            Dictionary<string, List<TimberBase>> beams = new Dictionary<string, List<TimberBase>>();
            beams.Add("MGP_12", this.LoadTimberListOnCvsFile<TimberBase>(mgp12File));
            beams.Add("LVL_13", this.LoadTimberListOnCvsFile<TimberBase>(lvl13File));
            beams.Add("GL_17C", this.LoadTimberListOnCvsFile<TimberBase>(gl17CFile));
            beams.Add("GL_18C", this.LoadTimberListOnCvsFile<TimberBase>(gl18CFile));
            beams.Add("F27_Hwd", this.LoadTimberListOnCvsFile<TimberBase>(f27File));
            return beams;
        }

        /// <summary>
        /// The create prenail bracing list.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<BracingBase> CreatePrenailBracingList()
        {
            var bracingFile = "BracingList.csv";
            return this.LoadTimberListOnCvsFile<BracingBase>(bracingFile);
        }

        /// <summary>
        /// The load list on cvs file.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        private List<T> LoadTimberListOnCvsFile<T>(string file)
        {
            var reader = new StreamReader(file);
            var csvFile = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvFile.Configuration.HeaderValidated = null;
            csvFile.Configuration.MissingFieldFound = null;
            return csvFile.GetRecords<T>().ToList();

        }
        
        

        /// <summary>
        /// The create Prenail bottom plate.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private Dictionary<string, List<TimberBase>> CreatePrenailBottomPlates()
        {
            var thick90File = "90BottomPlateList.csv";
            var thick70File = "70BottomPlateList.csv";
            var wallThick90 = this.LoadTimberListOnCvsFile<TimberBase>(thick90File);
            var wallThick70 = this.LoadTimberListOnCvsFile<TimberBase>(thick70File);
            Dictionary<string, List<TimberBase>> bottomPlates = new Dictionary<string, List<TimberBase>>();
            bottomPlates.Add("90", wallThick90);
            bottomPlates.Add("70", wallThick70);
            return bottomPlates;
        }
    }
}
