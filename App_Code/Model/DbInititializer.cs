using System;
using System.IO;
using System.Xml;
using System.Data.Entity;
using System.Collections.Generic;

using CompareCity.Control;

namespace CompareCity.Model
{
    /// <summary>
    /// Set initial data for the application database.
    /// </summary>
    public class DbInititializer :DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        private string serverHome;

        public DbInititializer(string serverHomeDir)
        {
            serverHome = serverHomeDir;
        }

        protected override void Seed(DatabaseContext db) 
        {
            base.Seed(db);

            // Load scoring identifiers.
            getScoringIdentifiers().ForEach(s => db.ScoringIdentifiers.Add(s));
            db.SaveChanges();
        }

        private List<ScoringIdentifier> getScoringIdentifiers()
        {
            List<ScoringIdentifier> scoringIdentifiers = new List<ScoringIdentifier>();

            string xmlFilepath = String.Format("{0}{1}", serverHome, SiteControl.ScoringIdentifierFilepath);

            // Parse xml file containing our scoring identifiers.
            using (XmlReader reader = XmlReader.Create(xmlFilepath))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement() && reader.Name.Equals("Identifier"))
                    {
                        scoringIdentifiers.Add(parseIdentifier(reader));
                    }
                }
            }

            return scoringIdentifiers;
        }

        private ScoringIdentifier parseIdentifier(XmlReader reader)
        {
            string name = "";
            string shortName = "";
            string description = "";

            reader.Read();
            while (!"Identifier".Equals(reader.Name))
            {
                switch (reader.Name) 
                {
                    case "Name":
                        name = reader.ReadElementContentAsString();
                        break;
                    case "ShortName":
                        shortName = reader.ReadElementContentAsString();
                        break;
                    case "Description":
                        description = reader.ReadElementContentAsString();
                        break;
                }

                reader.Read();
            }

            return new ScoringIdentifier
            {
                Name = name,
                ShortName = shortName,
                Descrition = description
            };
        }
    }
}